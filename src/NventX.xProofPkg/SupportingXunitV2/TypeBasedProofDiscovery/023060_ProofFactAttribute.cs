using System;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Sdk;

namespace NventX.xProof.SupportingXunit.TypeBasedProofDiscoverer
{
    [XunitTestCaseDiscoverer(
        "NventX.xProof.SupportingXunit.TypeBasedProofDiscoverer.ProofFactTestCaseDiscoverer",
        "NventX.xProof.SupportingXunitV2.TypeBasedProofDiscoverer"
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
