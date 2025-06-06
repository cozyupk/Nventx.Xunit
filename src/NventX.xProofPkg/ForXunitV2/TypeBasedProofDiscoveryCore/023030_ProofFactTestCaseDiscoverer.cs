using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.xProof.Xunit.TypeBasedProofDiscoveryCore
{
    /// <summary>
    /// A discoverer for test cases that expect a proof to be verified during their execution,
    /// </summary>
    internal class ProofFactTestCaseDiscoverer : IXunitTestCaseDiscoverer
    {
        internal IProofTestCaseDiscovererCore Core { get; } = new ProofTestCaseDiscovererCore();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofFactTestCaseDiscoverer"/> class.
        /// </summary>
        public ProofFactTestCaseDiscoverer(IMessageSink diagnosticMessageSink)
        {
            // Validate the diagnostic message sink and set it to the discoverer's core
            Core.DiagnosticMessageSink = diagnosticMessageSink
                                            ?? throw new ArgumentNullException(nameof(diagnosticMessageSink));
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
