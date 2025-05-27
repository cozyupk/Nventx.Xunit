using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NventX.Xunit.ExceptionTesting;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.ForXunit2V.UnitTests
{
    public class ExceptionTestCaseRunnerTests
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExceptionTestCaseRunner"/> for testing purposes.
        /// </summary>
        private static ExceptionTestCaseRunner CreateCaseRunner(
            MethodInfo method,
            Type testClass,
            Type? expectedExceptionType,
            string? expectedMessageSubstring,
            out ExceptionAggregator aggregator)
        {
            var reflectionClassInfo = new ReflectionTypeInfo(testClass);
            var reflectionMethodInfo = new ReflectionMethodInfo(method);

            var testCollectionMock = new Mock<ITestCollection>();
            testCollectionMock.Setup(x => x.DisplayName).Returns("test-collection");

            var testClassMock = new Mock<ITestClass>();
            testClassMock.Setup(x => x.Class).Returns(reflectionClassInfo);
            testClassMock.Setup(x => x.TestCollection).Returns(testCollectionMock.Object);

            var testMethodMock = new Mock<ITestMethod>();
            testMethodMock.Setup(x => x.Method).Returns(reflectionMethodInfo);
            testMethodMock.Setup(x => x.TestClass).Returns(testClassMock.Object);

            var testCaseMock = new Mock<IExceptionTestCase>();
            testCaseMock.Setup(x => x.ExpectedExceptionType).Returns(expectedExceptionType);
            testCaseMock.Setup(x => x.ExpectedMessageSubstring).Returns(expectedMessageSubstring);
            testCaseMock.Setup(x => x.Method).Returns(testMethodMock.Object.Method);
            testCaseMock.Setup(x => x.TestMethod).Returns(testMethodMock.Object);
            testCaseMock.Setup(x => x.TestMethodArguments).Returns([]);
            testCaseMock.Setup(x => x.DisplayName).Returns("test-display");
            testCaseMock.Setup(x => x.SkipReason).Returns((string?)null!);

            aggregator = new ExceptionAggregator();

            var messageBusMock = new Mock<IMessageBus>();
            messageBusMock.Setup(x => x.QueueMessage(It.IsAny<IMessageSinkMessage>())).Returns(true);

            return new ExceptionTestCaseRunner(
                testCaseMock.Object,
                displayName: "test-display",
                messageBus: messageBusMock.Object,
                constructorArguments: [],
                testMethodArguments: [],
                skipReason: null!,
                aggregator,
                new CancellationTokenSource()
            );
        }

        /// <summary>
        /// Tests that the test method does not throw an exception when the SUT does not throw an exception, but the expected exception type is specified.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RunAsync_Succeeds_WhenNoExceptionThrown_ButExpected()
        {
            // Arrange
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.DoNothing))!;
            var runner = CreateCaseRunner(method, typeof(DummyTestClass), typeof(ArgumentNullException), "hoge", out var aggregator);

            // Act
            RunSummary? result = null;
            var ex = await Record.ExceptionAsync(async () => { result = await runner.RunAsync(); });

            // Assert
            Assert.Null(ex);
            Assert.NotNull(result);
            Assert.Equal(1, result.Total); // 1 test executed
            Assert.Equal(1, result.Failed); // test failed
        }

        /// <summary>
        /// Tests that the test method succeed when the expected exception type and message substring match the thrown exception.
        /// </summary>
        [Fact]
        public async Task RunAsync_Succeeds_WhenExpectedExceptionThrown()
        {
            // Arrange
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowExpected))!;
            var runner = CreateCaseRunner(method, typeof(DummyTestClass), typeof(InvalidOperationException), "hoge", out var aggregator);

            // Act
            RunSummary? result = null;
            var ex = await Record.ExceptionAsync(async () => { result = await runner.RunAsync(); });

            // Assert
            Assert.Null(ex);
            Assert.Null(aggregator.ToException());
            Assert.NotNull(result);
            Assert.Equal(1, result.Total); // 1 test executed
            Assert.Equal(0, result.Failed); // no test failed
        }
    }
}
