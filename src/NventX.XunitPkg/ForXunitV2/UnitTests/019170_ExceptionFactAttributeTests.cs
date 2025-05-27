using System;
using NventX.Xunit.ExceptionTesting;
using Xunit;

namespace NventX.Xunit.ForXunit2V.UnitTests
{
    namespace NventX.Xunit.ForXunit2V.UnitTests
    {
        public class ExceptionFactAttributeTests
        {
            [Fact]
            public void Constructor_SetsPropertiesCorrectly_WhenValuesProvided()
            {
                // Arrange
                var expectedType = typeof(InvalidOperationException);
                var expectedMessage = "expected message";

                // Act
                var attr = new ExceptionFactAttribute(expectedType, expectedMessage);

                // Assert
                Assert.Equal(expectedType, attr.ExpectedExceptionType);
                Assert.Equal(expectedMessage, attr.ExpectedMessageSubstring);
            }

            [Fact]
            public void Constructor_SetsPropertiesToNull_WhenNoArgumentsProvided()
            {
                // Act
                var attr = new ExceptionFactAttribute();

                // Assert
                Assert.Null(attr.ExpectedExceptionType);
                Assert.Null(attr.ExpectedMessageSubstring);
            }
        }
    }
}
