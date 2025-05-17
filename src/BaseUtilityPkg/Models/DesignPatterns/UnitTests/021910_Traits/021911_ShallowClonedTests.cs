using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.Traits;
using Moq;
using Xunit;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.UnitTests.Traits.ShallowClonedTests
{
    /// <summary>
    /// Dummy argument class implementing shallow cloning for test purposes.
    /// </summary>
    public class DummyArgs : IShallowClonable<DummyArgs>
    {
        /// <summary>
        /// Gets or sets the value for the dummy argument.
        /// </summary>
        public virtual string Value { get; set; } = "";

        /// <summary>
        /// Returns a shallow clone of the current instance.
        /// </summary>
        /// <returns>A shallow copy of this object.</returns>
        public virtual DummyArgs ShallowClone()
        {
            return (DummyArgs)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Testable subclass of ShallowCloned to capture the copied argument.
    /// </summary>
    public class TestableShallowCloned : ShallowCloned<DummyArgs>
    {
        /// <summary>
        /// Gets the captured shallow-copied argument.
        /// </summary>
        public DummyArgs? Captured { get; private set; }

        /// <summary>
        /// Initializes a new instance and invokes the base constructor.
        /// </summary>
        /// <param name="args">The argument to be cloned and.</param>
        public TestableShallowCloned(DummyArgs args) : base(args) { }

        /// <summary>
        /// Called when the argument is shallow-copied.
        /// </summary>
        /// <param name="copied">The shallow-copied argument.</param>
        protected override void OnShallowCloned(DummyArgs copied)
        {
            Captured = copied;
        }
    }

    /// <summary>
    /// Unit tests for ShallowCloned with DummyArgs.
    /// </summary>
    public class ShallowClonedTests
    {
        /// <summary>
        /// Verifies that the constructor clones the argument and stores the result.
        /// </summary>
        [Fact]
        public void Constructor_ClonesArguments_AndStoresResult()
        {
            // Arrange
            var original = new DummyArgs { Value = "Hello" };

            // Act
            var cloned = new ShallowCloned<DummyArgs>(original);

            // Assert
            Assert.NotNull(cloned.Cloned);
            Assert.NotSame(original, cloned.Cloned);
            Assert.Equal("Hello", cloned.Cloned.Value);
        }

        /// <summary>
        /// Verifies that OnShallowCloned is called with the cloned object.
        /// </summary>
        [Fact]
        public void OnShallowCloned_IsCalled_WithClonedObject()
        {
            // Arrange
            var original = new DummyArgs { Value = "World" };

            // Act
            var cloned = new TestableShallowCloned(original);

            // Assert
            Assert.NotNull(cloned.Captured);
            Assert.Equal("World", cloned.Captured!.Value);
            Assert.NotSame(original, cloned.Captured);
        }

        /// <summary>
        /// Verifies that the constructor throws an exception when the argument is null.
        /// </summary>
        [Fact]
        public void Constructor_Throws_WhenArgumentIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ShallowCloned<DummyArgs>(null!));
        }

        /// <summary>
        /// Verifies that ShallowClone is invoked by the constructor.
        /// </summary>
        [Fact]
        public void ShallowClone_IsInvoked_ByConstructor()
        {
            // Arrange
            var mock = new Mock<DummyArgs>();
            var dummyClone = new DummyArgs { Value = "Cloned" };

            // Setup mock to return dummyClone when ShallowClone is called
            mock.As<IShallowClonable<DummyArgs>>()
                .Setup(x => x.ShallowClone())
                .Returns(dummyClone);

            // Act
            var cloned = new ShallowCloned<DummyArgs>(mock.Object);

            // Assert
            Assert.Equal("Cloned", cloned.Cloned.Value);
            mock.As<IShallowClonable<DummyArgs>>().Verify(x => x.ShallowClone(), Times.Once);
        }
    }
}
