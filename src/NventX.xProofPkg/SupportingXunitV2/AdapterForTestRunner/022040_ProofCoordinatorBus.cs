using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.xProof.SupportingXunit.AdapterForTestRunner
{
    /// <summary>
    /// A silent message bus that ignores all messages sent to it.
    /// </summary>
    internal class ProofCoordinatorBus : IMessageBus
    {
        CancellationTokenSource Cts { get; }
        public IMessageBus OriginalBus { get; }
        public bool IsTestFailed { get; set; } = false;
        public bool IsTestSkipped { get; private set; } = false;
        public bool IsTestFinished { get; private set; } = false;
        public bool IsTestCaseFinished { get; private set; } = false;
        public string Output { get; private set; } = string.Empty;
        public ProofCoordinatorBus(IMessageBus originalBus, CancellationTokenSource cts)
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

        public Task<RunSummary> FinalizeCoordinationAsync(RunSummary summary)
        {
            return Task.Run(() =>
            {
                // TODO: Implement any necessary cleanup logic here
                return summary;
            });
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
            if (message is TestFinished && (IsTestFailed || IsTestSkipped))
            {
                // If the message is a TestFinished and we have failed,
                // we mark it as finished and proxy the message to the original bus
                IsTestFinished = true;
                return OriginalBus.QueueMessage(message);
            }
            if (message is TestFailed tf)
            {
                // If the message is a TestFailed, we mark it as failed
                Console.WriteLine($"Test Failed: {tf.TestMethod}");
                IsTestFailed = true;
                Output = tf.Output ?? string.Empty;
                return OriginalBus.QueueMessage(message);
            }
            if (message is TestSkipped)
            {
                // If the message is a TestSkipped, we mark it as skipped
                IsTestSkipped = true;
                return OriginalBus.QueueMessage(message);
            }
            // otherwise, just proxy the message to the original bus
            return OriginalBus.QueueMessage(message);
        }
    }
}
