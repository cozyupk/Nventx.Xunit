using System;
using System.Collections.Generic;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.PayloadFlow;
using Xunit;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.UnitTests.PayloadFlow.PayloadMulticastNotifierBuilderTests
{
    /// <summary>
    /// Unit tests for PayloadMulticastNotifierBuilder.
    /// Verifies that consumers are correctly registered and notified.
    /// </summary>
    public class PayloadMulticastNotifierBuilderTests
    {
        /// <summary>
        /// Dummy implementation of IPayloadConsumer for testing notification.
        /// </summary>
        private class DummyConsumer : ISenderPayloadConsumer<string, string, string>
        {
            /// <summary>
            /// Indicates whether the consumer was notified.
            /// </summary>
            public bool WasNotified { get; private set; }

            /// <summary>
            /// Notifier that handles WasNotified when notified.
            /// </summary>
            public IUnicastNotifier<ISenderPayload<string, string, string>> SenderPayloadArrivalNotifier { get; }

            public DummyConsumer()
            {
                SenderPayloadArrivalNotifier = new TestNotifier(() => WasNotified = true);
            }

            /// <summary>
            /// Test notifier that invokes a callback on notification.
            /// </summary>
            private class TestNotifier(Action callback) : IUnicastNotifier<ISenderPayload<string, string, string>>
            {
                private readonly Action _callback = callback;

                public void Notify(ISenderPayload<string, string, string> payload)
                {
                    _callback();
                }
            }
        }

        /// <summary>
        /// Verifies that all consumers added to the PayloadMulticastNotifierBuilder are properly registered and notified when a payload is sent.
        /// </summary>
        [Fact]
        public void Build_ShouldRegisterAllAddedConsumers()
        {
            // Arrange
            IPayloadMulticastNotifierBuilder<string, string, string> builder = new PayloadMulticastNotifierBuilder<string, string, string>();
            ISenderPayloadConsumer<string, string, string> consumer1 = new DummyConsumer();
            ISenderPayloadConsumer<string, string, string> consumer2 = new DummyConsumer();
            builder.AddConsumer(consumer1);
            builder.AddConsumer(consumer2);

            var notifier = builder.Build("sender-xyz");
            var unicastNotifier = new UnicastNotificationFlow<IPayload<string, string>, DummyPayload>();
            notifier.RegisterHandler(unicastNotifier);

            // Act
            var payload = new DummyPayload("meta", ["test-body"]);
            unicastNotifier.Notify(payload);

            // Assert
            Assert.IsType<DummyConsumer>(consumer1);
            Assert.IsType<DummyConsumer>(consumer2);
            Assert.True(((DummyConsumer)consumer1).WasNotified);
            Assert.True(((DummyConsumer)consumer2).WasNotified);
        }

        /// <summary>
        /// Dummy implementation of IPayload for testing.
        /// </summary>
        private class DummyPayload(string meta, IEnumerable<string> bodies) : IPayload<string, string>, IAdaptTo<IPayload<string, string>>
        {
            public string Meta { get; } = meta;
            public IEnumerable<string> Bodies { get; } = bodies;
            public IPayload<string, string> Adapt() => this;
        }

        /// <summary>
        /// Dummy handler implementing INotificationHandler for testing notification flow.
        /// </summary>
        private class DummyHandler : INotificationHandler<IPayload<string, string>>
        {
            /// <summary>
            /// Delegate to handle notifications.
            /// </summary>
            public Action<IPayload<string, string>>? Handle { get; set; }

            /// <summary>
            /// Simulates a notification by invoking the handler.
            /// </summary>
            public void Simulate(IPayload<string, string> payload)
            {
                Handle?.Invoke(payload);
            }
        }
    }
}
