using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.xProof.SupportingXunit.AdapterForTestRunner
{
    /// <summary>
    /// Represents a test case runner for tests that expect a proof to be verified during their execution.
    /// </summary>
    internal class ProofTestCaseRunner : XunitTestCaseRunner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFactTestCaseRunner"/> class.
        /// </summary>
        public ProofTestCaseRunner(IProofTestCase testCase, string displayName, IMessageBus messageBus, object[] constructorArguments,
                               object[] testMethodArguments, string skipReason,
                               ExceptionAggregator aggregator,
                               CancellationTokenSource cancellationTokenSource
        ) : base(testCase, displayName, skipReason, constructorArguments, 
                 testMethodArguments, /* new ProofCoordinatorBus(messageBus, cancellationTokenSource) */ messageBus, aggregator, cancellationTokenSource)
        {
            // No additional initialization is needed here,
        }

        /// <summary>
        /// Creates a test runner for the specified test case.
        /// </summary>
        protected override XunitTestRunner CreateTestRunner(
            ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod,
            object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            // create a ProofTestInvoker with the proof instance
            var runner = new ProofTestRunner(
                test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
                skipReason, beforeAfterAttributes, aggregator, cancellationTokenSource
            );

            // return the runner
            return runner;
        }

        /*
        protected override async Task BeforeTestCaseFinishedAsync()
        {
            // Call the base method to ensure any necessary pre-finalization logic is executed
            await base.BeforeTestCaseFinishedAsync();

            // Check if the ProxyMessageBus is initialized
            if (ProxyMessageBus == null)
            {
                throw new InvalidOperationException("ProxyMessageBus is not initialized. " +
                    "Ensure that CreateTestRunner is called before RunTestAsync.");
            }

            // return if the test case has already finished
            if (ProxyMessageBus.IsTestCaseFinished)
            {
                Console.WriteLine($"test case already finished: {TestMethod.Name}");
                return;
            }

            // Check if the RealMessageBus is initialized
            if (RealMessageBus == null)
            {
                throw new InvalidOperationException("RealMessageBus is not initialized. " +
                     "Ensure that CreateTestRunner is called before RunTestAsync.");
            }

            RealSummary = new RunSummary()
            {
                Total = 1,
                Failed = 0,
                Skipped = ProxyMessageBus.IsTestSkipped ? 1 : 0,
                Time = 0m
            };

            try
            {
                // Check if the Proof is initialized
                var proof = TestMethodArguments.First() as IInvokableProof ?? throw new InvalidOperationException("Proof is not initialized. " +
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
                    ProxyMessageBus.IsTestFailed = true;
                    var isSuccessQueuing = RealMessageBus.QueueMessage(new TestFailed(Test, 0m, f.Exception.Message + $": {failures.Count()}", f.Exception));
                    if (!isSuccessQueuing)
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
                else if (cntFailures == 0)
                {
                    // Ntothing to report, we assume the proof was successful, and will be reported as a passed case later.
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Unexpected number of probing failures: {cntFailures}. " +
                        "This should not happen, as the proof should always return at least one failure or success."
                    );
                }
                // Store the output for later use
                Console.WriteLine($"setting Runsummary.Failed = {cntFailures} ({TestCase.Method.Name})");
                RealSummary.Total = cntFailures + proof.ProbingSuccessCount;
                RealSummary.Failed = cntFailures;
            }
            catch (Exception ex)
            {
                // If an exception occurs, we finalize the test case with a failure
                RealSummary.Failed = 1;
                RealSummary.Time = 0m;
                throw new XunitException(
                    $"An error occurred while finalizing the test case: {ex.Message}", ex
                );
            } 
        }

        /// <summary>
        /// Runs the test case asynchronously and returns a summary of the run.
        /// </summary>
        protected override async Task<RunSummary> RunTestAsync()
        {
            // Ensure that RealSummary is null before running the test
            RealSummary = null;

            // Running the test case
            var summary = await base.RunTestAsync();

            Aggregator.Run(() => {
                // RealSummary was created by the BeforeTestCaseFinishedAsync(), return it as the result.
                if (RealSummary != null)
                {
                    Console.WriteLine($"getting RealSummary: {TestCase.TestMethod.Method.Name}");
                    // If RealSummary is not null, we return it as the result of the test run
                    RealSummary.Time = summary.Time; // Update the time from the summary
                    summary = RealSummary;
                }
                if (!(ProxyMessageBus!.IsTestFailed || ProxyMessageBus.IsTestSkipped))
                {
                    if (0 < summary.Failed)
                    {
                        // If the test has failed, we finalize the test run with the failure
                        RealMessageBus!.QueueMessage(
                            new TestFailed(Test, summary.Time, "Test failed while finalizing the test case.", null)
                        );
                    }
                    else
                    {
                        // If the test has passed, we finalize the test run with a success
                        RealMessageBus!.QueueMessage(
                            new TestPassed(Test, summary.Time, ProxyMessageBus.Output + $"** {summary.Failed}")
                        );
                    }
                }
                if(!ProxyMessageBus.IsTestCaseFinished)
                {
                    RealMessageBus!.QueueMessage(
                        new TestCaseFinished(TestCase, summary.Time, summary.Total, summary.Failed, summary.Skipped)
                    );
                }
            });

            return summary;
        }
        */
    }
}
