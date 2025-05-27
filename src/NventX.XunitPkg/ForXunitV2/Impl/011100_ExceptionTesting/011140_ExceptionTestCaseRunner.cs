using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NventX.Xunit.ExceptionTesting.NventX.Xunit.ExceptionTesting;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.ExceptionTesting
{
    /// <summary>
    /// Runs a test case that expects an exception to be thrown.
    /// </summary>
    internal class ExceptionTestCaseRunner : XunitTestCaseRunner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTestCaseRunner"/> class.
        /// </summary>
        public ExceptionTestCaseRunner(IExceptionTestCase testCase, string displayName, IMessageBus messageBus, object[] constructorArguments,
                               object[] testMethodArguments, string skipReason,
                               ExceptionAggregator aggregator,
                               CancellationTokenSource cancellationTokenSource
        ) : base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator, cancellationTokenSource)
        {
            // no additional initialization required
        }

        /// <summary>
        /// Creates a test runner for the specified test case.
        /// </summary>
        protected override XunitTestRunner CreateTestRunner(
            ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod,
            object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            return new ExceptionTestRunner(
                test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
                skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator), cancellationTokenSource
            );
        }

        /// <summary>
        /// Runs the test case asynchronously and returns a summary of the run.
        /// </summary>
        protected override async Task<RunSummary> RunTestAsync()
        {
            // execute the test method and verify that the expected exception is thrown
            var summary = await base.RunTestAsync();

            /*
            // If the test case has more thatn one unexpected exception, we consider it as failed.
            if (base.BeforeAfterAttributes.AsTracker()
            {
                Console.WriteLine($"Test case '{DisplayName}' failed with unexpected exceptions: {Aggregator.ToString()}");
                ++summary.Failed;
            }
            else
            {
                Console.WriteLine($"Test case '{DisplayName}' succeeded: {Aggregator.ToString()}");
            }
            */

            // return the summary of the test run
            return summary;
        }
    }
}