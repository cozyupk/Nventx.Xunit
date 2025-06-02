using NventX.xProof.BaseProofLibrary;
using Xunit.Abstractions;

namespace NventX.Xunit.SupportingBaseProofLibrary
{
    internal class FailLateFactFactTestCaseDiscoverer : FactTestCaseDiscoverer<FailLateProof>
    {
        public FailLateFactFactTestCaseDiscoverer(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
            // This discoverer is used to find test cases that use the FailLateFact attribute.
            // It inherits from FactTestCaseDiscoverer with FailLateProof as the proof type.
        }
    }

}