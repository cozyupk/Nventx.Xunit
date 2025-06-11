using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.xProof.SupportingXunit.AdapterForTestRunner
{
    /// <summary>
    /// A test invoker for tests that expect a proof to be verified during their execution.
    /// </summary>
    internal class ProofTestInvoker : XunitTestInvoker
    {
        /// <summary>
        /// The test case that contains the proof to be verified.
        /// </summary>
        private IProofTestCase ProofTestCase { get; }

        /// <summary>
        /// Stopwatch to measure the execution time of the test method.
        /// </summary>
        private Stopwatch StopwatchToMeasureExecutionTime { get; } = new Stopwatch();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofTestInvoker"/> class.
        /// </summary>
        public ProofTestInvoker(
            ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod,
            object[] testMethodArguments, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource
        ) : base(
            test, messageBus, testClass, constructorArguments, testMethod,
            testMethodArguments, beforeAfterAttributes, aggregator, cancellationTokenSource
        )
        {
            // Validate the proof parameter and store it
            if (Test.TestCase is not IProofTestCase proofTestCase)
            {
                throw new InvalidOperationException($"Test case is not of type IProofTestCase<ITestProof>: {Test.GetType()}");
            }
            ProofTestCase = proofTestCase;
        }

        /// <summary>
        /// Invokes the test method and measures its execution time for unit testing purposes.
        /// </summary>
        internal Task<decimal> InvokeTestMethodAsyncForUT(object testClassInstance)
        {
            // This method is for unit testing purposes only.
            // It allows us to invoke the test method and measure its execution time.
            return InvokeTestMethodAsync(testClassInstance);
        }

        /// <summary>
        /// Enumeration representing the different phases of test method execution.
        /// </summary>
        enum ExecutionPhase
        {
            SettingUp,
            InvokingTestMethod,
            CollectProbingFailure
        }

        /// <summary>
        /// Invokes the test method, measures its execution time, and verifies that the expected exception is thrown.
        /// </summary>
        protected override Task<decimal> InvokeTestMethodAsync(object testClassInstance)
        {
            return Task.Run(() =>
            {
                // Reset the stopwatch for the test method
                StopwatchToMeasureExecutionTime.Restart();

                // Invoke the test method with the recorder as the first argument
                ExecutionPhase phase = ExecutionPhase.SettingUp;
                try
                {
                    var proof = TestMethodArguments.First() as IInvokableProof
                    ?? throw new InvalidOperationException(
                            $"The first argument of the test method {TestMethod.Name} must be of type IInvokableProof, but it is {TestMethodArguments.First()?.GetType()}."
                       );
                    proof.Setup(ProofTestCase.ProofInvocationKind);
                    phase = ExecutionPhase.InvokingTestMethod;
                    TestMethod.Invoke(testClassInstance, TestMethodArguments);
                }
                catch (Exception ex)
                {
                    // If the exception is a TargetInvocationException, we want to check the InnerException
                    if (ex is TargetInvocationException tie && tie.InnerException != null)
                    {
                        // If the exception is a TargetInvocationException, we want to check the InnerException
                        ex = tie.InnerException;
                    }
                    // If an exception occurs during the test method execution, capture it and add it to the exceptions list
                    var strPhase = "unknown phase";
                    switch (phase)
                    {
                        case ExecutionPhase.SettingUp:
                            strPhase = "Setup()";
                            break;
                        case ExecutionPhase.InvokingTestMethod:
                            strPhase = $"{TestMethod.Name}()";
                            break;
                        case ExecutionPhase.CollectProbingFailure:
                            strPhase = "CollectProbingFailure()";
                            break;
                    }
                    Aggregator.Add(
                        new XunitException(
                            $"Test FAILED during {strPhase}: {ex.Message}",
                            ex
                        )
                    );
                }
                finally
                {
                    // Stop the stopwatch after the test method execution
                    StopwatchToMeasureExecutionTime.Stop();
                }

                // Return the elapsed time of the test method execution
                return (decimal)StopwatchToMeasureExecutionTime.Elapsed.TotalSeconds;
            });
        }
    }
}
