using System;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.NotificationFlow;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.UnitTests.NotificationFlow
{
    /*
    public class MulticastNotifierTests
    {
        public class DummyAdaptable(string value) : IAdaptTo<string>
        {
            private readonly string _value = value;
            public string Adapt() => _value;
        }

        public class DummyFlow : UnicastNotificationFlow<string>
        {

        }

        [Fact]
        public void RegisterSendingFlow_ShouldNotifyReceivers()
        {
            var notifier = new MulticastNotifier<string, DummyAdaptable>();

            var receiver1 = new Mock<IUnicastNotifier<string>>();
            var receiver2 = new Mock<IUnicastNotifier<string>>();
            notifier.AttachReceivingFlow(receiver1.Object, receiver2.Object);

            var handler = new Mock<INotificationHandler<DummyAdaptable>>();
            Action<DummyAdaptable>? captured = null;
            handler.SetupSet(h => notifier.AttachReceivingFlow(new DummyFlow()) = It.IsAny<Action<DummyAdaptable>>())
                   .Callback<Action<DummyAdaptable>>(a => captured = a);

            notifier.RegisterSendingFlow(handler.Object);

            captured.Should().NotBeNull();
            captured!(new DummyAdaptable("hello"));

            receiver1.Verify(r => r.Notify("hello"), Times.Once);
            receiver2.Verify(r => r.Notify("hello"), Times.Once);
        }

        [Fact]
        public void RegisterSendingFlow_ShouldHandleExceptions()
        {
            var notifier = new TestableMulticastNotifier();
            var receiver = new Mock<IUnicastNotifier<string>>();
            receiver.Setup(r => r.Notify(It.IsAny<string>())).Throws(new InvalidOperationException());
            notifier.AttachReceivingFlow(receiver.Object);

            var handler = new Mock<INotificationHandler<DummyAdaptable>>();
            Action<DummyAdaptable>? captured = null;
            handler.SetupSet(h => h.RegisterHandler = It.IsAny<Action<DummyAdaptable>>())
                   .Callback<Action<DummyAdaptable>>(a => captured = a);

            notifier.RegisterSendingFlow(handler.Object);

            captured!(new DummyAdaptable("oops"));
            notifier.ExceptionHandled.Should().BeTrue();
        }

        private class TestableMulticastNotifier : MulticastNotifier<string, DummyAdaptable>
        {
            public bool ExceptionHandled { get; private set; } = false;

            protected override void OnExceptionInNotification(IUnicastNotifier<string> receiver, string output, Exception ex)
            {
                ExceptionHandled = true;
            }
        }

        [Fact]
        public void AttachReceivingFlow_ShouldThrowIfNull()
        {
            var notifier = new MulticastNotifier<string, DummyAdaptable>();
            Action act = () => notifier.AttachReceivingFlow(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AttachReceivingFlow_ShouldThrowIfReceiverIsNull()
        {
            var notifier = new MulticastNotifier<string, DummyAdaptable>();
            Action act = () => notifier.AttachReceivingFlow(new IUnicastNotifier<string>?[] { null! });
            act.Should().Throw<ArgumentNullException>().WithMessage("*null receiver*");
        }

        [Fact]
        public void DetachReceivingFlow_RemovesReceivers()
        {
            var notifier = new MulticastNotifier<string, DummyAdaptable>();
            var receiver = new Mock<IUnicastNotifier<string>>();
            notifier.AttachReceivingFlow(receiver.Object);
            notifier.DetachReceivingFlow(receiver.Object);

            var snapshot = notifier.GetLatestReceiversSnapshot();
            snapshot.Should().BeEmpty();
        }

        [Fact]
        public void GetLatestReceiversSnapshot_ShouldFilterSelfRemovable()
        {
            var notifier = new MulticastNotifier<string, DummyAdaptable>();
            var removable = new Mock<IUnicastNotifier<string>>();
            removable.As<ISelfRemovable>().Setup(r => r.CanRemove()).Returns(true);

            notifier.AttachReceivingFlow(removable.Object);
            var snapshot = notifier.GetLatestReceiversSnapshot();

            snapshot.Should().BeEmpty();
        }
    }
    */
}
