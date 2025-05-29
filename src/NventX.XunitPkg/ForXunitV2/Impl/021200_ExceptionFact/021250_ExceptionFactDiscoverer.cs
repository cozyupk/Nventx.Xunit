using System;
using System.Collections.Generic;
using System.Linq;
using NventX.Xunit.Abstractions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.ExceptionFact
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
            // Allow exactly one parameter of type IExceptionRecorder
            var parameters = testMethod.Method.GetParameters();
            string? message = null;
            if (parameters.Count() != 1)
            {
                message = $"[ExceptionFact] method must have exactly one parameter of type IExceptionRecorder. Unexpected parameter count: {parameters.Count()}";
            } else
            {
                var parameterType = parameters.First().ParameterType.ToRuntimeType();
                if (!typeof(IExceptionRecoder).IsAssignableFrom(parameterType))
                {
                    message = $"[ExceptionFact] method must have exactly one parameter of type IExceptionRecorder. Unexpected parameter type: {parameterType}";
                }
            }

            if (message != null)
            {
                return new List<IXunitTestCase>() {
                    new ExecutionErrorTestCase(
                        DiagnosticMessageSink,
                        discoveryOptions.MethodDisplayOrDefault(),
                        discoveryOptions.MethodDisplayOptionsOrDefault(),
                        testMethod, message)
                };
            }

            // Retrieve the expected exception type and message substring from the attribute
            var expectedExceptionType = factAttribute.GetNamedArgument<Type>("ExpectedExceptionType");
            var expectedMessageSubstring = factAttribute.GetNamedArgument<string>("ExpectedMessageSubstring");
            var testMethodDisplay = discoveryOptions.MethodDisplayOrDefault();

            // Create a new instance of ExceptionExpectedTestCase with the provided parameters and return it as an IEnumerable<IXunitTestCase>
            return new List<IXunitTestCase>() {
                new ExceptionFactTestCase(DiagnosticMessageSink, testMethodDisplay, testMethod, null, expectedExceptionType, expectedMessageSubstring)
            };
        }
    }
}
