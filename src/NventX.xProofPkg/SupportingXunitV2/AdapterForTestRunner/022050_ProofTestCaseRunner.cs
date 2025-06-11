using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.xProof.SupportingXunit.AdapterForTestRunner
{
    /// <summary>
    /// Runs a test case that expects an exception to be thrown.
    /// </summary>
    internal class ProofTestCaseRunner : XunitTestCaseRunner
    {
        private IProofTestCase ProofTestCase { get; }

        private IMessageBus? RealMessageBus { get; set; }
        private ProxyExceptPassedMessageBus? ProxyMessageBus { get; set; }

        private ExceptionAggregator PrivateAggregator { get; } = new ExceptionAggregator();

        private ITest? Test { get; set; }

        private CancellationTokenSource? Cts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFactTestCaseRunner"/> class.
        /// </summary>
        public ProofTestCaseRunner(IProofTestCase testCase, string displayName, IMessageBus messageBus, object[] constructorArguments,
                               object[] testMethodArguments, string skipReason,
                               ExceptionAggregator aggregator,
                               CancellationTokenSource cancellationTokenSource
        ) : base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator, cancellationTokenSource)
        {
            // Store the test case for later use
            ProofTestCase = testCase ?? throw new ArgumentNullException(nameof(testCase));
        }

        /// <summary>
        /// Creates a test runner for the specified test case.
        /// </summary>
        protected override XunitTestRunner CreateTestRunner(
            ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod,
            object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            // keep the message bus for later use
            RealMessageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus), "Message bus cannot be null.");

            // create a proxy message bus that ignores TestPassed messages
            ProxyMessageBus = new ProxyExceptPassedMessageBus(RealMessageBus, cancellationTokenSource);

            // keep the test for later use
            Test = test ?? throw new ArgumentNullException(nameof(test), "Test cannot be null. " +
                "Ensure that the test case implements IProofTestCase and provides a valid test instance.");

            // keep the CancellationTokenSource for later use
            Cts = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource), "Cancellation token source cannot be null.");

            // create a ProofTestInvoker with the proof instance
            var runner = new ProofTestRunner(
                test, ProxyMessageBus, testClass, constructorArguments, testMethod, testMethodArguments,
                skipReason, beforeAfterAttributes, PrivateAggregator, cancellationTokenSource
            );

            // return the runner
            return runner;
        }

        /// <summary>
        /// Runs the test case asynchronously and returns a summary of the run.
        /// </summary>
        protected override async Task<RunSummary> RunTestAsync()
        {
            // execute the test method and verify that the expected exception is thrown
            var summary = await base.RunTestAsync();

            // Check if the RealMessageBus is initialized
            if (RealMessageBus == null)
            {
                throw new InvalidOperationException("RealMessageBus is not initialized. " +
                     "Ensure that CreateTestRunner is called before RunTestAsync.");
            }

            // Check if the ProxyMessageBus is initialized
            if (ProxyMessageBus == null)
            {
                throw new InvalidOperationException("ProxyMessageBus is not initialized. " +
                    "Ensure that CreateTestRunner is called before RunTestAsync.");
            }

            // If the test has failed or was skipped, finalize the test run if it hasn't been finished yet,
            // and return the summary.
            if (ProxyMessageBus.IsFailed || ProxyMessageBus.IsSkipped)
            {
                if (!ProxyMessageBus.IsFinished)
                {
                    RealMessageBus.QueueMessage(
                        new TestFinished(Test, summary.Time, ProxyMessageBus.Output)
                    );
                }
                return summary;
            }

            // Check if the Proof is initialized
            var proof = TestMethodArguments.First()  as IInvokableProof ?? throw new InvalidOperationException("Proof is not initialized. " +
                    "Ensure that CreateTestRunner is called before RunTestAsync.");

            // Check if the CancellationTokenSource is initialized
            if (Cts == null)
            {
                throw new InvalidOperationException("CancellationTokenSource is not initialized. " +
                    "Ensure that CreateTestRunner is called before RunTestAsync.");
            }

            // Collect probing failures from the proof and report them as test failures
            var output = string.Empty;
            var failures = proof.CollectProbingFailure() ?? throw new InvalidOperationException($"CollectProbingFailure() of {proof.GetType().FullName} returned null. " +
                    "This violates the contract of CollectProbingFailure(), which must always return a non - null IEnumerable<Exception>.");

            // If there are no probing failures, we report the exceptions as failures
            foreach (var f in failures)
            {
                // TODO: mesure the time of the probing failure
                var isSuccessQueing = RealMessageBus.QueueMessage(new TestFailed(Test, 0m, f.Exception.Message, f.Exception));
                if (!isSuccessQueing)
                {
                    Cts.Cancel();
                }
            }
            var cntFailures = failures.Count();
            if (cntFailures == 1)
            {
                // If there is only one probing failure, we report it as a single failure
                output = failures.First().Exception.Message;
            }
            else if (1 < cntFailures)
            {
                output = "Multiple probing failures occurred: " +
                         string.Join(" / ", failures.Select(f => f.Exception.Message));
            }

            // If there are no probing failures, we report the test as passed
            if (cntFailures == 0)
            {
                var isSuccessQueing = RealMessageBus.QueueMessage(
                    new TestPassed(Test, summary.Time, output)
                );
                if (!isSuccessQueing)
                {
                    Cts.Cancel();
                }
            }

            // re-calculate the summary to include the probing failures and successes
            summary = new RunSummary
            {
                Total = cntFailures + proof.ProbingSuccessCount,
                Failed = cntFailures,
                Time = summary.Time
            };

            // return the summary of the test run
            return summary;
        }

        /// <summary>
        /// A silent message bus that ignores all messages sent to it.
        /// </summary>
        private class ProxyExceptPassedMessageBus : IMessageBus
        {
            IMessageBus OriginalBus { get; }
            CancellationTokenSource Cts { get; }
            public bool IsFailed { get; private set; } = false;
            public bool IsSkipped { get; private set; } = false;
            public bool IsFinished { get; private set; } = false;
            public string Output { get; private set; } = string.Empty;
            public ProxyExceptPassedMessageBus(IMessageBus originalBus, CancellationTokenSource cts)
            {
                // Validate the original message bus and store it
                OriginalBus = originalBus ?? throw new ArgumentNullException(nameof(originalBus), "Original message bus cannot be null.");

                // Validate the cancellation token source and store it
                Cts = cts ?? throw new ArgumentNullException(nameof(cts), "Cancellation token source cannot be null.");
            }
            public void Dispose()
            {
                // No resources to dispose of in this implementation
            }

            public bool QueueMessage(IMessageSinkMessage message)
            {
                // Queue the message internally, but ignore TestPassed messages
                if (InternalQueueMessage(message))
                {
                    // If the message was successfully queued, we return true
                    return true;
                }
                // If the message was not queued, cancel the operation and return false
                Cts.Cancel();
                return false;
            }

            private bool InternalQueueMessage(IMessageSinkMessage message)
            {
                // Proxy messages by condition except TestPassed
                if (message is TestPassed)
                {
                    return true; // Just Ignore the message and return true
                }
                if (message is TestFinished && (IsFailed || IsSkipped))
                {
                    // If the message is a TestFinished and we have failed,
                    // we mark it as finished and proxy the message to the original bus
                    IsFinished = true;
                    return OriginalBus.QueueMessage(message);
                }
                if (message is TestFailed tf)
                {
                    // If the message is a TestFailed, we mark it as failed
                    IsFailed = true;
                    Output = tf.Output ?? string.Empty;
                    return OriginalBus.QueueMessage(message);
                }
                if (message is TestSkipped)
                {
                    // If the message is a TestSkipped, we mark it as skipped
                    IsSkipped = true;
                    return OriginalBus.QueueMessage(message);
                }

                // otherwise, just proxy the message to the original bus
                return OriginalBus.QueueMessage(message);
            }
        }
    }
}
