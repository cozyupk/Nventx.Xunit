using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.ExceptionTesting
{
    /// <summary>
    /// Discovers test cases that are decorated with the <see cref="ExceptionFactAttribute"/> attribute.
    /// </summary>
    internal class ExceptionFactDiscoverer : IXunitTestCaseDiscoverer
    {
        public ExceptionFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            // Initialize the diagnostic message sink to be used for outputting messages during test execution
            DiagnosticMessageSink = diagnosticMessageSink ?? throw new ArgumentNullException(nameof(diagnosticMessageSink));
        }

        /// <summary>
        /// The message sink used to be passed to TestCase instances to output diagnostic messages during test execution.
        /// </summary>
        private IMessageSink DiagnosticMessageSink { get; }

        /// <summary>
        /// Discovers test cases that are decorated with the <see cref="ExceptionFactAttribute"/> attribute.
        /// </summary>
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions,
                                                    ITestMethod testMethod,
                                                    IAttributeInfo factAttribute)
        {
            var expectedExceptionType = factAttribute.GetNamedArgument<Type>("ExpectedExceptionType");
            var expectedMessageSubstring = factAttribute.GetNamedArgument<string>("ExpectedMessageSubstring");
            var testMethodDisplay = discoveryOptions.MethodDisplayOrDefault();

            // Create a new instance of ExceptionExpectedTestCase with the provided parameters and return it as an IEnumerable<IXunitTestCase>
            return new List<IXunitTestCase>() {
                new ExceptionTestCase(DiagnosticMessageSink, testMethodDisplay, testMethod, null, expectedExceptionType, expectedMessageSubstring)
            };
        }
    }
}
