using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.CreationFlow;
using Moq;
using Xunit;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.UnitTests.CreationFlow.IsolatedByNotificationFactoryTests
{
    /// <summary>
    /// DummyArgs is a simple implementation of IShallowClonable for testing purposes.
    /// </summary>
    public class DummyArgs : ISelfNewable<DummyArgs>
    {
        /// <summary>
        /// Gets or sets a string value for testing.
        /// </summary>
        public virtual string Value { get; set; } = "";

        /// <summary>
        /// Returns a shallow clone of the current DummyArgs instance.
        /// </summary>
        public virtual DummyArgs NewFromSelf()
        {
            return (DummyArgs)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Unit tests for ClonedNotifierFactory with DummyArgs.
    /// </summary>
    public class ClonedNotifierFactoryTests
    {
        /// <summary>
        /// Verifies that OnObjectCreated can be set only once without exception.
        /// </summary>
        [Fact]
        public void OnObjectCreated_CanBeSetOnce()
        {
            // Arrange
            var factory = new IsolatedByNotificationFactory<DummyArgs, DummyArgs>
            {
                // Act
                OnObjectCreated = _ => { }
            };

            // Assert
            // No exception should occur
        }

        /// <summary>
        /// Verifies that setting OnObjectCreated more than once throws InvalidOperationException.
        /// </summary>
        [Fact]
        public void OnObjectCreated_ThrowsIfSetTwice()
        {
            // Arrange
            var factory = new IsolatedByNotificationFactory<DummyArgs, DummyArgs>
            {
                OnObjectCreated = _ => { }
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                factory.OnObjectCreated = _ => { });
        }

        /// <summary>
        /// Verifies that setting OnObjectCreated to null throws ArgumentNullException.
        /// </summary>
        [Fact]
        public void OnObjectCreated_ThrowsIfSetToNull()
        {
            // Arrange
            var factory = new IsolatedByNotificationFactory<DummyArgs, DummyArgs>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                factory.OnObjectCreated = null!);
        }

        /// <summary>
        /// Verifies that passing null to Create throws ArgumentNullException.
        /// </summary>
        [Fact]
        public void Create_ThrowsIfArgsIsNull()
        {
            // Arrange
            var factory = new IsolatedByNotificationFactory<DummyArgs, DummyArgs>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                factory.CreateAndNotify(null!));
        }

        /// <summary>
        /// Verifies that Create invokes the handler with a cloned object.
        /// </summary>
        [Fact]
        public void Create_InvokesHandlerWithClonedObject()
        {
            // Arrange
            var factory = new IsolatedByNotificationFactory<DummyArgs, DummyArgs>();
            DummyArgs? received = null;

            // Set the handler to capture the created object
            factory.OnObjectCreated = obj => received = obj;

            // Setup a mock for DummyArgs and its clone
            var mock = new Mock<DummyArgs>();
            var clone = new DummyArgs { Value = "test" };

            mock.As<ISelfNewable<DummyArgs>>()
                .Setup(x => x.NewFromSelf())
                .Returns(clone);

            // Act
            factory.CreateAndNotify(mock.Object);

            // Assert
            Assert.NotNull(received);
            Assert.Equal("test", received!.Value);
            Assert.NotSame(mock.Object, received);
        }

        /// <summary>
        /// Verifies that Create throws an ArgumentNullException if the handler is not set.
        /// </summary>
        [Fact]
        public void Create_ThrowsIfHandlerNotSet()
        {
            // Arrange
            var factory = new IsolatedByNotificationFactory<DummyArgs, DummyArgs>();
            var args = new DummyArgs { Value = "test" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => factory.CreateAndNotify(args));
        }
    }
}