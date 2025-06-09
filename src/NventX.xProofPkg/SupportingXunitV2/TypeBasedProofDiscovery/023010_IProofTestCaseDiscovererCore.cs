using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.xProof.SupportingXunit.TypeBasedProofDiscoverer
{
    /// <summary>
    /// Defines the core functionality for discovering test cases that are decorated with proof attributes.
    /// </summary>
    internal interface IProofTestCaseDiscovererCore
    {
        /// <summary>
        /// The message sink used to be passed to TestCase instances to output diagnostic messages during test execution.
        /// </summary>
        IMessageSink? DiagnosticMessageSink { get; set; }

        /// <summary>
        /// Discovers test cases based on the provided discovery options, test method, attribute information, and optional data row.
        /// </summary>
        IEnumerable<IXunitTestCase> Discover(
                    ITestFrameworkDiscoveryOptions discoveryOptions,
                    ITestMethod testMethod,
                    IAttributeInfo attributeInfo,
                    object[]? dataRow = null
        );
    }
}
