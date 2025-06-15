using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestRunner;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xproof.SupportingXunit.AdapterForTestRunner
{
    /// <summary>
    /// A test invoker for tests that expect a proof to be verified during their execution.
    /// </summary>
    internal class ProofTestInvoker<TAxes> : XunitTestInvoker
    {
        /// <summary>
        /// The test case that contains the proof to be verified.
        /// </summary>
        private IProofTestCase ProofTestCase { get; }

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
        /// Invokes the test method, measures its execution time, and verifies that the expected exception is thrown.
        /// </summary>
        protected override Task<decimal> InvokeTestMethodAsync(object testClassInstance)
        {
            // Note: To avoid confusion, we do not use a setup and test method integrated virtual method here.

            // Measure the setup time for the proof.
            Timer.Aggregate(() =>
            {
                // Any exception thrown during the setup will be caught by the aggregator.
                Aggregator.Run(() =>
                {
                    var proof = TestMethodArguments.First() as IInvokableProof<TAxes>
                    ?? throw new InvalidOperationException(
                            $"The first argument of the test method {TestMethod.Name} must be of type IInvokableProof, but it is {TestMethodArguments.First()?.GetType()}."
                       );
                    proof.Setup(ProofTestCase.ProofInvocationKind);
                });
            });

            // Invoke the test method and measure its execution time,
            // and return total execution time, including setup time, in seconds.
            return base.InvokeTestMethodAsync(testClassInstance);
        }
    }
}
