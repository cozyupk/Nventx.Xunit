using System;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Sdk;

namespace NventX.xProof.ForXunit.TypeBasedProofDiscoveryCore
{
    [XunitTestCaseDiscoverer(
        "NventX.xProof.Xunit.TypeBasedProofDiscoveryCore.ProofFactTestCaseDiscoverer",
        "NventX.xProof.ForXunitV2.TypeBasedProofDiscoveryCore"
    )]
    public class ProofFactAttribute : ProofAttributeBase
    {
        public override ProofInvocationKind ProofInvocationKind => ProofInvocationKind.SingleCase;

        public ProofFactAttribute(Type? testProofType = null, Type? serializableTestProofFactoryType = null)
            : base(testProofType, serializableTestProofFactoryType)
        {
            // No additional initialization needed for ProofFactAttribute
        }
    }
}
