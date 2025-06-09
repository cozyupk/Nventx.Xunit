using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.xProof.SupportingXunit.SelfHostedTestRuntime
{
    public class SpyBusWithSink : IMessageBus
    {
        private TestMessageSink Sink { get; }

        public SpyBusWithSink(TestMessageSink sink)
        {
            this.Sink = sink ?? throw new System.ArgumentNullException(nameof(sink));
        }

        public bool QueueMessage(IMessageSinkMessage message)
        {
            // TestMessageSink に流す
            return Sink.OnMessage(message);
        }

        public void Dispose() { }
    }
}
