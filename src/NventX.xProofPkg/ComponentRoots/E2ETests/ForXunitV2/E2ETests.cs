using NventX.xProof.BaseProofLibrary;
using NventX.Xunit.SupportingBaseProofLibrary;

namespace NventX.Xunit.E2ETests.ForXunitV2
{

    public class MyTest
    {

        [FailLateFact]
        public static void SUCCESS_IF_WithConcreteArgument(FailLateProof _)
        {
        }

        /*
        [NonExceptionFact]
        public void FAIL_IF_NoArguments()
        {
        }

        [NonExceptionFact]
        public void Fail_IF_WithInvalidArgument(string _p)
        {
        }

        [NonExceptionFact]
        public void FAIL_IF_WithAbstractArgument(ITestProof _)
        {
        }

        [NonExceptionFact]
        public void FAIL_IF_WithTooMuchArguments(ITestProof _1, string _2)
        {
        }

        [ExceptionFact]
        public void FAIL_IfMethodHaveNoParameter()
        {
        }

        [ExceptionFact]
        public void FAIL_IfMethodHaveOtherThanIExceptionRecorderParameter(int _)
        {
        }

        [ExceptionFact]
        public void FAIL_IfMethodHaveMoreThanOneParameter(IExceptionRecorder recorder, int _)
        {
            recorder.Record(() => throw new InvalidOperationException("This is a test exception for test method."));
        }

        [ExceptionFact (DisplayName = "Tashiro")]
        public void SUCCESS_IfMethodHaveOnlyOneIExceptionRecorder(IExceptionRecorder recorder)
        {
            recorder.Record(() => throw new InvalidOperationException("This is a test exception for test method."));
        }

        [Fact (DisplayName = "KIKUCHI")]
        public void Hoge()
        {
        }

        [ExceptionFact(Skip = "Tashiro")]
        public void SUCCESS_IfMethodHaveOnlyOneIExceptionRecorder2(IExceptionRecorder recorder)
        {
            recorder.Record(() => throw new InvalidOperationException("This is a test exception for test method."));
        }

        [Fact(Skip = "KIKUCHI")]
        public void Hoge2()
        {
        }

        [Fact]
        public void TestException()
            => Assert.Throws<InvalidOperationException>(() => {});

        [Fact]
        public void TestException2()
        {
            var ex = Record.Exception(() => { });
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Contains("This is a test exception for test method.", ex.Message);
            ex = Record.Exception(() => { });
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Contains("This is a test exception for test method.", ex.Message);
            ex = Record.Exception(() => { });
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Contains("This is a test exception for test method.", ex.Message);
        }
    }
    */

        /*
        public class E2ETests
        {
            // Cases that should succeed.

            [ExceptionFact]
            public static void ExceptionFact_Success_If_ExceptionThrown()
            {
                throw new ArgumentNullException("hoge", "This is a test exception for test method.");
            }

            [ExceptionFact(typeof(InvalidOperationException))]
            public static void ExceptionFact_WithType_Success_If_ExceptionTypeIsExpected()
            {
                throw new InvalidOperationException("This is a test exception for test method.");
            }

            [ExceptionFact(typeof(Exception))]
            public static void ExceptionFact_WithType_Success_If_ExceptionTypeIsAssignableToExpected()
            {
                throw new InvalidOperationException("This is a test exception for test method.");
            }

            [ExceptionFact(null, "RecipeBook")]
            public static void ExceptionFact_WithMessage_Success_If_ExceptionMessageIsValid()
            {
                throw new InvalidOperationException("invalid RecipeBook.");
            }


            [ExceptionFact(typeof(InvalidOperationException), "RecipeBook")]
            public static void ExceptionFact_WithTypeAndMessage_Success_If_ExceptionTypeIsExpected_And_ExceptionMessageIsValid()
            {
                throw new InvalidOperationException("invalid RecipeBook.");
            }

            // Cases that should not succeed.
            // Note: for technical reasons, these methods are not actually executed as tests,

    #pragma warning disable IDE0079 // Delete unnecessary suppression
    #pragma warning disable xUnit1013 // Public method should be marked as test
    #pragma warning restore IDE0079 // Delete unnecessary suppression

            // [ExceptionFact]
            public static void ExceptionFact_Fail_IfNoExceptionThrown()
            {
            }

            // [ExceptionFact(typeof(InvalidOperationException))]
            public static void ExceptionFact_WithType_Fail_If_NoExceptionThrown()
            {
            }

            // [ExceptionFact(typeof(InvalidOperationException))]
            public static void ExceptionFact_WithType_Fail_If_ExceptionTypeDiffer()
            {
                throw new Exception("This is a test exception for test method.");
            }

            // [ExceptionFact(null, "RecipeBook")]
            public static void ExceptionFact_WithMessage_Fail_If_NoExceptionThrown()
            {
            }

            // [ExceptionFact(null, "RecipeBook")]
            public static void ExceptionFact_WithMessage_Fail_If_ExceptionMessageIsInvalid()
            {
                throw new InvalidOperationException("invalid book.");
            }

            // [ExceptionFact(typeof(InvalidOperationException), "RecipeBook")]
            public static void ExceptionFact_WithTypeAndMessage_Fail_If_NoExceptionThrown()
            {
            }

            // [ExceptionFact(typeof(InvalidOperationException), "RecipeBook")]
            public static void ExceptionFact_WithTypeAndMessage_Fail_If_ExceptionTypeDiffer_And_ExceptionMessageIsInvalid()
            {
                throw new Exception("invalid book.");
            }

            // [ExceptionFact(typeof(InvalidOperationException), "RecipeBook")]
            public static void ExceptionFact_WithTypeAndMessage_Fail_If_ExceptionTypeIsExpected_And_ExceptionMessageIsInvalid()
            {
                throw new InvalidOperationException("invalid book.");
            }

            // [ExceptionFact(typeof(InvalidOperationException), "RecipeBook")]
            public static void ExceptionFact_WithTypeAndMessage_Fail_If_ExceptionTypeIsDiffer_And_ExceptionMessageIsValid()
            {
                throw new Exception("invalid RecipeBook.");
            }

    #pragma warning disable IDE0079 // Delete unnecessary suppression
    #pragma warning restore xUnit1013 // Public method should be marked as test
    #pragma warning restore IDE0079 // Delete unnecessary suppression

            // Tests to verifay negative cases of ExceptionFact attribute.


            /// <summary>
            /// SpyMessageBus is a simple implementation of IMessageBus that collects messages for testing purposes.
            /// </summary>

            private class SpyMessageBus : IMessageBus
            {
                public List<IMessageSinkMessage> Messages { get; } = [];

                public bool QueueMessage(IMessageSinkMessage message)
                {
                    Messages.Add(message);
                    return true;
                }

                public void Dispose()
                {
                    // No resources to dispose
                }

            }

            /// <summary>
            /// Creates a ValueTuple containing MethodInfo, Type, and string for testing purposes.
            /// </summary>
            private static ValueTuple<MethodInfo, Type?, string?> TestMethodInfo(MethodInfo? methodInfo, Type? type, string? str)
            {
                return ValueTuple.Create(methodInfo!, type, str);
            }

            /// <summary>
            /// List of test methods that are expected to throw exceptions.
            /// </summary>
            IEnumerable<ValueTuple<MethodInfo, Type?, string?>> MethodsToTest { get; } =
            [
                TestMethodInfo(typeof(E2ETests).GetMethod("ExceptionFact_Fail_IfNoExceptionThrown"), null, null),
                TestMethodInfo(typeof(E2ETests).GetMethod("ExceptionFact_WithType_Fail_If_NoExceptionThrown"), typeof(InvalidOperationException), null),
                TestMethodInfo(typeof(E2ETests).GetMethod("ExceptionFact_WithType_Fail_If_ExceptionTypeDiffer"), typeof(InvalidOperationException), null),
                TestMethodInfo(typeof(E2ETests).GetMethod("ExceptionFact_WithMessage_Fail_If_NoExceptionThrown"), null, "RecipeBook"),
                TestMethodInfo(typeof(E2ETests).GetMethod("ExceptionFact_WithMessage_Fail_If_ExceptionMessageIsInvalid"), null, null),
                TestMethodInfo(typeof(E2ETests).GetMethod("ExceptionFact_WithTypeAndMessage_Fail_If_NoExceptionThrown"), typeof(InvalidOperationException), "RecipeBook"),
                TestMethodInfo(typeof(E2ETests).GetMethod("ExceptionFact_WithTypeAndMessage_Fail_If_ExceptionTypeDiffer_And_ExceptionMessageIsInvalid"), typeof(InvalidOperationException), "RecipeBook"),
                TestMethodInfo(typeof(E2ETests).GetMethod("ExceptionFact_WithTypeAndMessage_Fail_If_ExceptionTypeIsExpected_And_ExceptionMessageIsInvalid"), typeof(InvalidOperationException), "RecipeBook"),
                TestMethodInfo(typeof(E2ETests).GetMethod("ExceptionFact_WithTypeAndMessage_Fail_If_ExceptionTypeIsDiffer_And_ExceptionMessageIsValid"), typeof(InvalidOperationException), "RecipeBook"),
            ];

            /// <summary>
            /// Runs all test methods that are expected to throw exceptions and verifies that they fail as expected.
            /// </summary>
            [Fact]
            public async Task ExceptionFact_ReportsFailure_ForAllNegativeTestCases()
            {
                // Arrange
                var sink = new SpyMessageBus();
                var testClassType = typeof(E2ETests);
                var testCollection = new TestCollection(new TestAssembly(new ReflectionAssemblyInfo(testClassType.Assembly)), null, "Test collection for ExceptionFactTests");
                var testClass = new TestClass(testCollection, new ReflectionTypeInfo(testClassType));

                // Act 
                var summaries = new List<RunSummary>();
                foreach (var mti in MethodsToTest)
                {
                    var testMethod = new TestMethod(testClass, new ReflectionMethodInfo(mti.Item1));
                    var testCase = new ExceptionTestCase(new NullMessageSink(), TestMethodDisplay.ClassAndMethod, testMethod, null, mti.Item2, mti.Item3);

                    var runner = new XunitTestCaseRunner(
                        testCase,
                        testCase.DisplayName,
                        skipReason: null,
                        constructorArguments: [],
                        testMethodArguments: [],
                        messageBus: sink,
                        aggregator: new ExceptionAggregator(),
                        cancellationTokenSource: new System.Threading.CancellationTokenSource()
                    );
                    summaries.Add(await runner.RunAsync());
                }

                // Assert
                foreach (var summary in summaries)
                {
                    Assert.Equal(1, summary.Total);
                    Assert.Equal(1, summary.Failed);
                }
            }
        }
        */
    }
}
