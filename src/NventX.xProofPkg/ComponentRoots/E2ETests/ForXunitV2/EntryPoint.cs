using System.Reflection;
using NventX.xProof.ForXunit.TypeBasedProofDiscoveryCore;
using Xunit.Abstractions;
using Xunit.Sdk;
using Xunit;

namespace NventX.xProof.ForXunit.E2ETests.ForXunitV2
{
    internal class EntryPoint
    {
        public static void Main(string[] _)
        {
            var reporterSink = new DefaultRunnerReporterWithTypesMessageHandler(
                new WriteLineForConsoleAndDebugRunnerLogger()
            );

            var testCaseDiscoverer = new ProofFactTestCaseDiscoverer(
                reporterSink
            );

            var targetAssembly = Assembly.GetExecutingAssembly();
            var testTypes = targetAssembly.GetTypes()
                .Where(t => t.IsClass && t.GetMethods()
                    .Any(m => m.GetCustomAttributes(typeof(ProofFactAttribute), inherit: false).Length != 0));

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
                    var testCase = testCaseDiscoverer.Discover(
                        new MinimalDiscoveryOptions(),
                        new TestMethod(testClass, reflectionMethodInfo),
                        reflectionMethodInfo
                            .GetCustomAttributes(typeof(ProofFactAttribute))
                            .FirstOrDefault()!
                    ).FirstOrDefault();

                    var bus = new SpyBusWithSink(reporterSink);

                    var aggregator = new ExceptionAggregator();
                    var result = testCase!.RunAsync(
                        diagnosticMessageSink: reporterSink,
                        messageBus: bus,
                        constructorArguments: [],
                        aggregator: aggregator,
                        cancellationTokenSource: new CancellationTokenSource()
                    ).GetAwaiter().GetResult();
                }
            }
        }

        private class WriteLineForConsoleAndDebugRunnerLogger : IRunnerLogger
        {
            public object LockObject { get; } = new();

            private static void WriteLine(string message, ConsoleColor forgroundColor = default)
            {
                if (forgroundColor != default)
                {
                    Console.ForegroundColor = forgroundColor;
                }
                Console.WriteLine(message);
                System.Diagnostics.Debug.WriteLine(message);
                if (forgroundColor != default)
                {
                    Console.ResetColor();
                }
            }
            public void LogError(StackFrameInfo stackFrame, string message)
            {
                WriteLine($"[ERROR] {message}", ConsoleColor.Red);
            }

            public void LogImportantMessage(StackFrameInfo stackFrame, string message)
            {
                WriteLine(message, ConsoleColor.Green);
            }

            public void LogMessage(StackFrameInfo stackFrame, string message)
            {
                WriteLine(message, ConsoleColor.Gray);
            }

            public void LogRaw(string message)
            {
                WriteLine(message); // 色なしそのまま
            }

            public void LogWarning(StackFrameInfo stackFrame, string message)
            {
                WriteLine($"[WARNING] {message}", ConsoleColor.Yellow);
            }
        }

        private class MinimalDiscoveryOptions : ITestFrameworkDiscoveryOptions
        {
            public TValue GetValue<TValue>(string name)
            {
                switch (name)
                {
                    case "xunit.discovery.MethodDisplay":
                        if (typeof(TValue).IsAssignableFrom(typeof(string))) {
                            return (TValue)(object)"ClassAndMethod";
                        }
                        break;
                }
                throw new NotImplementedException(name);
            }

            public void SetValue<TValue>(string name, TValue value)
            {
                throw new NotImplementedException();
            }
        }

        private class SpyBusWithSink(TestMessageSink sink) : IMessageBus
        {
            private readonly TestMessageSink sink = sink;

            public bool QueueMessage(IMessageSinkMessage message)
            {
                // TestMessageSink に流す
                return sink.OnMessage(message);
            }

            public void Dispose() { }
        }
    }
}