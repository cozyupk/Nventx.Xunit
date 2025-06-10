using System;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Sdk;

namespace NventX.xProof.SupportingXunit.TypeBasedProofDiscoverer
{
    [XunitTestCaseDiscoverer(
        "NventX.xProof.SupportingXunit.TypeBasedProofDiscoverer.ProofTheoryTestCaseDiscoverer",
        "NventX.xProof.SupportingXunitV2.TypeBasedProofDiscoverer"
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
