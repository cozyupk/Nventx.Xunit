using System.Reflection;
using System.Threading;
using System;
using Xunit.Sdk;
using Xunit;
using System.Linq;
using NventX.xProof.SupportingXunit.TypeBasedProofDiscoverer;
using Xunit.Abstractions;

namespace NventX.xProof.SupportingXunit.SelfHostedTestRuntime
{
    public class ProofTestRuntimeHost
    {
        private ITestFrameworkDiscoveryOptions DiscoveryOptions { get; }
        private TestMessageSink Reporter { get; }
        
        private IMessageBus MessageBus { get; }

        public ProofTestRuntimeHost(
            ITestFrameworkDiscoveryOptions? discoveryOptions = null,
            IMessageBus? messageBus = null,
            IRunnerLogger? logger = null
        )
        {
            DiscoveryOptions = discoveryOptions ?? TestFrameworkOptions.ForDiscovery();
            Reporter = new DefaultRunnerReporterWithTypesMessageHandler(
                logger ?? throw new ArgumentNullException(nameof(logger))
            );
            MessageBus = messageBus ?? new SpyBusWithSink(Reporter);
        }

        public void Execute(Assembly targetAssembly)
        {
            _ = targetAssembly ?? throw new ArgumentNullException(nameof(targetAssembly));

            var testCaseDiscoverer = new ProofFactTestCaseDiscoverer(
                Reporter
            );

            // var targetAssembly = Assembly.GetExecutingAssembly();
            var targetAttributes = new[] { typeof(ProofFactAttribute), typeof(ProofTheoryAttribute) };

            var testTypes = targetAssembly.GetTypes()
                .Where(t => t.IsClass && t.GetMethods()
                    .Any(m => m.GetCustomAttributes(inherit: false)
                        .Any(attr => targetAttributes.Contains(attr.GetType()))));

            foreach (var testType in testTypes)
            {
                var testInstance = Activator.CreateInstance(testType)!;

                var targetMethods = testType.GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(ProofFactAttribute), inherit: false).Length != 0);

                var testCollection = new TestCollection(new TestAssembly(new ReflectionAssemblyInfo(targetAssembly)), null, "Test collection for ProofFactTest");
                var testClass = new TestClass(testCollection, new ReflectionTypeInfo(testType));

                foreach (var targetMethod in targetMethods)
                {
                    var reflectionMethodInfo = new ReflectionMethodInfo(targetMethod);
                    Type[] proofAttributes = {
                        typeof(ProofFactAttribute),
                        typeof(ProofTheoryAttribute),
                    };

                    var attribute = proofAttributes
                        .SelectMany(attrType => reflectionMethodInfo.GetCustomAttributes(attrType))
                        .Cast<IAttributeInfo>()
                        .FirstOrDefault() ?? throw new InvalidOperationException("Missing ProofFact or ProofTheory attribute.");

                    var testCase = testCaseDiscoverer.Discover(
                        DiscoveryOptions,
                        new TestMethod(testClass, reflectionMethodInfo),
                        attribute
                    ).FirstOrDefault();

                    // var bus = new SpyBusWithSink(reporterSink);

                    var aggregator = new ExceptionAggregator();
                    var result = testCase!.RunAsync(
                        diagnosticMessageSink: Reporter,
                        messageBus: MessageBus,
                        constructorArguments: Array.Empty<object>(),
                        aggregator: aggregator,
                        cancellationTokenSource: new CancellationTokenSource()
                    ).GetAwaiter().GetResult();
                }
            }
        }
    }
}
