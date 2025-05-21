using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.PayloadFlow;
using Moq;
using Xunit;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.UnitTests.PayloadFlow.PayloadMulticastNotifierTests
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
        public class DummyConsumer : ISenderPayloadConsumer<string, string, string>
        {
            public int NotifyCount { get; private set; } = 0;
            public IPayload<string, string>? ReceivedLast { get; private set; }
            public string? SenderLast { get; private set; }

            public IUnicastNotifier<ISenderPayload<string, string, string>> SenderPayloadArrivalNotifier { get; }

            public DummyConsumer()
            {
                // Notifier increments count and records payload and sender.
                SenderPayloadArrivalNotifier = new LambdaNotifier<ISenderPayload<string, string, string>>(payload =>
                {
                    // Record the last received payload for verification in tests.
                    ReceivedLast = payload.Payload;
                    // Record the sender's metadata for verification in tests.
                    SenderLast = payload.SenderMeta;
                    // Increment the notification count to track how many times the consumer was notified.
                    ++NotifyCount;
                });
            }

            /// <summary>
            /// Simple lambda-based notifier for test purposes.
            /// </summary>
            private class LambdaNotifier<T>(Action<T> notify) : IUnicastNotifier<T>
            {
                private readonly Action<T> _notify = notify;
                public void Notify(T arg) => _notify(arg);
            }
        }

        /// <summary>
        /// Consumer that always rejects notification via IConditionalNotified.
        /// </summary>
        public class RejectingConditionalConsumer : ISenderPayloadConsumer<string, string, string>, IShouldNotify<string, string>
        {
            public bool WasCalled { get; private set; }

            public IUnicastNotifier<ISenderPayload<string, string, string>> SenderPayloadArrivalNotifier { get; }

            public RejectingConditionalConsumer()
            {
                // Notifier sets WasCalled flag if invoked.
                SenderPayloadArrivalNotifier = new LambdaNotifier<ISenderPayload<string, string, string>>(payload =>
                {
                    WasCalled = true;
                });
            }

            /// <summary>
            /// Always returns false, so notification is never needed.
            /// </summary>
            public bool ShouldNotify(string subjectMeta, string payloadMeta) => false;

            private class LambdaNotifier<T>(Action<T> notify) : IUnicastNotifier<T>
            {
                private readonly Action<T> _notify = notify;
                public void Notify(T arg) => _notify(arg);
            }
        }

        /// <summary>
        /// Consumer that can remove itself from the notifier based on a flag.
        /// </summary>
        public class DummySelfRemovingConsumer
            : ISenderPayloadConsumer<string, string, string>, ISelfRemovable
        {
            public int NotifyCount { get; private set; }

            public IUnicastNotifier<ISenderPayload<string, string, string>> SenderPayloadArrivalNotifier { get; }

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
                SenderPayloadArrivalNotifier = new LambdaNotifier<ISenderPayload<string, string, string>>(_ =>
                {
                    NotifyCount++;
                    CanRemoveValue = CanRemoveFlag;
                }
                );
            }

            public DummySelfRemovingConsumer() : this(true)
            {
            }

            private class LambdaNotifier<T>(Action<T> notify) : IUnicastNotifier<T>
            {
                private readonly Action<T> _notify = notify;
                public void Notify(T arg) => _notify(arg);
            }
        }

        /// <summary>
        /// Dummy consumer implementation that signals completion via TaskCompletionSource when notified.
        /// Used for testing asynchronous notification scenarios.
        /// </summary>
        public class DummyConsumerWithCompletion : ISenderPayloadConsumer<string, string, string>
        {
            // TaskCompletionSource used to signal when notification occurs.
            private readonly TaskCompletionSource<bool> _tcs;

            /// <summary>
            /// Gets the number of times this consumer has been notified.
            /// </summary>
            public int NotifyCount { get; private set; }

            /// <summary>
            /// Gets the last payload received by this consumer.
            /// </summary>
            public IPayload<string, string>? ReceivedLast { get; private set; }

            /// <summary>
            /// Notifier that triggers when a payload arrives.
            /// </summary>
            public IUnicastNotifier<ISenderPayload<string, string, string>> SenderPayloadArrivalNotifier { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="DummyConsumerWithCompletion"/> class.
            /// </summary>
            /// <param name="tcs">TaskCompletionSource to signal upon notification.</param>
            public DummyConsumerWithCompletion(TaskCompletionSource<bool> tcs)
            {
                _tcs = tcs;

                // Notifier increments count, records payload, and signals completion.
                SenderPayloadArrivalNotifier = new LambdaNotifier<ISenderPayload<string, string, string>>(payload =>
                {
                    NotifyCount++;
                    ReceivedLast = payload.Payload;
                    _tcs.TrySetResult(true);
                });
            }

            /// <summary>
            /// Simple lambda-based notifier for test purposes.
            /// </summary>
            private class LambdaNotifier<T>(Action<T> notify) : IUnicastNotifier<T>
            {
                private readonly Action<T> _notify = notify;

                /// <summary>
                /// Invokes the notification action.
                /// </summary>
                /// <param name="arg">The argument to notify with.</param>
                public void Notify(T arg) => _notify(arg);
            }
        }

        /// <summary>
        /// Verifies that a consumer receives the payload and sender meta as expected.
        /// </summary>
        [Fact]
        public void Notify_ConsumerReceivesPayload()
        {
            // Arrange
            IPayloadMulticastNotifier<string, string, string> notifier = new PayloadMulticastNotifier<string, string, string>("sender123");
            // Arrange: Create and add a dummy consumer to the notifier.
            ISenderPayloadConsumer<string, string, string> consumer = new DummyConsumer();
            notifier.AddConsumer(consumer);

            // Arrange
            IPayloadUnicastNotificationFlow<string, string> unicastNotifier = new PayloadUnicastNotifier<string, string>();
            notifier.RegisterHandler(unicastNotifier);

            // Act
            var payload = new DummyPayload("meta", ["log1"]);
            unicastNotifier.Notify(payload);

            // Assert
            Assert.IsType<DummyConsumer>(consumer);
            var dummy = (DummyConsumer)consumer;
            Assert.Equal("sender123", dummy.SenderLast);
            Assert.Equal("meta", dummy.ReceivedLast?.Meta);
            Assert.Contains("log1", dummy.ReceivedLast?.Bodies!);
        }

        /// <summary>
        /// Verifies that a conditional consumer that rejects notification is not called.
        /// </summary>
        [Fact]
        public void Notify_WithRejectingConditionalConsumer_DoesNotInvokeNotification()
        {
            // Arrange
            IPayloadMulticastNotifier<string, string, string> notifier = new PayloadMulticastNotifier<string, string, string>("senderXYZ");
            ISenderPayloadConsumer<string, string, string> consumer = new RejectingConditionalConsumer();
            IPayloadUnicastNotificationFlow<string, string> unicastNotifier = new PayloadUnicastNotifier<string, string>();
            notifier.AddConsumer(consumer);
            notifier.RegisterHandler(unicastNotifier);

            // Act
            var payload = new DummyPayload("meta", ["logX"]);
            unicastNotifier.Notify(payload);

            // Assert
            Assert.IsType<RejectingConditionalConsumer>(consumer);
            var dummy = (RejectingConditionalConsumer)consumer;
            Assert.False(dummy.WasCalled);
        }

        /// <summary>
        /// Verifies that after removing a consumer, only the remaining consumers are notified.
        /// </summary>
        [Fact]
        public void Notify_AfterRemovingConsumer_OnlyNotifiesRemaining()
        {
            // Arrange: Create a PayloadMulticastNotifier with a specific sender meta.
            IPayloadMulticastNotifier<string, string, string> notifier = new PayloadMulticastNotifier<string, string, string>("senderXYZ");

            // Arrange: Create two dummy consumers, one to stay and one to be removed.
            ISenderPayloadConsumer<string, string, string> stayConsumer = new DummyConsumer();
            ISenderPayloadConsumer<string, string, string> removedConsumer = new DummyConsumer();

            // Add both consumers to the notifier.
            notifier.AddConsumer(stayConsumer);
            notifier.AddConsumer(removedConsumer);

            // Arrange: Create a unicast notifier and register it with the multicast notifier.
            var unicastNotifier = new UnicastNotificationFlow<IPayload<string, string>, DummyPayload>();
            notifier.RegisterHandler(unicastNotifier);

            // Act: Create a dummy payload and notify via the handler.
            var payload = new DummyPayload("meta", ["data"]);
            unicastNotifier.Notify(payload); // Both consumers should be notified.

            // Act: Remove one consumer and notify again.
            notifier.RemoveConsumer(removedConsumer);
            unicastNotifier.Notify(payload); // Only the remaining consumer should be notified.

            // Assert: Verify notification counts for both consumers.
            Assert.IsType<DummyConsumer>(stayConsumer);
            Assert.IsType<DummyConsumer>(removedConsumer);
            Assert.Equal(2, ((DummyConsumer)stayConsumer).NotifyCount);
            Assert.Equal(1, ((DummyConsumer)removedConsumer).NotifyCount);
        }

        /// <summary>
        /// Verifies that a self-removable consumer is removed before the next notification.
        /// </summary>
        [Fact]
        public void Notify_WithSelfRemovableConsumer_RemovesBeforeNotification()
        {
            // Arrange
            IPayloadMulticastNotifier<string, string, string> notifier = new PayloadMulticastNotifier<string, string, string>("senderXYZ");

            ISenderPayloadConsumer<string, string, string> stayConsumer = new DummyConsumer();
            ISenderPayloadConsumer<string, string, string> nonRemoveConsumer = new DummySelfRemovingConsumer(false);
            ISenderPayloadConsumer<string, string, string> removeConsumer = new DummySelfRemovingConsumer(true);

            notifier.AddConsumer(stayConsumer);
            notifier.AddConsumer(nonRemoveConsumer);
            notifier.AddConsumer(removeConsumer);

            var unicastNotifier = new UnicastNotificationFlow<IPayload<string, string>, DummyPayload>();
            notifier.RegisterHandler(unicastNotifier);

            var payload = new DummyPayload("meta", ["data"]);

            // Act
            unicastNotifier.Notify(payload);  // First: removeConsumer will be removed
            unicastNotifier.Notify(payload);  // Second: removeConsumer will not be notified

            // Assert
            Assert.IsType<DummyConsumer>(stayConsumer);
            Assert.IsType<DummySelfRemovingConsumer>(nonRemoveConsumer);
            Assert.IsType<DummySelfRemovingConsumer>(nonRemoveConsumer);
            Assert.Equal(2, ((DummyConsumer)stayConsumer).NotifyCount);       // Both notifications received
            Assert.Equal(2, ((DummySelfRemovingConsumer)nonRemoveConsumer).NotifyCount);  // Both notifications received
            Assert.Equal(1, ((DummySelfRemovingConsumer)removeConsumer).NotifyCount);     // Only first notification received
        }

        /// <summary>
        /// Verifies that RegisterHandler throws ArgumentNullException when a null handler is provided.
        /// </summary>
        [Fact]
        public void RegisterHandler_ThrowsArgumentNullException_WhenHandlerIsNull()
        {
            // Arrange
            IPayloadMulticastNotifier<string, string, string> notifier = new PayloadMulticastNotifier<string, string, string>("sender");

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                notifier.RegisterHandler(null!)); // Explicitly pass null to test exception

            // Assert
            Assert.Equal("notificationHandler", ex.ParamName);
        }

        /// <summary>
        /// Verifies that RegisterHandler, when used with asynchronous execution (e.g., Task.Run), notifies all consumers in the background and signals completion using TaskCompletionSource.
        /// </summary>
        [Fact]
        public async Task RegisterHandler_AsynchronousExecution_NotifiesAllConsumersInBackground_UsingTaskCompletionSource()
        {
            // Arrange
            IPayloadMulticastNotifier<string, string, string> notifier = new PayloadMulticastNotifier<string, string, string>("senderAsync");
            var tcs = new TaskCompletionSource<bool>();
            ISenderPayloadConsumer<string, string, string> consumer = new DummyConsumerWithCompletion(tcs);
            notifier.AddConsumer(consumer);
            IPayloadUnicastNotificationFlow<string, string> handler = new PayloadUnicastNotifier<string, string>();

            // RegisterHandler with Task.Run to force async dispatch
            notifier.RegisterHandler(handler, action => Task.Run(action));

            var payload = new DummyPayload("meta", ["data"]);

            // Act
            handler.Notify(payload);

            // Assert
            var completed = await Task.WhenAny(tcs.Task, Task.Delay(1000));
            Assert.IsType<DummyConsumerWithCompletion>(consumer);
            var dummy = (DummyConsumerWithCompletion)consumer;
            Assert.Equal(1, dummy.NotifyCount);
            Assert.Equal("meta", dummy.ReceivedLast?.Meta);
            Assert.True(tcs.Task.IsCompletedSuccessfully);
        }


        /// <summary>
        /// Verifies that RegisterHandler, when used with synchronous execution (the default), notifies all consumers immediately.
        /// </summary>
        [Fact]
        public void RegisterHandler_SynchronousExecution_NotifiesAllConsumersImmediately()
        {
            // Arrange
            IPayloadMulticastNotifier<string,string,string> notifier = new PayloadMulticastNotifier<string, string, string>("senderSync");
            ISenderPayloadConsumer<string, string, string> consumer = new DummyConsumer();
            notifier.AddConsumer(consumer);

            // Arrange
            var handler = new UnicastNotificationFlow<IPayload<string, string>, DummyPayload>();
            notifier.RegisterHandler(handler); // default is sync

            // Act
            var payload = new DummyPayload("meta", ["data"]);
            handler.Notify(payload);

            // Assert
            Assert.IsType<DummyConsumer>(consumer);
            var dummy = (DummyConsumer)consumer;
            Assert.Equal(1, dummy.NotifyCount);
            Assert.Equal("meta", dummy.ReceivedLast?.Meta);
        }

        /// <summary>
        /// Verifies that GetConsumers returns all registered consumers.
        /// </summary>
        [Fact]
        public void GetConsumers_ReturnsAllRegisteredConsumers()
        {
            // Arrange
            var notifier = new PayloadMulticastNotifier<string, string, string>("sender");
            ISenderPayloadConsumer<string, string, string> consumer1 = new Mock<ISenderPayloadConsumer<string, string, string>>().Object;
            ISenderPayloadConsumer<string, string, string> consumer2 = new Mock<ISenderPayloadConsumer<string, string, string>>().Object;

            notifier.AddConsumer(consumer1);
            notifier.AddConsumer(consumer2);

            // Act
            var consumers = notifier.GetConsumers();

            // Assert
            Assert.Contains(consumer1, consumers);
            Assert.Contains(consumer2, consumers);
        }

        /// <summary>
        /// Verifies that ClearConsumers removes all registered consumers.
        /// </summary>
        [Fact]
        public void ClearConsumers_RemovesAllConsumers()
        {
            // Arrange
            var notifier = new PayloadMulticastNotifier<string, string, string>("sender");
            ISenderPayloadConsumer<string, string, string> consumer = new Mock<ISenderPayloadConsumer<string, string, string>>().Object;
            notifier.AddConsumer(consumer);

            // Act
            notifier.ClearConsumers();
            var consumers = notifier.GetConsumers();

            // Assert
            Assert.Empty(consumers);
        }
    }
}
