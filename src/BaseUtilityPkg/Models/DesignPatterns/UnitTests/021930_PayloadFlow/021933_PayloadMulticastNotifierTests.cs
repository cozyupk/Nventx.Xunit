using System;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow;
using Xunit;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.UnitTests.PayloadFlow.PayloadMulticastNotifierTests
{
    /// <summary>
    /// Unit tests for PayloadMulticastNotifier, verifying multicast notification logic and consumer behaviors.
    /// </summary>
    public class PayloadMulticastNotifierTests
    {
        /// <summary>
        /// Dummy payload implementation for testing, with meta and bodies.
        /// </summary>
        public class DummyPayload(string meta, IEnumerable<string> bodies) : IPayload<string, string>, IAdaptTo<IPayload<string, string>>
        {
            public string Meta { get; } = meta;
            public IEnumerable<string> Bodies { get; } = bodies;
            public IPayload<string, string> Adapt() => this;
        }

        /// <summary>
        /// Dummy consumer that records notification count and last received payload.
        /// </summary>
        public class DummyConsumer : IPayloadConsumer<string, string, string>
        {
            public int NotifyCount { get; private set; } = 0;
            public IPayload<string, string>? ReceivedLast { get; private set; }
            public string? SenderLast { get; private set; }

            public INotifyAdapted<ISenderPayload<string, string, string>> PayloadArrivalNotifier { get; }

            public DummyConsumer()
            {
                // Notifier increments count and records payload and sender.
                PayloadArrivalNotifier = new LambdaNotifier<ISenderPayload<string, string, string>>(payload =>
                {
                    ReceivedLast = payload.Payload;
                    SenderLast = payload.SenderMeta;
                    ++NotifyCount;
                });
            }

            /// <summary>
            /// Simple lambda-based notifier for test purposes.
            /// </summary>
            private class LambdaNotifier<T>(Action<T> notify) : INotifyAdapted<T>
            {
                private readonly Action<T> _notify = notify;
                public void Notify(T arg) => _notify(arg);
            }
        }

        /// <summary>
        /// Consumer that always rejects notification via IConditionalNotified.
        /// </summary>
        public class RejectingConditionalConsumer : IPayloadConsumer<string, string, string>, IConditionalNotified<string, string>
        {
            public bool WasCalled { get; private set; }

            public INotifyAdapted<ISenderPayload<string, string, string>> PayloadArrivalNotifier { get; }

            public RejectingConditionalConsumer()
            {
                // Notifier sets WasCalled flag if invoked.
                PayloadArrivalNotifier = new LambdaNotifier<ISenderPayload<string, string, string>>(payload =>
                {
                    WasCalled = true;
                });
            }

            /// <summary>
            /// Always returns false, so notification is never needed.
            /// </summary>
            public bool IsNotifyNeeded(string subjectMeta, string payloadMeta) => false;

            private class LambdaNotifier<T>(Action<T> notify) : INotifyAdapted<T>
            {
                private readonly Action<T> _notify = notify;
                public void Notify(T arg) => _notify(arg);
            }
        }

        /// <summary>
        /// Consumer that can remove itself from the notifier based on a flag.
        /// </summary>
        public class DummySelfRemovingConsumer
            : IPayloadConsumer<string, string, string>, ISelfRemovable
        {
            public int NotifyCount { get; private set; }

            public INotifyAdapted<ISenderPayload<string, string, string>> PayloadArrivalNotifier { get; }

            public bool CanRemoveFlag { get; }
            public bool CanRemoveValue { get; set; }

            /// <summary>
            /// Returns the current removable state.
            /// </summary>
            public bool CanRemove() => CanRemoveValue;

            public DummySelfRemovingConsumer(bool removableFlag)
            {
                CanRemoveFlag = removableFlag;
                // Notifier increments count and sets CanRemoveValue.
                PayloadArrivalNotifier = new LambdaNotifier<ISenderPayload<string, string, string>>(_ =>
                {
                    NotifyCount++;
                    CanRemoveValue = CanRemoveFlag;
                }
                );
            }

            public DummySelfRemovingConsumer() : this(true)
            {
            }

            private class LambdaNotifier<T>(Action<T> notify) : INotifyAdapted<T>
            {
                private readonly Action<T> _notify = notify;
                public void Notify(T arg) => _notify(arg);
            }
        }

        /// <summary>
        /// Verifies that a consumer receives the payload and sender meta as expected.
        /// </summary>
        [Fact]
        public void Notify_ConsumerReceivesPayload()
        {
            var notifier = new PayloadMulticastNotifier<string, string, string>("sender123");
            var consumer = new DummyConsumer();
            notifier.AddConsumer(consumer);

            var unicastNotifier = new UnicastAdaptationNotifier<IPayload<string, string>, DummyPayload>();
            notifier.RegisterHandler(unicastNotifier);

            var payload = new DummyPayload("meta", ["log1"]);
            unicastNotifier.Notify(payload);

            Assert.Equal("sender123", consumer.SenderLast);
            Assert.Equal("meta", consumer.ReceivedLast?.Meta);
            Assert.Contains("log1", consumer.ReceivedLast!.Bodies);
        }

        /// <summary>
        /// Verifies that a conditional consumer that rejects notification is not called.
        /// </summary>
        [Fact]
        public void Notify_WithRejectingConditionalConsumer_DoesNotInvokeNotification()
        {
            var notifier = new PayloadMulticastNotifier<string, string, string>("senderXYZ");
            var consumer = new RejectingConditionalConsumer();
            notifier.AddConsumer(consumer);

            var unicastNotifier = new UnicastAdaptationNotifier<IPayload<string, string>, DummyPayload>();
            notifier.RegisterHandler(unicastNotifier);

            var payload = new DummyPayload("meta", ["logX"]);
            unicastNotifier.Notify(payload);

            Assert.False(consumer.WasCalled);
        }

        /// <summary>
        /// Verifies that after removing a consumer, only the remaining consumers are notified.
        /// </summary>
        [Fact]
        public void Notify_AfterRemovingConsumer_OnlyNotifiesRemaining()
        {
            var notifier = new PayloadMulticastNotifier<string, string, string>("senderXYZ");
            var stayConsumer = new DummyConsumer();
            var removedConsumer = new DummyConsumer();

            notifier.AddConsumer(stayConsumer);
            notifier.AddConsumer(removedConsumer);

            var unicastNotifier = new UnicastAdaptationNotifier<IPayload<string, string>, DummyPayload>();
            notifier.RegisterHandler(unicastNotifier);

            var payload = new DummyPayload("meta", ["data"]);
            unicastNotifier.Notify(payload);
            notifier.RemoveConsumer(removedConsumer);
            unicastNotifier.Notify(payload);

            Assert.Equal(2, stayConsumer.NotifyCount);
            Assert.Equal(1, removedConsumer.NotifyCount);
        }

        /// <summary>
        /// Verifies that a self-removable consumer is removed before the next notification.
        /// </summary>
        [Fact]
        public void Notify_WithSelfRemovableConsumer_RemovesBeforeNotification()
        {
            // Arrange
            var notifier = new PayloadMulticastNotifier<string, string, string>("senderXYZ");

            var stayConsumer = new DummyConsumer();
            var nonRemoveConsumer = new DummySelfRemovingConsumer(false);
            var removeConsumer = new DummySelfRemovingConsumer(true);

            notifier.AddConsumer(stayConsumer);
            notifier.AddConsumer(nonRemoveConsumer);
            notifier.AddConsumer(removeConsumer);

            var unicastNotifier = new UnicastAdaptationNotifier<IPayload<string, string>, DummyPayload>();
            notifier.RegisterHandler(unicastNotifier);

            var payload = new DummyPayload("meta", ["data"]);

            // Act
            unicastNotifier.Notify(payload);  // First: removeConsumer will be removed
            unicastNotifier.Notify(payload);  // Second: removeConsumer will not be notified

            // Assert
            Assert.Equal(2, stayConsumer.NotifyCount);       // Both notifications received
            Assert.Equal(2, nonRemoveConsumer.NotifyCount);  // Both notifications received
            Assert.Equal(1, removeConsumer.NotifyCount);     // Only first notification received
        }

        /// <summary>
        /// Verifies that RegisterHandler throws ArgumentNullException when a null handler is provided.
        /// </summary>
        [Fact]
        public void RegisterHandler_ThrowsArgumentNullException_WhenHandlerIsNull()
        {
            // Arrange: Create a PayloadMulticastNotifier instance with a sample sender.
            var notifier = new PayloadMulticastNotifier<string, string, string>("sender");

            // Act & Assert: Registering a null handler should throw ArgumentNullException.
            var ex = Assert.Throws<ArgumentNullException>(() =>
                notifier.RegisterHandler(null!)); // Explicitly pass null to test exception

            // Assert: The exception's parameter name should be "notificationHandler" for clarity.
            Assert.Equal("notificationHandler", ex.ParamName);
        }
    }
}