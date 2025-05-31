using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit.Generic
{
    public enum TestCasePropositionType
    {
        Fact,
        Theory,
    }
    internal class XunitAttributeInfoWrapper : INamedArgumentResolver
    {
        private readonly IAttributeInfo _attributeInfo;

        public XunitAttributeInfoWrapper(IAttributeInfo attributeInfo)
        {
            _attributeInfo = attributeInfo ?? throw new ArgumentNullException(nameof(attributeInfo));
        }

        public T Resolve<T>(string name)
            => _attributeInfo.GetNamedArgument<T>(name);
    }

    public class SerializableTestProofFactory : SerializableFactory<ITestProof>, ISerializableTestProofFactory
    {
    }

    public class TestCaseForProofDiscoverer<TTestProof> : TestCaseForProofDiscoverer<TTestProof, SerializableTestProofFactory>
    where TTestProof : ITestProof
    {
        public TestCaseForProofDiscoverer(
            TestCasePropositionType testCasePropositionType,
            IMessageSink diagnosticMessageSink
        ) : base(testCasePropositionType, diagnosticMessageSink)
        {
        }
    }

    public class TestCaseForProofDiscoverer<TTestProof, TSerializableTestProofFactory> : IXunitTestCaseDiscoverer
        where TTestProof : ITestProof
        where TSerializableTestProofFactory : ISerializableTestProofFactory, new()
    {
        private TestCasePropositionType TestCasePropositionType { get; }

        public TestCaseForProofDiscoverer(TestCasePropositionType testCasePropositionType, IMessageSink diagnosticMessageSink)
        {
            // Hold the test proposition type that defines the expected behavior of the test cases
            TestCasePropositionType = testCasePropositionType;

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
            string methodName = testMethod.Method.Name;

            var testProofFactory = new TSerializableTestProofFactory();
            var proofType = typeof(TTestProof);

            var parameters = testMethod.Method.GetParameters();
            List<string> messages = new();
            if (TestCasePropositionType == TestCasePropositionType.Fact)
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

            if (0 < parameters.Count())
            {
                var parameterType = parameters.First().ParameterType.ToRuntimeType();
                if (proofType.IsAssignableFrom(parameterType))
                {
                    messages.Add(
                        $"[{methodName}] The first parameter of the method should be {proofType.FullName}. Unexpected parameter type: {parameterType}"
                    );
                }
            }

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
            var testMethodDisplay = discoveryOptions.MethodDisplayOrDefault();

            testProofFactory.SetParameter(
                new XunitAttributeInfoWrapper(factAttribute)
            );

            return new List<IXunitTestCase>()
            {
                new TestCaseForProof<TTestProof>(
                        DiagnosticMessageSink,
                        testMethodDisplay,
                        testMethod,
                        testProofFactory,
                        null
                    )
            };
        }
    }
}
