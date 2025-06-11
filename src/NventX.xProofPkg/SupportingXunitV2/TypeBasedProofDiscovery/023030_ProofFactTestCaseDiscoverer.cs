using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.xProof.SupportingXunit.TypeBasedProofDiscoverer
{
    /// <summary>
    /// A discoverer for test cases that expect a proof to be verified during their execution,
    /// </summary>
    public class ProofFactTestCaseDiscoverer : IXunitTestCaseDiscoverer
    {
        internal IProofTestCaseDiscovererCore Core { get; } = new ProofTestCaseDiscovererCore();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofFactTestCaseDiscoverer"/> class.
        /// </summary>
        public ProofFactTestCaseDiscoverer(IMessageSink diagnosticMessageSink)
        {
            // Set the diagnostic message sink for the core discoverer
            Core.DiagnosticMessageSink = diagnosticMessageSink;
        }

        /// <summary>
        /// Discovers test cases that are decorated with the <see cref="ExceptionFactAttribute"/> attribute.
        /// </summary>
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions,
                                                    ITestMethod testMethod,
                                                    IAttributeInfo factAttribute)
        {
            // Discover test cases using the core functionality
            return Core.Discover(
                discoveryOptions,
                testMethod,
                factAttribute
            );
        }
    }
}
