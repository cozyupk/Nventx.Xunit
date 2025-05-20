using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.NotificationFlow;
using Moq;
using Xunit;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.UnitTests.NotificationFlow.UnicastAdaptationNotifierTests
{
    /// <summary>
    /// Dummy argument class implementing IAdaptTo for testing purposes.
    /// </summary>
    public class DummyArgs : IAdaptTo<DummyArgs>
    {
        /// <summary>
        /// Gets or sets the value for testing.
        /// </summary>
        public virtual string Value { get; set; } = "";

        /// <summary>
        /// Returns a shallow clone of the current DummyArgs instance.
        /// </summary>
        /// <returns>A cloned DummyArgs object.</returns>
        public virtual DummyArgs Adapt()
        {
            return (DummyArgs)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Unit tests for UnicastAdaptationNotifier using DummyArgs.
    /// </summary>
    public class ClonedUnicastAdaptationNotifierTests
    {
        /// <summary>
        /// Verifies that the Handle property can be set only once without exception.
        /// </summary>
        [Fact]
        public void OnObjectCreated_CanBeSetOnce()
        {
            // Arrange & Act
            var notifier = new UnicastAdaptationNotifier<DummyArgs, DummyArgs>
            {
                Handle = _ => { }
            };

            // Assert
            // No exception should occur
        }

        /// <summary>
        /// Verifies that setting the Handle property more than once throws an InvalidOperationException.
        /// </summary>
        [Fact]
        public void Handle_ThrowsIfSetTwice()
        {
            // Arrange
            INotificationFlow<DummyArgs, DummyArgs> notifier = new UnicastAdaptationNotifier<DummyArgs, DummyArgs>
            {
                Handle = _ => { }
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                notifier.Handle = _ => { });
        }

        /// <summary>
        /// Verifies that setting the Handle property to null throws an ArgumentNullException.
        /// </summary>
        [Fact]
        public void Handle_ThrowsIfSetToNull()
        {
            // Arrange
            INotificationFlow<DummyArgs, DummyArgs> notifier = new UnicastAdaptationNotifier<DummyArgs, DummyArgs>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                notifier.Handle = null!);
        }

        /// <summary>
        /// Verifies that Notify throws an ArgumentNullException if the argument is null.
        /// </summary>
        [Fact]
        public void Notify_ThrowsIfArgsIsNull()
        {
            // Arrange
            INotificationFlow<DummyArgs, DummyArgs> notifier = new UnicastAdaptationNotifier<DummyArgs, DummyArgs>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                notifier.Notify(null!));
        }

        /// <summary>
        /// Verifies that Notify invokes the handler with a cloned object.
        /// </summary>
        [Fact]
        public void Notify_InvokesHandlerWithClonedObject()
        {
            // Arrange
            INotificationFlow<DummyArgs, DummyArgs> notifier = new UnicastAdaptationNotifier<DummyArgs, DummyArgs>();
            DummyArgs? received = null;

            // Set the handler to capture the created object
            notifier.Handle = obj => received = obj;

            // Setup a mock for DummyArgs and its clone
            var mock = new Mock<DummyArgs>();
            var clone = new DummyArgs { Value = "test" };

            mock.As<IAdaptTo<DummyArgs>>()
                .Setup(x => x.Adapt())
                .Returns(clone);

            // Act
            notifier.Notify(mock.Object);

            // Assert
            Assert.NotNull(received);
            Assert.Equal("test", received!.Value);
            Assert.NotSame(mock.Object, received);
        }
    }
}