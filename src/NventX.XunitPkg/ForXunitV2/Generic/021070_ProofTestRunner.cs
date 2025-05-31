using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.Generic
{
    internal class ProofTestRunner : XunitTestRunner
    {

        public ProofTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
                               MethodInfo testMethod, object[] testMethodArguments, string skipReason,
                               IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator,
                               CancellationTokenSource cancellationTokenSource
            ) : base(
                test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
                skipReason, beforeAfterAttributes, aggregator, cancellationTokenSource)
        {
            // no additional initialization required
        }

        /// <summary>
        /// Invokes the test method and measures its execution time for unit testing purposes.
        /// </summary>
        internal Task<decimal> InvokeTestMethodAsyncForUT(ExceptionAggregator aggregator)
        {
            // This method is for unit testing purposes only.
            // It allows us to invoke the test method and measure its execution time.
            return InvokeTestMethodAsync(aggregator);
        }

        /// <summary>
        /// Invokes the test method and measures its execution time.
        /// </summary>
        protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
        {
            // Note: We use the aggregator passed as a parameter here instead of the base class's Aggregator,
            // because the provided aggregator may be a child (nested) aggregator of the base Aggregator.
            // This ensures that exceptions are correctly propagated and isolated within the intended scope.
            return new ProofTestInvoker(
                         Test, MessageBus, TestClass, ConstructorArguments,
                         TestMethod, TestMethodArguments, BeforeAfterAttributes,
                         aggregator, CancellationTokenSource
                       ).RunAsync();
        }
    }
}
