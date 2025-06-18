using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xproof.SupportingXunit.AdapterForTestRunner
{
    /// <summary>
    /// Represents a test case runner for tests that expect a proof to be verified during their execution.
    /// </summary>
    public class ProofTestCaseRunner : XunitTestCaseRunner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFactTestCaseRunner"/> class.
        /// </summary>
        public ProofTestCaseRunner(IProofTestCase testCase, string displayName, IMessageBus messageBus, object[] constructorArguments,
                               object[] testMethodArguments, string skipReason,
                               ExceptionAggregator aggregator,
                               CancellationTokenSource cancellationTokenSource
        ) : base(testCase, displayName, skipReason, constructorArguments, testMethodArguments,
                 new ProofCoordinatorBus(testMethodArguments[0], testCase, messageBus, cancellationTokenSource),
                 aggregator, cancellationTokenSource)
        {
            // No additional initialization is needed here,
        }

        /// <summary>
        /// Creates a test runner for the specified test case.
        /// </summary>
        protected override XunitTestRunner CreateTestRunner(
            ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod,
            object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            Console.WriteLine($"Creating ProofTestInvoker for {messageBus.GetType().Name} arguments.");
            // create a ProofTestInvoker with the proof instance
            var runner = new ProofTestRunner(
                test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
                skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator), cancellationTokenSource
            );

            // return the runner
            return runner;
        }

        /// <summary>
        /// Runs the test case asynchronously and returns a summary of the run.
        /// </summary>
        protected override Task<RunSummary> RunTestAsync()
        {
            // Run the base test case runner to execute the test method and aggregate the results with ProofCoordinatorBus.
            return base.RunTestAsync()
                       .ContinueWith(task =>
                       {
                           if (MessageBus is not ProofCoordinatorBus proxyMessageBus)
                           {
                               throw new InvalidOperationException("MessageBus is not of type ProofCoordinatorBus.");
                           }
                           return proxyMessageBus.FinalizeCoordinationAsync(task.Result).Result;
                       });
        }
    }
}
