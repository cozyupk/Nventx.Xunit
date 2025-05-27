using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.ExceptionTesting
{
    /// <summary>
    /// Represents a test case that expects an exception to be thrown during its execution.
    /// </summary>
    [Serializable]
    internal class ExceptionTestCase : XunitTestCase, IExceptionTestCase
    {
        /// <summary>
        /// Gets or sets the type of the expected exception.
        /// </summary>
        public Type? ExpectedExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the expected message of the exception.
        /// </summary>
        public string? ExpectedMessageSubstring { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTestCase"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is used by the de-serializer and should not be called directly.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", true)]
        public ExceptionTestCase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTestCase"/> class with the specified parameters.
        /// </summary>
        /// <remarks>
        /// The unique paramsters are the expected exception type and message, which are used to verify that the test method throws the expected exception.
        /// Other parameters are inherited from the base class <see cref="XunitTestCase"/>.
        /// </remarks>
        public ExceptionTestCase(
            IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay,
            ITestMethod testMethod, object[]? testMethodArguments = null,
            Type? expectedExceptionType = null, string? expectedMessageSubstring = null
#pragma warning disable CS0618
        ) : base(diagnosticMessageSink, defaultMethodDisplay, testMethod, testMethodArguments)
#pragma warning restore CS0618
        {
            // Set properties for expected exception type and message
            ExpectedExceptionType = expectedExceptionType;
            ExpectedMessageSubstring = expectedMessageSubstring;
        }

        /// <summary>
        /// Runs the test case asynchronously, executing the test method and verifying that the expected exception is thrown.
        /// </summary>
        /// <remarks>
        /// All parameters are need to be passed to the <see cref="ExceptionTestCaseRunner"/> for execution,
        /// and the base class is not holding.
        /// </remarks>
        public override Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            // Create a new instance of the ExceptionTestCaseRunner with the current test case and parameters
            var runner = new ExceptionTestCaseRunner(
                  this, DisplayName, messageBus, constructorArguments,
                  TestMethodArguments, SkipReason, aggregator, cancellationTokenSource
                );

            // Run the test case asynchronously and return the result
            return runner.RunAsync();
        }

        /// <summary>
        /// Serializes the test case, including the expected exception type and message.
        /// </summary>
        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            data.AddValue(nameof(ExpectedExceptionType), ExpectedExceptionType, typeof(Type));
            data.AddValue(nameof(ExpectedMessageSubstring), ExpectedMessageSubstring);
        }

        /// <summary>
        /// Deserializes the test case, restoring the expected exception type and message.
        /// </summary>
        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);
            ExpectedExceptionType = data.GetValue<Type>(nameof(ExpectedExceptionType));
            ExpectedMessageSubstring = data.GetValue<string>(nameof(ExpectedMessageSubstring));
        }
    }
}
