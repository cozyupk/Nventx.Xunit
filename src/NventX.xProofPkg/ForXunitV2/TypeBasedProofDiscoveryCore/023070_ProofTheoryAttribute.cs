using System;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Sdk;

namespace NventX.xProof.ForXunit.TypeBasedProofDiscoveryCore
{
    [XunitTestCaseDiscoverer(
    "NventX.xProof.Xunit.TypeBasedProofDiscoveryCore.ProofTheoryTestCaseDiscoverer",
    "NventX.xProof.ForXunitV2.TypeBasedProofDiscoveryCore"
)]
    public class ProofTheoryAttribute : ProofAttributeBase
    {
        public override ProofInvocationKind ProofInvocationKind => ProofInvocationKind.Parameterized;

        public ProofTheoryAttribute(Type? testProofType = null, Type? serializableTestProofFactoryType = null)
            : base(testProofType, serializableTestProofFactoryType)
        {
            // No additional initialization needed for ProofFactAttribute
        }
    }
}
