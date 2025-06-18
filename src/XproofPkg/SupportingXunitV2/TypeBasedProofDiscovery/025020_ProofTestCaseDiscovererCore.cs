using System;
using System.Collections.Generic;
using System.Linq;
using Xproof.Abstractions.TestProofForTestRunner;
using Xproof.Abstractions.Utils;
using Xproof.SupportingXunit.AdapterForTestRunner;
using Xproof.Utils.SerializableFactory;
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
        /// A が IInvokableProof&lt;TLabelAxes&gt; または（非ジェネリックの）IInvokableProof を
        /// 実装している前提で、TLabelAxes の実際の <see cref="Type"/> を返す。
        /// 非ジェネリックのみの場合は <c>null</c> を返す。
        /// </summary>
        /*
        private static Type? GetLabelAxesType(Type candidate)
        {
            if (candidate is null)
                throw new ArgumentNullException(nameof(candidate));

            var genericDef = typeof(IInvokableProof<>);
            var nonGenericIface = typeof(IInvokableProof);      // ★ 追加

            // ① IInvokableProof<T> を全部列挙
            var genericArgs = candidate
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericDef)
                .Select(i => i.GetGenericArguments()[0])
                .ToArray();

            // ② 見つかった？
            if (genericArgs.Length > 0)
            {
                // TODO: 複数ある場合の選択戦略をここで決める
                return genericArgs.First();
            }

            // ③ 非ジェネリック IInvokableProof を持っているかチェック
            if (candidate.GetInterfaces().Any(i => i == nonGenericIface))
            {
                // 仕様：非ジェネリックなら null を返す
                return null;
            }

            // ④ どちらも無い → 例外
            var ifaceList = string.Join(", ",
                candidate.GetInterfaces().Select(t => t.FullName ?? t.Name));

            throw new InvalidOperationException(
                $"{candidate.FullName} does not implement IInvokableProof interfaces: {ifaceList}");
        }
        */

        /// <summary>
        /// Creates an instance of a serializable test proof factory based on the given factory type and test proof type.
        /// </summary>
        private static object CreateSerializableTestProofFactory(
            Type? serializableTestProofFactoryType,
            Type testProofType)
        {
            // Assume types are pre-validated by the attribute. We resolve only the necessary type closures here.

            /*
            // Determine the label axes type (null means non-generic IInvokableProof)
            var labelAxesType = GetLabelAxesType(testProofType);

            // Select default factory type if not specified
            if (serializableTestProofFactoryType == null)
            {
                serializableTestProofFactoryType = labelAxesType == null
                    ? typeof(SerializableTestProofFactory<>)
                    : typeof(SerializableTestProofFactory<,>);
            }
            */

            serializableTestProofFactoryType ??= typeof(SerializableTestProofFactory<>);

            // If the provided factory type is already closed, just create it directly
            if (!serializableTestProofFactoryType.IsGenericTypeDefinition)
            {
                return Activator.CreateInstance(serializableTestProofFactoryType)
                    ?? throw new InvalidOperationException(
                        $"Failed to create an instance of the factory type '{serializableTestProofFactoryType.FullName}'.");
            }

            // If the factory is open generic, close it using appropriate arity
            Type closedFactoryType = serializableTestProofFactoryType.MakeGenericType(testProofType);

            return Activator.CreateInstance(closedFactoryType)
                ?? throw new InvalidOperationException(
                    $"Failed to create an instance of the closed factory type '{closedFactoryType.FullName}'.");
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
                var proofType = attributeInfo.GetNamedArgument<Type>("TestProofType")
                                    ?? throw new InvalidOperationException("TestProofType is not specified in the attribute.");

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
                var keyForSerializableTestProofFactory = "SerializableTestProofFactory";
                var testProofFactory = CreateSerializableTestProofFactory(
                    attributeInfo.GetNamedArgument<Type>(keyForSerializableTestProofFactory),
                    proofType
                ) as ISerializableTestProofFactoryBase
                        ?? throw new InvalidOperationException(
                            $"Failed to create an instance of {nameof(ISerializableTestProofFactoryBase)}."
                            + $"Please check that the {keyForSerializableTestProofFactory} is correct and implements ISerializableTestProofFactory."
                        );

                // Get the method display options from the discovery options
                var testMethodDisplay = discoveryOptions.MethodDisplayOrDefault();

                // Set the parameters for the test proof factory using the provided attribute information
                testProofFactory.SetParameter(
                    new XunitAttributeInfoWrapper(attributeInfo)
                );

                // Create a new test case for the proof with the provided parameters
                var testCaseGenericType = typeof(ProofTestCase<,>);

                // The proof type is known at runtime
                Type factoryType = testProofFactory.GetType();

                // Make the test case type generic with the proof type and factory type
                var closedTestCaseType = testCaseGenericType.MakeGenericType(proofType, factoryType);

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
