using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit.Abstractions;
using Xunit.Sdk;
using Xunit;
using NventX.Xunit.ExceptionTesting;

namespace NventX.Xunit.ForXunit2V.UnitTests
{
    public class ExceptionFactDiscovererTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenMessageSinkIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new ExceptionFactDiscoverer(null!));
            Assert.Equal("diagnosticMessageSink", ex.ParamName);
        }

        [Fact]
        public void Discover_ReturnsExpectedTestCase_WithCorrectArguments()
        {
            // Arrange
            var expectedType = typeof(InvalidOperationException);
            var expectedMessage = "hoge";

            var sink = Mock.Of<IMessageSink>();

            var discoverer = new ExceptionFactDiscoverer(sink);

            var discoveryOptionsMock = new Mock<ITestFrameworkDiscoveryOptions>();
            // discoveryOptionsMock.Setup(x => x.MethodDisplayOrDefault()).Returns(TestMethodDisplay.ClassAndMethod);

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
}
