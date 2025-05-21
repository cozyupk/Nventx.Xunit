using System;
using System.Collections.Generic;
using System.Threading;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.PayloadFlow;
using Moq;
using Xunit;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.UnitTests.PayloadConsumerUsingWeakReferenceBaseTests
{
    /// <summary>
    /// Unit tests for <see cref="PayloadConsumerUsingWeakReferenceBase{TSenderMeta, TPayloadMeta, TPayloadBody}"/>.
    /// Verifies correct forwarding, weak reference behavior, and argument validation.
    /// </summary>
    public class PayloadConsumerUsingWeakReferenceBaseTests
    {
        /// <summary>
        /// Dummy implementation of ISenderPayload for testing purposes.
        /// </summary>
        /// <remarks>
        /// Initializes a new instance of DummySenderPayload.
        /// </remarks>
        private class DummySenderPayload(string sender, string meta, IEnumerable<string> bodies) : ISenderPayload<string, string, string>
        {
            public string SenderMeta { get; } = sender;
            public IPayload<string, string> Payload { get; } = new DummyPayload(meta, bodies);

            /// <summary>
            /// Dummy implementation of IPayload for testing.
            /// </summary>
            /// <remarks>
            /// Initializes a new instance of DummyPayload.
            /// </remarks>
            private class DummyPayload(string meta, IEnumerable<string> bodies) : IPayload<string, string>
            {
                public string Meta { get; } = meta;
                public IEnumerable<string> Bodies { get; } = bodies;
            }
        }

        /// <summary>
        /// Verifies that notifications are forwarded to the target when it is alive.
        /// </summary>
        [Fact]
        public void PayloadArrivalNotifier_ForwardsNotification_WhenTargetIsAlive()
        {
            // Arrange: Set up a mock consumer and notifier
            var mockTarget = new Mock<ISenderPayloadConsumer<string, string, string>>();
            var mockNotifier = new Mock<IUnicastNotifier<ISenderPayload<string, string, string>>>();

            mockTarget.Setup(c => c.SenderPayloadArrivalNotifier).Returns(mockNotifier.Object);

            var wrapper = new PayloadConsumerUsingWeakReferenceBase<string, string, string>(mockTarget.Object);
            var payload = new DummySenderPayload("sender", "meta", ["b1"]);

            // Act: Notify through the wrapper
            wrapper.SenderPayloadArrivalNotifier.Notify(payload);

            // Assert: Notification is forwarded exactly once
            mockNotifier.Verify(n => n.Notify(payload), Times.Once);
        }

        /// <summary>
        /// Verifies that CanRemove returns true when the target has been garbage collected.
        /// </summary>
        [Fact]
        public void CanRemove_ReturnsTrue_WhenTargetIsCollected()
        {
            // Arrange: Create a consumer in a temporary scope and wrap it
            static ISenderPayloadConsumer<string, string, string> CreateConsumer()
            {
                var mock = new Mock<ISenderPayloadConsumer<string, string, string>>();
                mock.Setup(c => c.SenderPayloadArrivalNotifier).Returns(new NoOpNotifier());
                return mock.Object;
            }

            var wrapper = CreateWeakWrapperFromTemporaryScope(CreateConsumer, out var weakRef);

            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Thread.Sleep(50);

            // Act: Check if the wrapper is removable
            var result = wrapper.CanRemove();

            // Assert: The wrapper should be removable and the weak reference should be dead
            Assert.True(result); // Should be true if GC has collected the target
            Assert.False(weakRef.TryGetTarget(out _)); // Double-check: reference is gone
        }

        /// <summary>
        /// Creates a weak wrapper from a temporary scope and returns the weak reference.
        /// </summary>
        private static PayloadConsumerUsingWeakReferenceBase<string, string, string> CreateWeakWrapperFromTemporaryScope(
            Func<ISenderPayloadConsumer<string, string, string>> factory,
            out WeakReference<ISenderPayloadConsumer<string, string, string>> weakRef)
        {
            ISenderPayloadConsumer<string, string, string> temp = factory();
            weakRef = new WeakReference<ISenderPayloadConsumer<string, string, string>>(temp);

            // Wrap and return
            return new PayloadConsumerUsingWeakReferenceBase<string, string, string>(temp);
        }

        /// <summary>
        /// No-op notifier implementation for testing.
        /// </summary>
        private class NoOpNotifier : IUnicastNotifier<ISenderPayload<string, string, string>>
        {
            public void Notify(ISenderPayload<string, string, string> arg) { /* noop */ }
        }

        /// <summary>
        /// Verifies that the constructor throws ArgumentNullException when passed a null target.
        /// </summary>
        [Fact]
        public void Constructor_NullTarget_ThrowsArgumentNullException()
        {
            // Act & Assert: Constructor should throw for null argument
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new PayloadConsumerUsingWeakReferenceBase<string, string, string>(null!));

            Assert.Equal("target", ex.ParamName);
        }
    }
}