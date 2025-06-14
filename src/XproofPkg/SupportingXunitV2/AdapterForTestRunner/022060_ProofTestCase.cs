using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestRunner;
using Xproof.Abstractions.Utils;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xproof.SupportingXunit.AdapterForTestRunner
{
    /// <summary>
    /// Represents a test case that expects a proof to be verified during its execution.
    /// </summary>
    [Serializable]
    public class ProofTestCase<TTestProof, TSerializableTestProofFactory> : XunitTestCase, IProofTestCase
        where TTestProof : IInvokableProof
        where TSerializableTestProofFactory : ISerializableTestProofFactory<TTestProof>, new()
    {
        /// <summary>
        /// The factory used to create instances of the test proof for this test case.
        /// </summary>
        private TSerializableTestProofFactory TestProofFactory { get; set; }

        /// <summary>
        /// The kind of proof invocation that this test case represents, which defines how the proof is expected to be invoked during the test execution.
        /// </summary>
        public ProofInvocationKind ProofInvocationKind { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofTestCase"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is used by the de-serializer and should not be called directly.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", false)]
#pragma warning disable CS8618 // Not nullable field must contain a non-null value when the constructor exits. In this case, Deserializer will set it.
        public ProofTestCase() {
        }
#pragma warning restore CS8618 //  // Not nullable field must contain a non-null value when the constructor exits.  In this case, Deserializer will set it.

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofTestCase"/> class with the specified parameters.
        /// </summary>
        /// <remarks>
        /// The unique paramsters are the expected exception type and message, which are used to verify that the test method throws the expected exception.
        /// Other parameters are inherited from the base class <see cref="XunitTestCase"/>.
        /// </remarks>
        public ProofTestCase(
            IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay,
            ITestMethod testMethod, ProofInvocationKind proofInvocationKind,
            TSerializableTestProofFactory testProofFactory,
            object[]? testMethodArguments = null
#pragma warning disable CS0618 // The constructor is marked as obsolete, but we need to support it for being called by the de-serializer.
        ) : base(diagnosticMessageSink, defaultMethodDisplay, testMethod, testMethodArguments)
#pragma warning restore CS0618 // The constructor is marked as obsolete, but we need to support it for being called by the de-serializer.
        {
            // Validate the arguments and initialize the properties
            TestProofFactory = testProofFactory;
            ProofInvocationKind = Enum.IsDefined(typeof(ProofInvocationKind), proofInvocationKind)
                ? proofInvocationKind : ProofInvocationKind.Unknown;
        }

        /// <summary>
        /// Runs the test case asynchronously, executing the test method and verifying that the expected exception is thrown.
        /// </summary>
        /// <remarks>
        /// All parameters are need to be passed to the <see cref="ProofTestCaseRunner"/> for execution,
        /// and the base class is not holding.
        /// </remarks>
        public override Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            return aggregator.RunAsync(
                () => {
                    // Check if the test method arguments contains more than one argument, so we can update the first argument with the proof instance.
                    if (0 < TestMethodArguments.Length)
                    {
                        TestMethodArguments[0] = TestProofFactory.Create();
                    }

                    diagnosticMessageSink.OnMessage(
                        new TestCaseMessage(
                            this
                        )
                    );

                    // Create a new instance of the ExceptionTestCaseRunner with the current test case and parameters
                    var runner = new ProofTestCaseRunner(
                          this, DisplayName, messageBus, constructorArguments,
                          TestMethodArguments, SkipReason, aggregator, cancellationTokenSource
                        );

                    // Run the test case asynchronously and return the result
                    return runner.RunAsync();
                }
            );
        }

        /// <summary>
        /// Serializes the test case, including the test proof information.
        /// </summary>
        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            data.AddValue(nameof(ProofInvocationKind), ProofInvocationKind);
            data.AddValue(nameof(TestProofFactory), TestProofFactory?.SerializeToString());
        }

        /// <summary>
        /// Deserializes the test case, restoring the test proof information.
        /// </summary>
        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);
            TestProofFactory = new TSerializableTestProofFactory();
            ProofInvocationKind = data.GetValue<ProofInvocationKind>(nameof(ProofInvocationKind));
            TestProofFactory.DeserializeFromString(data.GetValue<string>(nameof(TestProofFactory)));
        }
    }
}
