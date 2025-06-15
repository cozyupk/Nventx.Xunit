using System;
using System.Threading;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestRunner;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xproof.SupportingXunit.AdapterForTestRunner
{
    /// <summary>
    /// A silent message bus that ignores all messages sent to it.
    /// </summary>
    internal class ProofCoordinatorBus : IMessageBus
    {
        private IInvokableProof Proof { get; }
        private ITestCase TestCase { get; }
        private CancellationTokenSource Cts { get; }
        private IMessageBus OriginalBus { get; }
        private ITest? CapturedTest { get; set; } // Captured during message handling (e.g. TestStarting, TestFailed, etc.)
        private bool IsTestFailed { get; set; } = false;
        private bool IsTestSkipped { get; set; } = false;
        private bool IsTestFinished { get; set; } = false;
        private bool IsTestCaseFinished { get; set; } = false;
        private string Output { get; set; } = string.Empty;
        public ProofCoordinatorBus(object? proofCand, ITestCase testCase, IMessageBus originalBus, CancellationTokenSource cts)
        {
            // validate the proof candidate and store it
            Proof = proofCand as IInvokableProof ?? throw new ArgumentException(
                      "The proof candidate must implement IInvokableProof.", nameof(proofCand)
                    );

            // Validate the test case and store it
            TestCase = testCase ?? throw new ArgumentNullException(nameof(testCase), "Test case cannot be null.");

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

        public Task<RunSummary> FinalizeCoordinationAsync(RunSummary summary)
        {
            return Task.Run(() =>
            {
                // TODO: Implement any necessary cleanup logic here
                if (!IsTestCaseFinished)
                {
                    try
                    {
                        FinalizeTest(summary);
                    } finally
                    {
                        // Finally, we queue a TestCaseFinished message with the summary
                        if(!OriginalBus.QueueMessage(new TestCaseFinished(
                            TestCase, summary.Time, summary.Total, summary.Failed, summary.Skipped
                        )))
                        {
                            Cts.Cancel();
                        } else
                        {
                            IsTestCaseFinished = true;
                        }
                    }
                } 

                return summary;
            });
        }

        void FinalizeTest(RunSummary summary)
        {
            if (!IsTestFinished)
            {
                try
                {
                    summary.Total = 0;
                    var results = Proof.GetResults();
                    foreach (var result in results)
                    {
                        ++summary.Total;
                        if (result.Exception != null)
                        {
                            ++summary.Failed;
                            Output = $"{result.ProbeScopeRecord}";
                            // TODO: Mesure the time of the failure
                            if (!OriginalBus.QueueMessage(new TestFailed(
                                CapturedTest, (decimal)result.Elapsed.TotalSeconds, Output, result.Exception
                            )))
                            {
                                Cts.Cancel();
                                break;
                            }
                            IsTestFailed = true;
                            continue;
                        }
                        Output = $"{result.ProbeScopeRecord}";
                        // If there are no probing failures, we consider the test passed
                        Console.WriteLine($"**** Sending TestPassed for {CapturedTest?.DisplayName}");
                        Console.WriteLine($"**** Summary: {summary.Total}, {summary.Failed}, {summary.Skipped}");
                        if (!OriginalBus.QueueMessage(new TestPassed(
                            CapturedTest, (decimal)result.Elapsed.TotalSeconds, Output
                        )))
                        {
                            Cts.Cancel();
                        }
                    }
                }
                finally
                {
                    // Finally, we queue a TestFinished message with the captured test and summary
                    InternalQueueMessage(new TestFinished(
                        CapturedTest , summary.Time, Output
                    ));
                    IsTestFinished = true;
                }
            }
        }

        private bool InternalQueueMessage(IMessageSinkMessage message)
        {
            // Proxy messages except some conditions
            if (message is TestPassed tp)
            {
                Console.WriteLine($"Test Passed but ignored {tp.TestCase.TestMethod.Method.Name}");
                return true; // Just Ignore the message and return true
            }
            if (message is TestCaseFinished && IsTestFinished)
            {
                // If the message is a TestCaseFinished and we have already finished the test,
                // we mark it as finished and proxy the message to the original bus
                IsTestCaseFinished = true;
                return OriginalBus.QueueMessage(message);
            }
            if (message is TestFinished tFinished)
            {
                CapturedTest  = tFinished.Test;
                if (IsTestFailed || IsTestSkipped)
                {
                    // If the message is a TestFinished and we have failed,
                    // we mark it as finished and proxy the message to the original bus
                    IsTestFinished = true;
                    return OriginalBus.QueueMessage(message);
                }
            }
            if (message is TestFailed tFailed)
            {
                // If the message is a TestFailed, we mark it as failed
                Console.WriteLine($"Test Failed: {tFailed.TestMethod}");
                CapturedTest  = tFailed.Test;
                IsTestFailed = true;
                Output = tFailed.Output ?? string.Empty;
                return OriginalBus.QueueMessage(message);
            }
            if (message is TestSkipped tSkipped)
            {
                // If the message is a TestSkipped, we mark it as skipped
                CapturedTest  = tSkipped.Test;
                IsTestSkipped = true;
                return OriginalBus.QueueMessage(message);
            }
            if (message is TestStarting tStarting)
            {
                CapturedTest  = tStarting.Test;
                return OriginalBus.QueueMessage(message);
            }
            // otherwise, just proxy the message to the original bus
            return OriginalBus.QueueMessage(message);
        }
    }
}
