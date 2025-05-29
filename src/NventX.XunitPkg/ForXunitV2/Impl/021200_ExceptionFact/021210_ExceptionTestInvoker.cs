using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NventX.Xunit.ExceptionTesting;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.ExceptionFact
{
    /// <summary>
    /// Represents a test invoker that executes a test method and verifies that the expected exception is thrown.
    /// </summary>
    internal class ExceptionTestInvoker : XunitTestInvoker
    {
        /// <summary>
        /// Represents the test case that to test for expected exceptions.
        /// </summary>
        private IExceptionTestCase ExceptionTestCase { get; }

        /// <summary>
        /// Stopwatch to measure the execution time of the test method.
        /// </summary>
        private Stopwatch StopwatchToMeasureExecutionTime { get; } = new Stopwatch();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTestInvoker"/> class.
        /// </summary>
        public ExceptionTestInvoker(
            ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod,
            object[] testMethodArguments, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource
        ) : base(
            test, messageBus, testClass, constructorArguments, testMethod,
            testMethodArguments, beforeAfterAttributes, aggregator, cancellationTokenSource
        )
        {
            // Check if the test case is of type ExceptionExpectedTestCase
            if (Test.TestCase is not IExceptionTestCase exceptionTestCase)
            {
                // If the test case is not of the expected type, cancel the test and throw an exception
                throw new InvalidOperationException($"Test case is not of type ExceptionExpectedTestCase: {Test.GetType()}");
            }

            // Hold the expected exception test case for later use
            ExceptionTestCase = exceptionTestCase;
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
            return Task.Run(() =>
            {
                // Reset the stopwatch for the test method
                StopwatchToMeasureExecutionTime.Restart();

                // Record the exception thrown by the test method, if any
                Exception? thrown = null;

                // Try to invoke the test method and catch any exceptions that may occur
                try
                {
                    // Invoke the test method and catch any exceptions
                    TestMethod.Invoke(testClassInstance, TestMethodArguments);
                }
                catch (TargetInvocationException tie) when (tie.InnerException != null)
                {
                    // If the exception is a TargetInvocationException, we want to check the InnerException
                    thrown = tie.InnerException;
                }
                finally
                {
                    // Stop the stopwatch after the test method execution
                    StopwatchToMeasureExecutionTime.Stop();
                }

                // Check if an exception was thrown and if it matches the expected type
                if (thrown == null)
                {
                    Aggregator.Add(new XunitException($"Expected an exception, but no exception was thrown."));
                }
                else
                {
                    // If there is expected type of the exception, check if the thrown exception matches it
                    // Note: Nested if statements were used for unit test coverage purposes
                    if (ExceptionTestCase.ExpectedExceptionType != null)
                    {
                        if (!ExceptionTestCase.ExpectedExceptionType.IsAssignableFrom(thrown.GetType()))
                        {
                            Aggregator.Add(new XunitException($"Expected exception of type '{ExceptionTestCase.ExpectedExceptionType.Name}', but got '{thrown.GetType().Name}' instead."));
                        }
                    }
                    // If there is an expected message, check if the thrown exception's message contains it
                    if (ExceptionTestCase.ExpectedMessageSubstring != null)
                    {
                        if(!thrown.Message.Contains(ExceptionTestCase.ExpectedMessageSubstring))
                        {
                            Aggregator.Add(new XunitException($"Expected exception message to contain '{ExceptionTestCase.ExpectedMessageSubstring}', but got '{thrown.Message}' instead."));
                        }
                    }
                }

                // Return the elapsed time of the test method execution
                return (decimal)StopwatchToMeasureExecutionTime.Elapsed.TotalSeconds;
            });
        }
    }
}
