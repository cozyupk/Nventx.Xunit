using System;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Sdk;

namespace NventX.xProof.Xunit.SupportingBaseProofLibrary
{
    [XunitTestCaseDiscoverer(
    "NventX.xProof.Xunit.SupportingBaseProofLibrary.ProofTheoryAttributeDiscoverer",
    "NventX.xProof.ForXunitV2.SupportingBaseProofLibrary"
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
