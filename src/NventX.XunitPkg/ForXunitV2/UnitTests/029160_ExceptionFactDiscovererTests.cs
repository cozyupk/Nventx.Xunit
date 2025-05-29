using System;
using Moq;
using NventX.Xunit.ExceptionTesting;
using Xunit;
using Xunit.Abstractions;

namespace NventX.Xunit.ForXunit2V.UnitTests
{
    /*
    /// <summary>
    /// Unit tests for the ExceptionFactDiscoverer class.
    /// </summary>
    public class ExceptionFactDiscovererTests
    {
        /// <summary>
        /// Verifies that the constructor throws an ArgumentNullException when the message sink is null.
        /// </summary>
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenMessageSinkIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new ExceptionFactDiscoverer(null!));
            Assert.Equal("diagnosticMessageSink", ex.ParamName);
        }

        /// <summary>
        /// Verifies that the Discover method returns an ExceptionTestCase with the expected exception type and message substring
        /// when provided with correct arguments.
        /// </summary>
        [Fact]
        public void Discover_ReturnsExpectedTestCase_WithCorrectArguments()
        {
            // Arrange
            var expectedType = typeof(InvalidOperationException);
            var expectedMessage = "hoge";
            var sink = Mock.Of<IMessageSink>();
            var discoverer = new ExceptionFactDiscoverer(sink);
            var discoveryOptionsMock = new Mock<ITestFrameworkDiscoveryOptions>();
            var testMethodMock = new Mock<ITestMethod>();
            var attributeMock = new Mock<IAttributeInfo>();
            attributeMock.Setup(x => x.GetNamedArgument<Type>("ExpectedExceptionType")).Returns(expectedType);
            attributeMock.Setup(x => x.GetNamedArgument<string>("ExpectedMessageSubstring")).Returns(expectedMessage);

            // Act
            var testCases = discoverer.Discover(discoveryOptionsMock.Object, testMethodMock.Object, attributeMock.Object);

            // Assert
            var enumerator = testCases.GetEnumerator();
            Assert.True(enumerator.MoveNext());

            var testCase = enumerator.Current as ExceptionTestCase;
            Assert.NotNull(testCase);
            Assert.Equal(expectedType, testCase!.ExpectedExceptionType);
            Assert.Equal(expectedMessage, testCase.ExpectedMessageSubstring);
        }
    }
    */
}