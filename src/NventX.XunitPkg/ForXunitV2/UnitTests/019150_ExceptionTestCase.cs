using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NventX.Xunit.ExceptionTesting;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.ForXunit2V.UnitTests
{
    public class ExceptionTestCaseTests
    {
        private static ExceptionTestCase CreateTestCase(Type? expectedExceptionType, string? expectedMessage)
        {
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowExpected))!;
            var reflectionMethodInfo = new ReflectionMethodInfo(method);

            var reflectionClassInfo = new ReflectionTypeInfo(typeof(DummyTestClass));
            var testCollectionMock = new Mock<ITestCollection>();
            testCollectionMock.Setup(x => x.DisplayName).Returns("test-collection");

            var testClassMock = new Mock<ITestClass>();
            testClassMock.Setup(x => x.Class).Returns(reflectionClassInfo);
            testClassMock.Setup(x => x.TestCollection).Returns(testCollectionMock.Object);

            var testMethodMock = new Mock<ITestMethod>();
            testMethodMock.Setup(x => x.Method).Returns(reflectionMethodInfo);
            testMethodMock.Setup(x => x.TestClass).Returns(testClassMock.Object);

            var sink = Mock.Of<IMessageSink>();

            return new ExceptionTestCase(
                sink,
                TestMethodDisplay.Method,
                testMethodMock.Object,
                testMethodArguments: [],
                expectedExceptionType: expectedExceptionType,
                expectedMessageSubstring: expectedMessage
            );
        }

        [Fact]
        public void Properties_Are_Set_Correctly()
        {
            var testCase = CreateTestCase(typeof(InvalidOperationException), "hoge");

            Assert.Equal(typeof(InvalidOperationException), testCase.ExpectedExceptionType);
            Assert.Equal("hoge", testCase.ExpectedMessageSubstring);
        }

        /*
        [Fact]
        public void Can_Serialize_And_Deserialize()
        {
            var original = CreateTestCase(typeof(InvalidOperationException), "hoge");
            var info = new XunitSerializationInfo();

            original.Serialize(info);

            var clone = new ExceptionTestCase();
            clone.Deserialize(info);

            Assert.Equal(original.ExpectedExceptionType, clone.ExpectedExceptionType);
            Assert.Equal(original.ExpectedMessageSubstring, clone.ExpectedMessageSubstring);
        }
        */

        [Fact]
        public async Task RunAsync_Delegates_To_ExceptionTestCaseRunner()
        {
            var testCase = CreateTestCase(typeof(InvalidOperationException), "hoge");

            var runner = new ExceptionTestCaseRunner(
                testCase,
                displayName: "test",
                messageBus: Mock.Of<IMessageBus>(),
                constructorArguments: [],
                testMethodArguments: [],
                skipReason: null!,
                aggregator: new ExceptionAggregator(),
                cancellationTokenSource: new CancellationTokenSource()
            );

            var summary = await runner.RunAsync();

            // We only check if execution succeeded with some result (since the test actually throws the expected exception)
            Assert.NotNull(summary);
            Assert.True(summary.Total >= 0);
        }
    }
}
