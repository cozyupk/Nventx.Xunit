using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.ShallowClonedNotifierFactory;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.Traits;
using Moq;
using Xunit;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.UnitTests.ShallowClonedNotifierFactory.ClonedNotifierFactoryTests
{
    /// <summary>
    /// DummyArgs is a simple implementation of IShallowClonable for testing purposes.
    /// </summary>
    public class DummyArgs : IShallowClonable<DummyArgs>
    {
        /// <summary>
        /// Gets or sets a string value for testing.
        /// </summary>
        public virtual string Value { get; set; } = "";

        /// <summary>
        /// Returns a shallow clone of the current DummyArgs instance.
        /// </summary>
        public virtual DummyArgs ShallowClone()
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
            var factory = new ClonedNotifierFactory<DummyArgs>
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
            var factory = new ClonedNotifierFactory<DummyArgs>
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
            var factory = new ClonedNotifierFactory<DummyArgs>();

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
            var factory = new ClonedNotifierFactory<DummyArgs>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                factory.TriggerCreation(null!));
        }

        /// <summary>
        /// Verifies that Create invokes the handler with a cloned object.
        /// </summary>
        [Fact]
        public void Create_InvokesHandlerWithClonedObject()
        {
            // Arrange
            var factory = new ClonedNotifierFactory<DummyArgs>();
            ShallowCloned<DummyArgs>? received = null;

            // Set the handler to capture the created object
            factory.OnObjectCreated = obj => received = obj;

            // Setup a mock for DummyArgs and its clone
            var mock = new Mock<DummyArgs>();
            var clone = new DummyArgs { Value = "test" };

            mock.As<IShallowClonable<DummyArgs>>()
                .Setup(x => x.ShallowClone())
                .Returns(clone);

            // Act
            factory.TriggerCreation(mock.Object);

            // Assert
            Assert.NotNull(received);
            Assert.Equal("test", received!.Cloned.Value);
            Assert.NotSame(mock.Object, received.Cloned);
        }

        /// <summary>
        /// Verifies that Create throws an ArgumentNullException if the handler is not set.
        /// </summary>
        [Fact]
        public void Create_ThrowsIfHandlerNotSet()
        {
            // Arrange
            var factory = new ClonedNotifierFactory<DummyArgs>();
            var args = new DummyArgs { Value = "test" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => factory.TriggerCreation(args));
        }
    }
}