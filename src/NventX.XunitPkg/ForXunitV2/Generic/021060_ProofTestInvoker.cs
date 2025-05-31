using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.Generic
{
    internal class ProofTestInvoker : XunitTestInvoker
    {
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

        internal Task<decimal> InvokeTestMethodAsyncForUT(object testClassInstance)
        {
            // This method is for unit testing purposes only.
            // It allows us to invoke the test method and measure its execution time.
            return InvokeTestMethodAsync(testClassInstance);
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
                // Create an instance of ExceptionRecorder to capture exceptions
                var proof = ProofTestCase.TestProofFactory.Create();

                // Add the recorder as the first argument to the test method
                object[] testMethodArguments = AddProofToFront(TestMethodArguments, proof);
                // Invoke the test method with the recorder as the first argument
                try
                {
                    proof.OnTestMethodStarting();
                    TestMethod.Invoke(testClassInstance, testMethodArguments);
                    proof.OnTestMethodCompleted();
                }
                catch (Exception ex)
                {
                    Aggregator.Add(new
                      XunitException("Test FAILED: ", ex)
                    );
                } finally
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
