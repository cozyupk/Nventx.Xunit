namespace NventX.Xunit.ForXunit2V.UnitTests
{
    /*
    /// <summary>
    /// Test Dummy class for testing exception handling in ExceptionTestInvoker.
    /// </summary>
    public class DummyTestClass
    {
        public static void ThrowExpected() => throw new InvalidOperationException("this is a hoge message");

        public static void ThrowWrongType() => throw new ArgumentException("this is a hoge message");

        public static void ThrowWrongMessage() => throw new InvalidOperationException("unexpected message");

        public static void DoNothing() { }
    }

    /// <summary>
    /// Tests for the ExceptionTestInvoker class, which verifies that the expected exception is thrown by the test method.
    /// </summary>
    public class ExceptionTestInvokerTests
    {
        /// <summary>
        /// Creates an instance of ExceptionTestInvoker for testing purposes.
        /// </summary>
        private static ExceptionTestInvoker CreateInvoker(
            MethodInfo testMethod,
            Type? expectedExceptionType,
            string? expectedMessageSubstring,
            out ExceptionAggregator aggregator)
        {
            var testCaseMock = new Mock<IExceptionTestCase>();
            testCaseMock.As<ITestCase>(); // Important for xUnit compatibility
            testCaseMock.Setup(x => x.ExpectedExceptionType).Returns(expectedExceptionType);
            testCaseMock.Setup(x => x.ExpectedMessageSubstring).Returns(expectedMessageSubstring);

            var testMock = new Mock<ITest>();
            testMock.Setup(x => x.TestCase).Returns(testCaseMock.Object);

            aggregator = new ExceptionAggregator();

            return new ExceptionTestInvoker(
                testMock.Object,
                Mock.Of<IMessageBus>(),
                typeof(DummyTestClass),
                [],
                testMethod,
                [],
                [],
                aggregator,
                new CancellationTokenSource()
            );
        }

        /// <summary>
        /// Tests that the ExceptionTestInvoker successfully captures the expected exception type and message.
        /// </summary>
        [Fact]
        public async Task Success_When_ExceptionType_And_MessageMatch()
        {
            // Arrange
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowExpected))!;
            var instance = new DummyTestClass();
            var invoker = CreateInvoker(method, typeof(InvalidOperationException), "hoge", out var aggregator);

            // Act
            var elapsed = await invoker.InvokeTestMethodAsyncForUT(instance);

            // Assert
            Assert.True(elapsed > 0);
            Assert.Null(aggregator.ToException());
        }

        /// <summary>
        /// Tests that the ExceptionTestInvoker fails when no exception is thrown, as expected by the test case.
        /// </summary>
        [Fact]
        public async Task Fail_When_NoExceptionThrown()
        {
            // Arrange
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.DoNothing))!;
            var instance = new DummyTestClass();
            var invoker = CreateInvoker(method, typeof(InvalidOperationException), "hoge", out var aggregator);

            // Act
            await invoker.InvokeTestMethodAsyncForUT(instance);

            // Assert
            var ex = aggregator.ToException();
            Assert.Contains("Expected an exception", ex?.ToString());
        }

        /// <summary>
        /// Tests that the ExceptionTestInvoker fails when the thrown exception type does not match the expected type.
        /// </summary>
        [Fact]
        public async Task Fail_When_ExceptionTypeDoesNotMatch()
        {
            // Arrange
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowWrongType))!;
            var instance = new DummyTestClass();
            var invoker = CreateInvoker(method, typeof(InvalidOperationException), "hoge", out var aggregator);

            // Act
            await invoker.InvokeTestMethodAsyncForUT(instance);

            // Assert
            var ex = aggregator.ToException();
            Assert.Contains("Expected exception of type", ex?.ToString());
        }

        /// <summary>
        /// Tests that the ExceptionTestInvoker fails when the exception message does not match the expected message substring.
        /// </summary>
        [Fact]
        public async Task Fail_When_ExceptionMessageDoesNotMatch()
        {
            // Arrange
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowWrongMessage))!;
            var instance = new DummyTestClass();
            var invoker = CreateInvoker(method, typeof(InvalidOperationException), "hoge", out var aggregator);

            // Act
            await invoker.InvokeTestMethodAsyncForUT(instance);

            // Assert
            var ex = aggregator.ToException();
            Assert.Contains("Expected exception message to contain", ex?.ToString());
        }

        /// <summary>
        /// Tests that the ExceptionTestInvoker throws an InvalidOperationException when the test case is not of type IExceptionTestCase.
        /// </summary>
        [Fact]
        public void Fail_When_TestCaseIsNotExceptionTestCase()
        {
            // Arrange
            var testCase = new Mock<IXunitTestCase>(); // Does not implement IExceptionTestCase
            var testMock = new Mock<ITest>();
            testMock.Setup(x => x.TestCase).Returns(testCase.Object);

            // Act
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.DoNothing))!;
            var instance = new DummyTestClass();

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                new ExceptionTestInvoker(
                    testMock.Object,
                    Mock.Of<IMessageBus>(),
                    typeof(DummyTestClass),
                    [instance],
                    method,
                    [],
                    [],
                    new ExceptionAggregator(),
                    new CancellationTokenSource()
                );
            });
        }

        /// <summary>
        /// Tests that the ExceptionTestInvoker fails when the expected exception type and message do not match the actual exception thrown.
        /// </summary>
        [Fact]
        public async Task Fails_When_ExceptionType_And_Message_DoNotMatch()
        {
            // Arrange
            var method = typeof(DummyTestClass).GetMethod(nameof(DummyTestClass.ThrowWrongType))!;
            var instance = new DummyTestClass();

            // Mock: Expected InvalidOperationException + "expected_substring"
            var testCaseMock = new Mock<IExceptionTestCase>();
            testCaseMock.As<ITestCase>();
            testCaseMock.Setup(x => x.ExpectedExceptionType).Returns(typeof(InvalidOperationException));
            testCaseMock.Setup(x => x.ExpectedMessageSubstring).Returns("expected_substring");

            var testMock = new Mock<ITest>();
            testMock.Setup(x => x.TestCase).Returns(testCaseMock.Object);

            var aggregator = new ExceptionAggregator();

            var invoker = new ExceptionTestInvoker(
                testMock.Object,
                Mock.Of<IMessageBus>(),
                typeof(DummyTestClass),
                [],
                method,
                [],
                [],
                aggregator,
                new CancellationTokenSource()
            );

            // Act
            await invoker.InvokeTestMethodAsyncForUT(instance);

            // Assert
            var exception = aggregator.ToException();
            Assert.NotNull(exception);
            Assert.Contains("Expected exception of type", exception.Message);
            Assert.Contains("Expected exception message to contain", exception.Message);
        }
    }
    */
}