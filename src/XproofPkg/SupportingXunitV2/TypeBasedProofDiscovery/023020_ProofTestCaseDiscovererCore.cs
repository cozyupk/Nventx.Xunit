using System;
using System.Collections.Generic;
using System.Linq;
using Xproof.Abstractions.TestProofForTestRunner;
using Xproof.Abstractions.Utils;
using Xproof.SupportingXunit.AdapterForTestRunner;
using Xproof.Utils;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xproof.SupportingXunit.TypeBasedProofDiscoverer
{
    /// <summary>
    /// Provides core functionality for discovering test cases that are decorated with proof attributes.
    /// </summary>
    internal class ProofTestCaseDiscovererCore : IProofTestCaseDiscovererCore
    {
        /// <summary>
        /// The message sink used to be passed to TestCase instances to output diagnostic messages during test execution.
        /// </summary>
        public IMessageSink? DiagnosticMessageSink { get; set; }

        /// <summary>
        /// A が IInvokableProof&lt;TAxes&gt; を実装している前提で、
        /// その TAxes の実際の <see cref="Type"/> を返す。
        /// </summary>
        private static Type GetAxesType(Type candidate)
        {
            if (candidate is null)
                throw new ArgumentNullException(nameof(candidate));

            // ① candidate 自身が IInvokableProof<...> そのものか？
            if (candidate.IsGenericType &&
                candidate.GetGenericTypeDefinition() == typeof(IInvokableProof<>))
            {
                return candidate.GetGenericArguments()[0];
            }

            // ② 実装インタフェースをスキャン
            var iface = candidate
                .GetInterfaces()                              // <- 継承階層もぜんぶ含む
                .FirstOrDefault(i =>
                     i.IsGenericType &&
                     i.GetGenericTypeDefinition() == typeof(IInvokableProof<>));

            if (iface is null)
            {
                throw new InvalidOperationException(
                    $"{candidate.FullName} doesn't implement IInvokableProof<>.");
            }

            // 第一型引数 (= TAxes) を返す
            return iface.GetGenericArguments()[0];
        }

        /// <summary>
        /// Creates an instance of a serializable test p
        /// roof factory based on the provided type and test proof type.
        /// </summary>
        private static object CreateSerializableTestProofFactory(Type serializableTestProofFactoryType, Type testProofType)
        {
            if (!serializableTestProofFactoryType.IsGenericTypeDefinition)
            {
                // not a generic type definition, so we can create an instance directly
                return Activator.CreateInstance(serializableTestProofFactoryType)
                       ?? throw new InvalidOperationException($"Failed to create instance of {serializableTestProofFactoryType}");
            }
            var numGenericArgs = serializableTestProofFactoryType.GetGenericArguments().Length;
            if (numGenericArgs != 2)
            {

                throw new ArgumentException(nameof(
                    serializableTestProofFactoryType),
                    $"{serializableTestProofFactoryType} must have exactly two generic arguments, but found {numGenericArgs}."
                );
            }

            var closedType = serializableTestProofFactoryType.MakeGenericType(testProofType, GetAxesType(testProofType));
            return Activator.CreateInstance(closedType)
                   ?? throw new InvalidOperationException($"Failed to create instance of {closedType}");
        }

        /// <summary>
        /// Discovers test cases based on the provided discovery options, test method, attribute information, and optional data row.
        /// </summary>
        public IEnumerable<IXunitTestCase> Discover(
                    ITestFrameworkDiscoveryOptions discoveryOptions,
                    ITestMethod testMethod,
                    IAttributeInfo attributeInfo,
                    object?[]? dataRow = null
        ) {
            List<IXunitTestCase>? testCases = new();
            List<string> messages = new();

            try
            {
                // Validate the afguments
                _ = discoveryOptions ?? throw new ArgumentNullException(nameof(discoveryOptions));
                _ = testMethod ?? throw new ArgumentNullException(nameof(testMethod));
                _ = attributeInfo ?? throw new ArgumentNullException(nameof(attributeInfo));

                // Get the method name from the test method
                string methodName = testMethod.Method.Name;

                // Ensure that the test method is not null and has a valid name
                var proofType = attributeInfo.GetNamedArgument<Type>("TestProofType") ?? typeof(Xproof.BaseProofLibrary.Proofs.Xproof);

                var keyForProofInvocationKind = "ProofInvocationKind";
                var proofInvocationKind = attributeInfo.GetNamedArgument<ProofInvocationKind>(keyForProofInvocationKind);
                proofInvocationKind = Enum.IsDefined(typeof(ProofInvocationKind), proofInvocationKind)
                    ? proofInvocationKind
                    : throw new InvalidOperationException($"Invalid Attirbute Parameter: {keyForProofInvocationKind}");

                // Check the parameters of the test method to ensure they match the expected proof type and count
                var parameters = testMethod.Method.GetParameters();
                if (proofInvocationKind == ProofInvocationKind.SingleCase)
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
                    if (!parameterType.IsAssignableFrom(proofType)
                        /* parameters.First() is not TTestProof) */
                        /* !parameterType.IsAssignableFrom(proofType) || !proofType.IsAssignableFrom(parameterType) */)

                    {
                        messages.Add(
                            $"[{methodName}] The first parameter of the method should be {proofType.FullName}. Unexpected parameter type: {parameterType}"
                        );
                    }
                }

                // Create a new instance of the test proof factory that will be used to create test cases
                var keyForSerializableTestProofFactory = "SerializableTestProofFactoryType";
                var testProofFactory = CreateSerializableTestProofFactory(
                    attributeInfo.GetNamedArgument<Type>(keyForSerializableTestProofFactory),
                    proofType
                ) as ISerializableTestProofFactory
                        ?? throw new InvalidOperationException(
                            $"Failed to create an instance of {nameof(ISerializableTestProofFactory)}. Please check that the {keyForSerializableTestProofFactory} is correct and implements ISerializableTestProofFactory."
                        );

                // Get the method display options from the discovery options
                var testMethodDisplay = discoveryOptions.MethodDisplayOrDefault();

                // Set the parameters for the test proof factory using the provided attribute information
                testProofFactory.SetParameter(
                    new XunitAttributeInfoWrapper(attributeInfo)
                );

                // Create a new test case for the proof with the provided parameters
                var testCaseGenericType = typeof(ProofTestCase<,,>);

                // The proof type is known at runtime
                Type factoryType = typeof(SerializableTestProofFactory<,>).MakeGenericType(proofType, GetAxesType(proofType));

                // Make the test case type generic with the proof type and factory type
                var closedTestCaseType = testCaseGenericType.MakeGenericType(proofType, GetAxesType(proofType), factoryType);

                // Insert null value for the proof into the test method arguments at the front
                dataRow ??= Array.Empty<object>();
                // Register the proof type as the first element in the data row as a placeholder
                dataRow = new object?[] { $"[{proofType.Name}]" }.Concat(dataRow).ToArray();

                // Prepare the constructor arguments for the test case instance
                var constructorArgs = new object?[]
                {
                    DiagnosticMessageSink,
                    testMethodDisplay,
                    testMethod,
                    proofInvocationKind,
                    testProofFactory,
                    dataRow
                };

                // Create an instance of the test case using reflection
                var testCase = (IXunitTestCase?)Activator.CreateInstance(closedTestCaseType, constructorArgs)
                                    ?? throw new InvalidOperationException("Failed to create test case instance.");
                testCases.Add(testCase);
            }
            catch (Exception ex)
            {
                // If an exception occurs, add the error message to the messages list
                var message = $"Error while discovering test case for method {testMethod.Method.Name}:{Environment.NewLine}{ex}";
                messages.Add(message);
                DiagnosticMessageSink?.OnMessage(new DiagnosticMessage(message));
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

            // Check if the test case was created successfully
            if (testCases == null)
            {
                // If the test case is null, return an empty list
                return Enumerable.Empty<IXunitTestCase>();
            }

            // Return the discovered test cases
            return testCases.AsReadOnly();
        }
    }
}
