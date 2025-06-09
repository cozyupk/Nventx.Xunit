using System;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using NventX.xProof.BaseProofLibrary.Proofs;
using NventX.xProof.Utils;
using Xunit;

namespace NventX.xProof.SupportingXunit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited =true)]
    public abstract class ProofAttributeBase : FactAttribute
    {
        public abstract ProofInvocationKind ProofInvocationKind { get; }
        public virtual Type DefaultTestProofType { get; } = typeof(XProof);

        public virtual Type DefaultSerializableTestProofFactoryType { get; } = typeof(SerializableTestProofFactory<>);
        public Type TestProofType { get; set; }
        public Type SerializableTestProofFactoryType { get; set; }
        public ProofAttributeBase(Type? testProofType = null, Type? serializableTestProofFactoryType = null)
        {
            TestProofType = testProofType ?? DefaultTestProofType;
            SerializableTestProofFactoryType = serializableTestProofFactoryType ?? DefaultSerializableTestProofFactoryType;
        }
    }
}
