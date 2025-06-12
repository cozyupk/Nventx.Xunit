using System;
using System.Collections.Generic;
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
            Aggregator.Run(() =>
            {
                var proof = TestMethodArguments.First() as IInvokableProof
                ?? throw new InvalidOperationException(
                        $"The first argument of the test method {TestMethod.Name} must be of type IInvokableProof, but it is {TestMethodArguments.First()?.GetType()}."
                   );
                proof.Setup(ProofTestCase.ProofInvocationKind);
            });

            return base.InvokeTestMethodAsync(testClassInstance);
        }
    }
}
