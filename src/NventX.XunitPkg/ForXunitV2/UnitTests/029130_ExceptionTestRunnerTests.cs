using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NventX.Xunit.ExceptionTesting;
using NventX.Xunit.ExceptionTesting.NventX.Xunit.ExceptionTesting;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.ForXunit2V.UnitTests
{
    /*
    /// <summary>
    /// Tests for the ExceptionTestRunner class, which executes test methods and verifies that the expected exceptions are thrown.
    /// </summary>
    public class ExceptionTestRunnerTests
    {
        /// <summary>
        /// Creates an instance of ExceptionTestRunner for testing purposes.
        /// </summary>
        private static ExceptionTestRunner CreateRunner(
            MethodInfo method,
            Type? expectedExceptionType,
            string? expectedMessageSubstring,
            out ExceptionAggregator aggregator)
        {
            var testCaseMock = new Mock<IExceptionTestCase>();
            testCaseMock.As<ITestCase>();
            testCaseMock.Setup(x => x.ExpectedExceptionType).Returns(expectedExceptionType);
            testCaseMock.Setup(x => x.ExpectedMessageSubstring).Returns(expectedMessageSubstring);

            var testMock = new Mock<ITest>();
            testMock.Setup(x => x.TestCase).Returns(testCaseMock.Object);

            aggregator = new ExceptionAggregator();

            return new ExceptionTestRunner(
                testMock.Object,
                Mock.Of<IMessageBus>(),
                typeof(DummyTestClass),
                [],
                method,
                [],
                skipReason: string.Empty,
                beforeAfterAttributes: [],
                aggregator,
                new CancellationTokenSource()
            );
        }

        /// <summary>
        /// Tests that the test method throws the expected exception type and message substring.
        /// </summary>
        [Fact]
        public async Task InvokeTestMethodAsyncForUT_CapturesExceptionProperly()
        {
            // Arrange
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowExpected))!;
            var runner = CreateRunner(method, typeof(InvalidOperationException), "this is a hoge message", out var aggregator);

            // Act
            var elapsed = await runner.InvokeTestMethodAsyncForUT(aggregator);

            // Assert
            Assert.True(0 <= elapsed);
            Assert.Null(aggregator.ToException()); // 成功したのでエラーなし
        }

        /// <summary>
        /// Tests that the test method fails when the expected exception type does not match.
        /// </summary>
        [Fact]
        public async Task InvokeTestMethodAsyncForUT_RecordsFailure_WhenNoExceptionThrown()
        {
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.DoNothing))!;
            var runner = CreateRunner(method, typeof(InvalidOperationException), "expected", out var aggregator);

            var elapsed = await runner.InvokeTestMethodAsyncForUT(aggregator);

            var ex = aggregator.ToException();
            Assert.NotNull(ex);
            Assert.Contains("Expected an exception", ex.Message);
        }
    }
    */
}