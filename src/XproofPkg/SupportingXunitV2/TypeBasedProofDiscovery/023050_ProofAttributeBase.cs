using System;
using Xproof.Abstractions.TestProofForTestRunner;
using Xproof.BaseProofLibrary.Proofs;
using Xproof.Utils;
using Xunit;

namespace Xproof.SupportingXunit.TypeBasedProofDiscoverer
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited =true)]
    public abstract class ProofAttributeBase : FactAttribute
    {
        public abstract ProofInvocationKind ProofInvocationKind { get; }
        public virtual Type DefaultTestProofType { get; } = typeof(Xproof.BaseProofLibrary.Proofs.Xproof);

        public virtual Type DefaultSerializableTestProofFactoryType { get; } = typeof(SerializableTestProofFactory<,>);
        public Type TestProofType { get; set; }
        public Type SerializableTestProofFactoryType { get; set; }
        public ProofAttributeBase(Type? testProofType = null, Type? serializableTestProofFactoryType = null)
        {
            TestProofType = testProofType ?? DefaultTestProofType;
            SerializableTestProofFactoryType = serializableTestProofFactoryType ?? DefaultSerializableTestProofFactoryType;
        }
    }
}
