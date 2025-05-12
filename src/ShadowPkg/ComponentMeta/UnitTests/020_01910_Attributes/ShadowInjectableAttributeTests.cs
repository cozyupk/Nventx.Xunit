using System;
using Cozyupk.HelloShadowDI.ShadowPkg.ComponentMeta.Attributes;
using Xunit;

namespace Cozyupk.HelloShadowDI.ShadowPkg.ComponentMeta.UnitTests.Attributes
{
    /// <summary>
    /// Unit tests for the <see cref="ShadowInjectableAttribute"/> class.
    /// Verifies the behavior of its constructors and properties.
    /// </summary>
    public class ShadowInjectableAttributeTests
    {
        /// <summary>
        /// A sample interface used for testing.
        /// </summary>
        public interface IFoo { }

        /// <summary>
        /// A sample implementation of <see cref="IFoo"/> used for testing.
        /// </summary>
        public class Foo : IFoo { }

        /// <summary>
        /// Tests that the constructor correctly sets the <see cref="ShadowInjectableAttribute.ServiceType"/>
        /// and defaults the <see cref="ShadowInjectableAttribute.Scope"/> to <see cref="InjectionScope.Unspecified"/>.
        /// </summary>
        [Fact]
        public void Constructor_Should_Set_ServiceType_And_Default_Scope()
        {
            // Arrange & Act
            var attr = new ShadowInjectableAttribute(typeof(IFoo));

            // Assert
            Assert.Equal(typeof(IFoo), attr.ServiceType);
            Assert.Equal(InjectionScope.Unspecified, attr.Scope);
        }

        /// <summary>
        /// Tests that the constructor correctly sets a custom <see cref="InjectionScope"/>
        /// when provided.
        /// </summary>
        /// <param name="scope">The custom scope to test.</param>
        [Theory]
        [InlineData(InjectionScope.Singleton)]
        [InlineData(InjectionScope.Scoped)]
        [InlineData(InjectionScope.Transient)]
        public void Constructor_Should_Set_Custom_Scope(InjectionScope scope)
        {
            // Arrange & Act
            var attr = new ShadowInjectableAttribute(typeof(Foo), scope);

            // Assert
            Assert.Equal(typeof(Foo), attr.ServiceType);
            Assert.Equal(scope, attr.Scope);
        }

        /// <summary>
        /// Tests that the constructor throws an <see cref="ArgumentNullException"/>
        /// when the <see cref="ShadowInjectableAttribute.ServiceType"/> is null.
        /// </summary>
        [Fact]
        public void Constructor_Should_Throw_If_ServiceType_Is_Null()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new ShadowInjectableAttribute(null!);
            });
        }
    }
}
