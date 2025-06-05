using System;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Sdk;

namespace NventX.xProof.Xunit.SupportingBaseProofLibrary
{
    [XunitTestCaseDiscoverer(
        "NventX.xProof.Xunit.SupportingBaseProofLibrary.ProofFactAttributeDiscoverer",
        "NventX.xProof.ForXunitV2.SupportingBaseProofLibrary"
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
