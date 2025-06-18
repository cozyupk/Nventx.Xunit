using System;
using Xproof.Abstractions.TestProofForTestRunner;
using Xunit.Sdk;

namespace Xproof.SupportingXunit.TypeBasedProofDiscoverer
{
    [XunitTestCaseDiscoverer(
        "Xproof.SupportingXunit.TypeBasedProofDiscoverer.ProofTheoryTestCaseDiscoverer",
        "Xproof.SupportingXunitV2.TypeBasedProofDiscoverer"
    )]
    public class ProofTheoryAttribute : ProofAttributeBase
    {
        public override ProofInvocationKind ProofInvocationKind => ProofInvocationKind.Parameterized;

        public ProofTheoryAttribute(
            Type? testProofType = null,
            Type? serializableTestProofFactoryType = null) : base(testProofType, serializableTestProofFactoryType)
        {
            // No additional initialization needed for ProofFactAttribute
        }
    }
}
