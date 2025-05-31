using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.Generic
{
    /// <summary>
    /// Represents a test case that expects a proof to be verified during its execution.
    /// </summary>
    [Serializable]
    internal class TestCaseForProof<TTestProof> : XunitTestCase, ITestCaseForProof
        where TTestProof : ITestProof
    {
        public ISerializableTestProofFactory TestProofFactory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseForProof"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is used by the de-serializer and should not be called directly.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", false)]
#pragma warning disable CS8618 // Not nullable field must contain a non-null value when the constructor exits. In this case, Deserializer will set it.
        public TestCaseForProof() { }
#pragma warning restore CS8618 //  // Not nullable field must contain a non-null value when the constructor exits.  In this case, Deserializer will set it.

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseForProof"/> class with the specified parameters.
        /// </summary>
        /// <remarks>
        /// The unique paramsters are the expected exception type and message, which are used to verify that the test method throws the expected exception.
        /// Other parameters are inherited from the base class <see cref="XunitTestCase"/>.
        /// </remarks>
        public TestCaseForProof(
            IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay,
            ITestMethod testMethod, ISerializableTestProofFactory testProofFactory,
            object[]? testMethodArguments = null
#pragma warning disable CS0618 // The constructor is marked as obsolete, but we need to support it for being called by the de-serializer.
        ) : base(diagnosticMessageSink, defaultMethodDisplay, testMethod, testMethodArguments)
#pragma warning restore CS0618 // The constructor is marked as obsolete, but we need to support it for being called by the de-serializer.
        {
            TestProofFactory = testProofFactory ?? throw new ArgumentNullException(nameof(testProofFactory));
        }

        /// <summary>
        /// Runs the test case asynchronously, executing the test method and verifying that the expected exception is thrown.
        /// </summary>
        /// <remarks>
        /// All parameters are need to be passed to the <see cref="TestCaseRunnerForProof"/> for execution,
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
            var runner = new TestCaseRunnerForProof(
                  this, DisplayName, messageBus, constructorArguments,
                  TestMethodArguments, SkipReason, aggregator, cancellationTokenSource
                );

            // Run the test case asynchronously and return the result
            return runner.RunAsync();
        }

        /// <summary>
        /// Serializes the test case, including the test proof information.
        /// </summary>
        /// <param name="data"></param>
        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            data.AddValue(nameof(TestProofFactory), TestProofFactory?.SerializeToString());
        }

        /// <summary>
        /// Deserializes the test case, restoring the test proof information.
        /// </summary>
        /// <param name="data"></param>
        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);
            var factory = new SerializableFactory<ITestProof>();
            factory.DeserializeFromString(data.GetValue<string>(nameof(TestProofFactory)));
        }
    }
}
