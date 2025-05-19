using System;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow;
using Moq;
using Xunit;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.UnitTests.PayloadFlow.PayloadMulticastNotifierTests
{
    /// <summary>
    /// Unit tests for the PayloadMulticastNotifier class, verifying notification and conditional consumer logic.
    /// </summary>
    public class PayloadMulticastNotifierTests
    {
        /// <summary>
        /// Dummy implementation of IPayload for testing purposes.
        /// </summary>
        public class DummyPayload(string meta, IEnumerable<string> bodies) : IPayload<string, string>
        {
            /// <summary>
            /// Gets the payload metadata.
            /// </summary>
            public string Meta { get; } = meta;

            /// <summary>
            /// Gets the payload bodies.
            /// </summary>
            public IEnumerable<string> Bodies { get; } = bodies;
        }

        /// <summary>
        /// Dummy consumer that records the last received payload and sender.
        /// </summary>
        public class DummyConsumer : IPayloadConsumer<string, string, string>
        {
            /// <summary>
            /// Gets the last received payload.
            /// </summary>
            public IPayload<string, string>? Received { get; private set; }

            /// <summary>
            /// Gets the sender metadata of the last received payload.
            /// </summary>
            public string? Sender { get; private set; }

            /// <summary>
            /// Gets the notifier for payload arrival.
            /// </summary>
            public INotifyAdapted<ISenderPayload<string, string, string>> PayloadArrivalNotifier { get; }

            /// <summary>
            /// Initializes a new instance of the DummyConsumer class.
            /// </summary>
            public DummyConsumer()
            {
                // Lambda-based notifier that records the received payload and sender.
                PayloadArrivalNotifier = new LambdaNotifier<ISenderPayload<string, string, string>>(payload =>
                {
                    Received = payload.Payload;
                    Sender = payload.SenderMeta;
                });
            }

            /// <summary>
            /// Lambda-based notifier for testing.
            /// </summary>
            /// <typeparam name="T">Type of the notification argument.</typeparam>
            private class LambdaNotifier<T>(Action<T> notify) : INotifyAdapted<T>
            {
                private readonly Action<T> _notify = notify;

                /// <summary>
                /// Invokes the notification action.
                /// </summary>
                /// <param name="arg">Notification argument.</param>
                public void Notify(T arg) => _notify(arg);
            }
        }

        /// <summary>
        /// Consumer that always rejects notifications via IsNotifyNeeded.
        /// </summary>
        public class RejectingConditionalConsumer : IPayloadConsumer<string, string, string>, IConditionalNotified<string, string>
        {
            /// <summary>
            /// Indicates whether the consumer was called.
            /// </summary>
            public bool WasCalled { get; private set; }

            /// <summary>
            /// Gets the notifier for payload arrival.
            /// </summary>
            public INotifyAdapted<ISenderPayload<string, string, string>> PayloadArrivalNotifier { get; }

            /// <summary>
            /// Initializes a new instance of the RejectingConditionalConsumer class.
            /// </summary>
            public RejectingConditionalConsumer()
            {
                // Lambda-based notifier that sets WasCalled to true if invoked.
                PayloadArrivalNotifier = new LambdaNotifier<ISenderPayload<string, string, string>>(payload =>
                {
                    WasCalled = true;
                });
            }

            /// <summary>
            /// Always returns false to reject notification.
            /// </summary>
            public bool IsNotifyNeeded(string subjectMeta, string payloadMeta)
            {
                return false; // Always reject
            }

            /// <summary>
            /// Lambda-based notifier for testing.
            /// </summary>
            /// <typeparam name="T">Type of the notification argument.</typeparam>
            private class LambdaNotifier<T>(Action<T> notify) : INotifyAdapted<T>
            {
                private readonly Action<T> _notify = notify;
                public void Notify(T arg) => _notify(arg);
            }
        }

        /// <summary>
        /// Dummy trigger that simulates notification events.
        /// </summary>
        public class DummyTrigger : INotificationHandler<IPayload<string, string>>
        {
            /// <summary>
            /// Event handler for object creation.
            /// </summary>
            public Action<IPayload<string, string>>? Handle { get; set; }

            /// <summary>
            /// Simulates the creation of a payload object.
            /// </summary>
            /// <param name="payload">The payload to notify.</param>
            public void Simulate(IPayload<string, string> payload)
            {
                Handle?.Invoke(payload);
            }
        }

        /// <summary>
        /// Verifies that registering a handler and adding a consumer results in notification.
        /// </summary>
        [Fact]
        public void RegisterHandler_And_AddConsumer_InvokesNotification()
        {
            // Arrange
            var notifier = new PayloadMulticastNotifier<string, string, string>("sender123");
            var consumer = new DummyConsumer();
            notifier.AddConsumer(consumer);

            var trigger = new DummyTrigger();
            notifier.RegisterHandler(trigger);

            var payload = new DummyPayload("meta", ["log1"]);

            // Act
            trigger.Simulate(payload);

            // Assert
            Assert.Equal("sender123", consumer.Sender);
            Assert.Equal("meta", consumer.Received?.Meta);
            Assert.Contains("log1", consumer.Received!.Bodies);
        }

        /// <summary>
        /// Verifies that a rejecting conditional consumer does not receive notifications.
        /// </summary>
        [Fact]
        public void RegisterHandler_WithRejectingConditionalConsumer_DoesNotInvokeNotification()
        {
            // Arrange
            var notifier = new PayloadMulticastNotifier<string, string, string>("senderXYZ");
            var rejectingConsumer = new RejectingConditionalConsumer();
            notifier.AddConsumer(rejectingConsumer);

            var trigger = new DummyTrigger();
            notifier.RegisterHandler(trigger);

            var payload = new DummyPayload("meta", ["logX"]);

            // Act
            trigger.Simulate(payload);

            // Assert
            Assert.False(rejectingConsumer.WasCalled); // validate that the consumer was not notified
        }

        /// <summary>
        /// Verifies that a consumer implementing ISelfRemovable with CanRemove returning true is removed before notification.
        /// </summary>
        [Fact]
        public void RegisterHandler_WithSelfRemovableConsumer_RemovesBeforeNotification()
        {
            // Arrange
            var notifier = new PayloadMulticastNotifier<string, string, string>("senderXYZ");

            var mockConsumer = new Mock<IPayloadConsumer<string, string, string>>();
            var mockNotifier = new Mock<INotifyAdapted<ISenderPayload<string, string, string>>>();
            var selfRemovable = mockConsumer.As<ISelfRemovable>();

            selfRemovable.Setup(r => r.CanRemove()).Returns(true); // ✅ remove対象

            mockConsumer.Setup(c => c.PayloadArrivalNotifier).Returns(mockNotifier.Object);

            notifier.AddConsumer(mockConsumer.Object);

            var trigger = new DummyTrigger();
            notifier.RegisterHandler(trigger);

            var payload = new DummyPayload("meta", ["data"]);

            // Act
            trigger.Simulate(payload);

            // Assert
            mockNotifier.Verify(n => n.Notify(It.IsAny<ISenderPayload<string, string, string>>()), Times.Never);
            selfRemovable.Verify(r => r.CanRemove(), Times.Once);
        }

        /// <summary>
        /// Verifies that RegisterHandler throws an ArgumentNullException when passed a null handler.
        /// </summary>
        [Fact]
        public void RegisterHandler_NullHandler_ThrowsArgumentNullException()
        {
            // Arrange
            var notifier = new PayloadMulticastNotifier<string, string, string>("sender");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                notifier.RegisterHandler(null!)); // force null argument
        }
    }
}
