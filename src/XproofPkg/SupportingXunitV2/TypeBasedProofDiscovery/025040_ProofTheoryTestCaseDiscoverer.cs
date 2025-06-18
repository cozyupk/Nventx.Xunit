using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xproof.SupportingXunit.TypeBasedProofDiscoverer
{
    internal class ProofTheoryTestCaseDiscoverer : TheoryDiscoverer
    {
        internal IProofTestCaseDiscovererCore Core { get; } = new ProofTestCaseDiscovererCore();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofTheoryTestCaseDiscoverer"/> class.
        /// </summary>
        public ProofTheoryTestCaseDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            // Set the diagnostic message sink for the core discoverer
            Core.DiagnosticMessageSink = diagnosticMessageSink;
        }

        /// <summary>
        /// Creates test cases for a data row in a theory test method, which is a specific set of data that the theory can be executed with.
        /// </summary>
        protected override IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute, object[] dataRow)
        {
            // Discover test cases using the core functionality
            return Core.Discover(
                discoveryOptions,
                testMethod,
                theoryAttribute,
                dataRow
            );
        }
    }
}
