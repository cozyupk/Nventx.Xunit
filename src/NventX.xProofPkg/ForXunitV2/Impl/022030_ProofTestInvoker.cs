using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NventX.xProof.Abstractions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit
{
    /// <summary>
    /// A test invoker for tests that expect a proof to be verified during their execution.
    /// </summary>
    internal class ProofTestInvoker : XunitTestInvoker
    {
        /// <summary>
        /// The test case that contains the proof to be verified.
        /// </summary>
        private ITestCaseForProof ProofTestCase { get; }

        /// <summary>
        /// Stopwatch to measure the execution time of the test method.
        /// </summary>
        private Stopwatch StopwatchToMeasureExecutionTime { get; } = new Stopwatch();

        /// <summary>
        /// Adds a new object to the front of an existing array of objects.
        /// </summary>
        private static object[] AddProofToFront(object[] original, ITestProof proof)
        {
            object[] result = new object[original.Length + 1];
            result[0] = proof;
            Array.Copy(original, 0, result, 1, original.Length);
            return result;
        }

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
            if (Test.TestCase is not ITestCaseForProof proofTestCase)
            {
                throw new InvalidOperationException($"Test case is not of type ITestCaseForProof<ITestProof>: {Test.GetType()}");
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
            Starting,
            InvokingTestMethod,
            ValidatingAfterProbing
        }

        /// <summary>
        /// Invokes the test method, measures its execution time, and verifies that the expected exception is thrown.
        /// </summary>
        protected override Task<decimal> InvokeTestMethodAsync(object testClassInstance)
        {
            return Task.Run(() =>
            {
                // Create an instance of ExceptionRecorder to capture exceptions
                var proof = ProofTestCase.CreateTestProof();

                // Add the recorder as the first argument to the test method
                object[] testMethodArguments = AddProofToFront(TestMethodArguments, proof);

                // Reset the stopwatch for the test method
                StopwatchToMeasureExecutionTime.Restart();

                // Invoke the test method with the recorder as the first argument
                IEnumerable<IProbingFailure>? collectedExceptions = null;
                ExecutionPhase phase = ExecutionPhase.Starting;
                try
                {
                    proof.Setup(ProofTestCase.ProofInvocationKind);
                    phase = ExecutionPhase.InvokingTestMethod;
                    TestMethod.Invoke(testClassInstance, testMethodArguments);
                    phase = ExecutionPhase.ValidatingAfterProbing;
                    collectedExceptions = proof.CollectProbingFailure();
                }
                catch (Exception ex)
                {
                    // If an exception occurs during the test method execution, capture it and add it to the exceptions list
                    var strPhase = "unknown phase";
                    switch (phase)
                    {
                        case ExecutionPhase.Starting:
                            strPhase = "OnTestMethodStarting()";
                            break;
                        case ExecutionPhase.InvokingTestMethod:
                            strPhase = $"{TestMethod.Name}()";
                            break;
                        case ExecutionPhase.ValidatingAfterProbing:
                            strPhase = "collectedExceptions()";
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

                // ValidateAfterProbing() should not return null but we handle it gracefully
                if (collectedExceptions == null)
                {
                    // If no exceptions were collected, add an exception indicating that the proof did not validate correctly
                    Aggregator.Add(
                    new XunitException(
                            $"ValidateAfterProbing() of {proof.GetType().FullName} returned null.This violates the contract of ValidateAfterProbing(), which must always return a non - null IEnumerable<Exception>."
                        )
                    );
                }
                else
                {
                    // If exceptions were collected, add them to the list of exceptions
                    foreach(var pf in collectedExceptions)
                    {
                        Aggregator.Add(
                            new XunitException(pf.Message, pf.Exception)
                        );
                    }
                }

                // Return the elapsed time of the test method execution
                return (decimal)StopwatchToMeasureExecutionTime.Elapsed.TotalSeconds;
            });
        }
    }
}
