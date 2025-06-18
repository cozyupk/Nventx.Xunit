using System;
using System.Linq;
using Xproof.Abstractions.TestProofForTestRunner;
using Xproof.SupportingXunit.AssertBinding;
using Xunit;

namespace Xproof.SupportingXunit.TypeBasedProofDiscoverer
{
    /// <summary>
    /// Base attribute for xUnit test methods that use proof-based verification.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ProofAttributeBase : FactAttribute
    {
        // --------------------------------------------------------------------
        // Public properties
        // --------------------------------------------------------------------

        /// <summary>The kind of proof invocation used in this test method.</summary>
        public abstract ProofInvocationKind ProofInvocationKind { get; }

        /// <summary>The default test proof type to use if none is explicitly specified.</summary>
        public virtual Type DefaultTestProofType { get; } = typeof(AssertProof);

        /// <summary>The actual test proof type to use for this test method.</summary>
        public Type TestProofType { get; }

        /// <summary>
        /// Generic factory definition for creating serializable test proof instances.
        /// - If omitted, the test discovery process will select an appropriate one automatically.
        /// - If specified, it must be an open generic type such as SerializableTestProofFactory&lt;&gt; or &lt;,&gt;.
        /// </summary>
        public Type? SerializableTestProofFactory { get; }

        // --------------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------------

        protected ProofAttributeBase(
            Type? testProofType = null,
            Type? serializableTestProofFactory = null)
        {
            TestProofType = testProofType ?? DefaultTestProofType;
            SerializableTestProofFactory = serializableTestProofFactory;

            ValidateTypes();
        }

        // --------------------------------------------------------------------
        // Internal validation logic
        // --------------------------------------------------------------------

        private void ValidateTypes()
        {
            // --- (1) Ensure that TestProofType implements either IInvokableProof or IInvokableProof<T>
            bool implementsNonGeneric = typeof(IInvokableProof).IsAssignableFrom(TestProofType);
            bool implementsGeneric = TestProofType
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInvokableProof<>));

            if (!implementsNonGeneric && !implementsGeneric)
            {
                throw new ArgumentException(
                    $"TestProofType '{TestProofType.FullName}' must implement either IInvokableProof or IInvokableProof<TLabelAxes>.\n" +
                    $"[Hint] Please implement one of these interfaces on your test proof class.");
            }

            // --- (2) If a factory is specified, ensure its arity matches the proof type
            if (SerializableTestProofFactory is null) return;
            if (!SerializableTestProofFactory.IsGenericType) return;

            int factoryArity = SerializableTestProofFactory.GetGenericArguments().Length;
            int expectedArity = implementsGeneric ? 2 : 1;

            if (factoryArity != expectedArity)
            {
                string hint = implementsGeneric
                    ? "SerializableTestProofFactory<TTestProof, TLabelAxes>"
                    : "SerializableTestProofFactory<TTestProof>";

                throw new ArgumentException(
                    $"Mismatch between factory and proof type:\n" +
                    $"- Factory '{SerializableTestProofFactory.FullName}' has {factoryArity} generic parameter(s).\n" +
                    $"- Proof type '{TestProofType.FullName}' requires a factory with {expectedArity}.\n" +
                    $"[Hint] Make sure your factory is defined as {hint}.");
            }
        }
    }
}
