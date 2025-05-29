using System;
using System.Collections.Generic;
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
    /*
    /// <summary>
    /// Simple implementation of IXunitSerializationInfo for testing serialization in unit tests.
    /// </summary>
    internal class XunitSerializationInfo : IXunitSerializationInfo
    {
        private readonly Dictionary<string, (object? value, Type type)> _data = [];

        public void AddValue(string key, object value, Type type)
            => _data[key] = (value, type);

        public T GetValue<T>(string key)
            => (T)_data[key].value!;

        public object GetValue(string key, Type type)
        {
            if (!_data.TryGetValue(key, out var result))
            {
                throw new KeyNotFoundException($"Key '{key}' not found in serialization info.");
            }
            return result;
        }
    }

    /// <summary>
    /// Tests for the ExceptionTestCase class, which represents a test case that expects an exception to be thrown.
    /// </summary>
    public class ExceptionTestCaseTests
    {
        /// <summary>
        /// Creates a test case for the specified method and expected exception type and message.
        /// </summary>
        private static ExceptionTestCase CreateTestCase(MethodInfo method, Type testClass, Type? expectedExceptionType, string? expectedMessageSubstring)
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

            var sink = Mock.Of<IMessageSink>();

            return new ExceptionTestCase(
                sink,
                TestMethodDisplay.Method,
                testMethodMock.Object,
                testMethodArguments: [],
                expectedExceptionType: expectedExceptionType,
                expectedMessageSubstring: expectedMessageSubstring
            );
        }

        /// <summary>
        /// Tests that the ExceptionTestCase constructor sets the properties correctly when provided with valid parameters.
        /// </summary>
        [Fact]
        public void Properties_Are_Set_Correctly()
        {
            var testCase = CreateTestCase(
                typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowExpected))!,
                typeof(DummyTestClass), typeof(InvalidOperationException), "hoge");

            Assert.Equal(typeof(InvalidOperationException), testCase.ExpectedExceptionType);
            Assert.Equal("hoge", testCase.ExpectedMessageSubstring);
        }

        /// <summary>
        /// Tests that the ExceptionTestCase can be serialized and deserialized correctly.
        /// </summary>
        [Fact]
        public void Can_Serialize_And_Deserialize()
        {
            var original = CreateTestCase(
                typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowExpected))!,
                typeof(DummyTestClass), typeof(InvalidOperationException), "hoge");
            var info = new XunitSerializationInfo();

            original.Serialize(info);
#pragma warning disable CS0618 // Type or member is obsolete
            var clone = new ExceptionTestCase();
#pragma warning restore CS0618 // Type or member is obsolete
            clone.Deserialize(info);

            Assert.Equal(original.ExpectedExceptionType, clone.ExpectedExceptionType);
            Assert.Equal(original.ExpectedMessageSubstring, clone.ExpectedMessageSubstring);
        }

        [Fact]
        public async Task RunAsync_Delegates_To_ExceptionTestCaseRunner()
        {
            var testCase = CreateTestCase(
                typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowExpected))!,
                typeof(DummyTestClass), typeof(InvalidOperationException), "hoge");

            var messageBusMock = new Mock<IMessageBus>();
            messageBusMock.Setup(m => m.QueueMessage(It.IsAny<IMessageSinkMessage>())).Returns(true);

            var runner = new ExceptionTestCaseRunner(
                testCase,
                displayName: "test",
                messageBus: messageBusMock.Object,
                constructorArguments: [],
                testMethodArguments: [],
                skipReason: null!,
                aggregator: new ExceptionAggregator(),
                cancellationTokenSource: new CancellationTokenSource()
            );

            var summary = await runner.RunAsync();

            // We only check if execution succeeded with some result (since the test actually throws the expected exception)
            Assert.NotNull(summary);
            Assert.True(0 < summary.Total);
        }

        /// <summary>
        /// Tests that the RunAsync method returns a RunSummary with the expected total count when the test case is successful.
        /// </summary>
        [Fact]
        public async Task RunAsync_Returns_RunSummary_When_Successful()
        {
            var testCase = CreateTestCase(
                typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowExpected))!,
                typeof(DummyTestClass), typeof(InvalidOperationException), "hoge");

            var messageBusMock = new Mock<IMessageBus>();
            messageBusMock.Setup(m => m.QueueMessage(It.IsAny<IMessageSinkMessage>())).Returns(true);

            var result = await testCase.RunAsync(
                Mock.Of<IMessageSink>(),
                messageBusMock.Object,
                [],
                new ExceptionAggregator(),
                new CancellationTokenSource());

            Assert.NotNull(result);
            Assert.Equal(1, result.Total); // 1 test executed
        }

        /// <summary>
        /// Tests that the RunAsync method handles a missing exception correctly, meaning it should not throw an exception but return a RunSummary indicating failure.
        /// </summary>
        [Fact]
        public async Task RunAsync_Handles_Missing_Exception()
        {
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.DoNothing))!;
            var testMethod = Mocks.TestMethod(method, typeof(DummyTestClass));
            var testCase = new ExceptionTestCase(Mock.Of<IMessageSink>(), TestMethodDisplay.Method, testMethod, null, typeof(ArgumentNullException), "expected");

            var messageBusMock = new Mock<IMessageBus>();
            messageBusMock.Setup(m => m.QueueMessage(It.IsAny<IMessageSinkMessage>())).Returns(true);

            var aggregator = new ExceptionAggregator();
            var result = await testCase.RunAsync(
                Mock.Of<IMessageSink>(),
                messageBusMock.Object,
                [],
                aggregator,
                new CancellationTokenSource());

            var ex = aggregator.ToException();
            Assert.Null(ex);  // The outer aggregator does not record the exception, as an internal child aggregator is used during test execution.
            Assert.Equal(1, result.Total);
            Assert.Equal(1, result.Failed);
        }

        /// <summary>
        /// Mocks for creating test methods and classes for unit tests.
        /// </summary>
        private static class Mocks
        {
            public static ITestMethod TestMethod(MethodInfo method, Type testClass)
            {
                var typeInfo = new ReflectionTypeInfo(testClass);
                var methodInfo = new ReflectionMethodInfo(method);

                var testClassMock = new Mock<ITestClass>();
                var testCollectionMock = new Mock<ITestCollection>();
                testClassMock.Setup(x => x.Class).Returns(typeInfo);
                testClassMock.Setup(x => x.TestCollection).Returns(testCollectionMock.Object);

                var testMethodMock = new Mock<ITestMethod>();
                testMethodMock.Setup(x => x.Method).Returns(methodInfo);
                testMethodMock.Setup(x => x.TestClass).Returns(testClassMock.Object);

                return testMethodMock.Object;
            }
        }
    }
    */
}
