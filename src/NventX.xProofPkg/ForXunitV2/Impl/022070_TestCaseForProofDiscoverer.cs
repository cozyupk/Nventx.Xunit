using System;
using System.Collections.Generic;
using System.Linq;
using NventX.xProof.Abstractions;
using NventX.xProof.Utils;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit
{
    /// <summary>
    /// A wrapper for <see cref="IAttributeInfo"/> that implements <see cref="INamedArgumentResolver"/>.
    /// </summary>
    internal class XunitAttributeInfoWrapper : INamedArgumentResolver
    {
        /// <summary>
        /// The underlying attribute information that this wrapper encapsulates.
        /// </summary>
        private IAttributeInfo AttributeInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitAttributeInfoWrapper"/> class with the specified attribute information.
        /// </summary>
        public XunitAttributeInfoWrapper(IAttributeInfo attributeInfo)
        {
            // Validate the argument
            _ = attributeInfo ?? throw new ArgumentNullException(nameof(attributeInfo));

            // Initialize the wrapper with the provided attribute info
            AttributeInfo = attributeInfo ?? throw new ArgumentNullException(nameof(attributeInfo));
        }

        /// <summary>
        /// Resolves a named argument of type T from the attribute information.
        /// </summary>
        public T Resolve<T>(string name)
            => AttributeInfo.GetNamedArgument<T>(name);
    }



    /// <summary>
    /// A discoverer for test cases that expect a proof to be verified during their execution.
    /// </summary>
    public class TestCaseForProofDiscoverer<TTestProof> : TestCaseForProofDiscoverer<TTestProof, SerializableTestProofFactory<TTestProof>>
        where TTestProof : ITestProof
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseForProofDiscoverer{TTestProof}"/> class with the specified test case proposition type and diagnostic message sink.
        /// </summary>
        public TestCaseForProofDiscoverer(
            ProofInvocationKind proofInvocationKind,
            IMessageSink diagnosticMessageSink
        ) : base(proofInvocationKind, diagnosticMessageSink)
        {
        }
    }

    /// <summary>
    /// A discoverer for test cases that expect a proof to be verified during their execution,
    /// </summary>
    public class TestCaseForProofDiscoverer<TTestProof, TSerializableTestProofFactory> : IXunitTestCaseDiscoverer
        where TTestProof : ITestProof
        where TSerializableTestProofFactory : ISerializableTestProofFactory<TTestProof>, new()
    {
        /// <summary>
        /// The type of test case proposition that defines the expected behavior of the test cases.
        /// </summary>
        private ProofInvocationKind ProofInvocationKind { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseForProofDiscoverer{TTestProof, TSerializableTestProofFactory}"/> class.
        /// </summary>
        public TestCaseForProofDiscoverer(ProofInvocationKind proofInvocationKind, IMessageSink diagnosticMessageSink)
        {
            // Validate the arguments
            _ = diagnosticMessageSink ?? throw new ArgumentNullException(nameof(diagnosticMessageSink));
            _ = proofInvocationKind != ProofInvocationKind.SingleCase &&
                proofInvocationKind != ProofInvocationKind.Parameterized &&
                proofInvocationKind != ProofInvocationKind.Unknown
                ? throw new ArgumentOutOfRangeException(nameof(proofInvocationKind), "Invalid proof invocation kind.")
                : proofInvocationKind;

            // Hold the test proposition type that defines the expected behavior of the test cases
            ProofInvocationKind = proofInvocationKind;

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
            // Validate the afguments
            _ = discoveryOptions ?? throw new ArgumentNullException(nameof(discoveryOptions));
            _ = testMethod ?? throw new ArgumentNullException(nameof(testMethod));
            _ = factAttribute ?? throw new ArgumentNullException(nameof(factAttribute));

            // Get the method name from the test method
            string methodName = testMethod?.Method.Name ?? throw new ArgumentNullException(nameof(testMethod));

            // Ensure that the test method is not null and has a valid name
            var proofType = typeof(TTestProof);

            var parameters = testMethod.Method.GetParameters();
            List<string> messages = new();

            // Check if the method is for a Fact or Theory proposition type
            if (ProofInvocationKind == ProofInvocationKind.SingleCase)
            {
                if (parameters.Count() != 1)
                {
                    messages.Add(
                        $"[{methodName}] method must have exactly one parameter of {proofType}. Unexpected parameter count: {parameters.Count()}"
                    );
                }
            } 
            else
            {
                if (parameters.Count() < 1)
                {
                    messages.Add(
                        $"[{methodName}] method must have exactly one parameter of {proofType}. Unexpected parameter count: {parameters.Count()}"
                    );
                }
            }

            // Check if the first parameter is of the expected proof type
            if (0 < parameters.Count())
            {
                var parameterType = parameters.First().ParameterType.ToRuntimeType();
                if (
                    /* !parameterType.IsAssignableFrom(proofType) */
                    /* parameters.First() is not TTestProof) */
                    !parameterType.IsAssignableFrom(proofType) || !proofType.IsAssignableFrom(parameterType))
                {
                    messages.Add(
                        $"[{methodName}] The first parameter of the method should be {proofType.FullName}. Unexpected parameter type: {parameterType}"
                    );
                }
            }

            // If there are any messages, return them as ExecutionErrorTestCase instances
            if (messages.Any())
            {
                return messages.Select(
                    message => new ExecutionErrorTestCase(
                        DiagnosticMessageSink,
                        discoveryOptions.MethodDisplayOrDefault(),
                        discoveryOptions.MethodDisplayOptionsOrDefault(),
                        testMethod, message
                    )
                );
            }

            // Create a new instance of the test proof factory that will be used to create test cases

            // throw new InvalidOperationException($"{typeof(TSerializableTestProofFactory)}");
            //  → TSerializableTestProofFactory が NventX.Xunit.Generic.SerializableTestProofFactory`1[NventX.Xunit.E2ETests.ForXunitV2.Mock.NonExceptionProof]
            //     であることを確認済み
            var testProofFactory = new TSerializableTestProofFactory();

            // Get the method display options from the discovery options
            var testMethodDisplay = discoveryOptions.MethodDisplayOrDefault();

            // Set the parameters for the test proof factory using the provided attribute information
            testProofFactory.SetParameter(
                new XunitAttributeInfoWrapper(factAttribute)
            );

            // Create a new test case for the proof with the provided parameters
            return new List<IXunitTestCase>()
            {
                new TestCaseForProof<TTestProof, TSerializableTestProofFactory>(
                        DiagnosticMessageSink,
                        testMethodDisplay,
                        testMethod,
                        ProofInvocationKind,
                        testProofFactory,
                        null
                    )
            };
        }
    }
}
