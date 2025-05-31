using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NventX.Xunit.Abstractions;
using NventX.Xunit.ExceptionTest;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.ExceptionFact
{
    /// <summary>
    /// Represents a test invoker that executes a test method and verifies that the expected exception is thrown.
    /// </summary>
    internal class ExceptionFactTestInvoker : XunitTestInvoker
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
        /// Records exceptions thrown during the test method execution and provides validation against expected exceptions.
        /// </summary>
        protected internal class ExceptionRecorder : IExceptionRecorder
        {
            /// <summary>
            /// Gets the exception that was captured during the execution of the recorded action, if any.
            /// </summary>
            public Exception? Captured { get; private set; }

            /// <summary>
            /// Indicates whether the <see cref="Record"/> or <see cref="RecordAsync"/> method was called during the test method execution.
            /// </summary>
            private int _recordCalled = 0;
            public bool RecordCalled => _recordCalled != 0;

            /// <summary>
            /// The message to ensures that the <see cref="RecordAsync"/> method is called only once.
            /// </summary>
            private string EnsureRecordCalledMessage { get; } = ".RecordAsync() must only be called once.";

            /// <summary>
            /// Records an exception thrown by the specified action.
            /// </summary>
            /// <param name="action"></param>
            public void Record(Action action)
            {
                Captured = RecordCore(action);
            }

            /// <summary>
            /// Records an exception thrown by the specified asynchronous action.
            /// </summary>
            public async Task RecordAsync(Func<Task> action)
            {
                if (Interlocked.Exchange(ref _recordCalled, 1) != 0)
                    throw new InvalidOperationException(EnsureRecordCalledMessage);

                Captured = await RecordCoreAsync(action);
            }

            /// <summary>
            /// Records an exception thrown by the specified action and returns it.
            /// </summary>
            private Exception? RecordCore(Action action)
            {
                if (Interlocked.Exchange(ref _recordCalled, 1) != 0)
                    throw new InvalidOperationException(EnsureRecordCalledMessage);

                try { action(); return null; }
                catch (Exception ex) { return ex; }
            }

            /// <summary>
            /// Records an exception thrown by the specified asynchronous action and returns it.
            /// </summary>
            private async Task<Exception?> RecordCoreAsync(Func<Task> action)
            {
                try { await action(); return null; }
                catch (Exception ex) { return ex; }
            }
        }

        /// <summary>
        /// Adds a new object to the front of an existing array of objects.
        /// </summary>
        private static object[] AddRecorderToFront(object[] original, IExceptionRecorder recorder)
        {
            object[] result = new object[original.Length + 1];
            result[0] = recorder;
            Array.Copy(original, 0, result, 1, original.Length);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFactTestInvoker"/> class.
        /// </summary>
        public ExceptionFactTestInvoker(
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
                // Create an instance of ExceptionRecorder to capture exceptions
                var recorder = new ExceptionRecorder();
                // Add the recorder as the first argument to the test method
                object[] testMethodArguments = AddRecorderToFront(TestMethodArguments, recorder);
                // Invoke the test method with the recorder as the first argument
                try
                {
                    TestMethod.Invoke(testClassInstance, testMethodArguments);
                }
                catch (Exception ex)
                {
                    Aggregator.Add(new
                      XunitException("Test method threw exception outside of .Record()", ex)
                    );
                }

                // Stop the stopwatch after the test method execution
                StopwatchToMeasureExecutionTime.Stop();

                // Check if the recorder was called and if an exception was captured
                if (!recorder.RecordCalled)
                {
                    Aggregator.Add(new XunitException("No call to .Record() was made during test method execution."));
                }
                else if (recorder.Captured == null)
                {
                    Aggregator.Add(new XunitException("Expected an exception, but none was thrown."));
                }
                else
                {
                    // Validate the captured exception against the expected exception type and message substring
                    if (ExceptionTestCase.ExpectedExceptionType != null)
                    {
                        if (!ExceptionTestCase.ExpectedExceptionType.IsAssignableFrom(recorder.Captured.GetType()))
                        {
                            Aggregator
                              .Add(
                                new XunitException(
                                    $"Expected exception of type '{ExceptionTestCase.ExpectedExceptionType.FullName}', but got '{recorder.Captured.GetType().FullName}'",
                                    recorder.Captured
                              )
                            );
                        }
                    }
                    if (ExceptionTestCase.ExpectedMessageSubstring != null)
                    {
                        if (recorder.Captured.Message == null)
                        {
                            Aggregator
                               .Add(
                                 new XunitException(
                                     $"Expected exception message to contain '{ExceptionTestCase.ExpectedMessageSubstring}', but the message was null.",
                                     recorder.Captured
                               )
                             );
                        }
                        else if (!recorder.Captured.Message.Contains(ExceptionTestCase.ExpectedMessageSubstring))
                        {
                            Aggregator
                              .Add(
                                new XunitException(
                                    $"Expected exception message to contain '{ExceptionTestCase.ExpectedMessageSubstring}', but got '{recorder.Captured.Message}'",
                                    recorder.Captured
                              )
                            );
                        }
                    }
                }

                // Return the elapsed time of the test method execution
                return (decimal)StopwatchToMeasureExecutionTime.Elapsed.TotalSeconds;
            });
        }
    }
}
