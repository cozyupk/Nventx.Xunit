using System;
using Xproof.Abstractions.TestProofForTestRunner;
using Xunit.Sdk;

namespace Xproof.SupportingXunit.TypeBasedProofDiscoverer
{
    [XunitTestCaseDiscoverer(
        "Xproof.SupportingXunit.TypeBasedProofDiscoverer.ProofFactTestCaseDiscoverer",
        "Xproof.SupportingXunitV2.TypeBasedProofDiscoverer"
    )]
    public class ProofFactAttribute : ProofAttributeBase
    {
        public override ProofInvocationKind ProofInvocationKind => ProofInvocationKind.SingleCase;

        public ProofFactAttribute(
            Type? testProofType = null,
            Type? serializableTestProofFactoryType = null) : base(testProofType, serializableTestProofFactoryType)
        {
            // No additional initialization needed for ProofFactAttribute
        }
    }
}
