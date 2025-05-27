using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

// This code is adapted from the VsixTesting project.
// Original source: https://github.com/josetr/VsixTesting
// Licensed under the Apache License, Version 2.0.
// See: https://www.apache.org/licenses/LICENSE-2.0

namespace NventX.CollectionPkg.UnitTest
{
    [XunitTestCaseDiscoverer($"NventX.CollectionPkg.UnitTest.XunitTestsBase.{nameof(ExceptionFactDiscoverer)}", "NventX.CollectionPkg.UnitTest")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExceptionFact2Attribute(Type? expectedExceptionType = null, string? expectedMessage = null) : FactAttribute
    {
        public Type? ExpectedExceptionType { get; } = expectedExceptionType;
        public string? ExpectedMessage { get; } = expectedMessage;
    }

    public class ExceptionTestCase : XunitTestCase, ITest
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", true)]
        public ExceptionTestCase() { }

#pragma warning disable CS0618
        public ExceptionTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay testMethodDisplay, ITestMethod testMethod, Type? expectedExceptionType, string? expectedMessage)
            : base(diagnosticMessageSink, testMethodDisplay, testMethod)
        {
            ExpectedExceptionType = expectedExceptionType;
            ExpectedMessage = expectedMessage;
        }
#pragma warning restore CS0618
        public Type? ExpectedExceptionType { get; set; }
        public string? ExpectedMessage { get; set; }

        public ITestCase TestCase => throw new NotImplementedException();

        public override async Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            var runner = new ExceptionExpectedTestRunner(
                new XunitTest(this, DisplayName),
                messageBus,
                TestMethod.TestClass.Class.ToRuntimeType(),
                constructorArguments,
                TestMethod.Method.ToRuntimeMethod(),
                TestMethodArguments,
                SkipReason,
                [],
                aggregator,
                cancellationTokenSource,
                ExpectedExceptionType,
                ExpectedMessage
            );

            return await runner.RunAsync();
        }
        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            data.AddValue(nameof(ExpectedExceptionType), ExpectedExceptionType, typeof(Type));
            data.AddValue(nameof(ExpectedMessage), ExpectedMessage);
        }

        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);
            ExpectedExceptionType = data.GetValue<Type>(nameof(ExpectedExceptionType));
            ExpectedMessage = data.GetValue<string>(nameof(ExpectedMessage));
        }
    }

    public class ExceptionExpectedTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments,
                                   MethodInfo testMethod, object[] testMethodArguments, string skipReason,
                                   IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator,
                                   CancellationTokenSource cancellationTokenSource,
                                   Type? expectedExceptionType, string? expectedMessage
        ) : XunitTestRunner(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments,
           skipReason, beforeAfterAttributes, aggregator, cancellationTokenSource)
    {
        private readonly Type? _expectedExceptionType = expectedExceptionType;
        private readonly string? _expectedMessage = expectedMessage;

        protected override async Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
        {
            Exception? thrown = null;
            MethodInfo method = TestMethod;

            // 判断: 戻り値が Task
            bool returnsTask = typeof(Task).IsAssignableFrom(method.ReturnType);

            try
            {
                if (method.IsStatic)
                {
                    if (returnsTask)
                    {
                        var func = (Func<Task>)Delegate.CreateDelegate(typeof(Func<Task>), method);
                        await func();
                    }
                    else
                    {
                        var action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                        action();
                    }
                }
                else
                {
                    if (returnsTask)
                    {
                        var func = (Func<Task>)Delegate.CreateDelegate(typeof(Func<Task>), TestCase, method);
                        await func();
                    }
                    else
                    {
                        var action = (Action)Delegate.CreateDelegate(typeof(Action), TestCase, method);
                        action();
                    }
                }
            }
            catch (Exception ex)
            {
                thrown = ex;
            }
            if (thrown is null)
            {
                // aggregator.Add(new Exception("Expected an exception, but none was thrown."));
            }
            else
            {
                if (_expectedExceptionType != null && !_expectedExceptionType.IsAssignableFrom(thrown.GetType()))
                {
                    // aggregator.Add(new Exception($"Expected exception of type {_expectedExceptionType.Name}, but got {thrown.GetType().Name}."));
                }
                if (_expectedMessage != null && !thrown.Message.Contains(_expectedMessage))
                {
                    // aggregator.Add(new Exception($"Expected message to contain '{_expectedMessage}', but got '{thrown.Message}'"));
                }
            }

            return await base.InvokeTestMethodAsync(aggregator);
        }
    }

    public class ExceptionFactDiscoverer : FactDiscoverer
    {
        public ExceptionFactDiscoverer(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
            Console.WriteLine("**** OK2");
        }

        public override IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            Console.WriteLine("**** OK3");
            var retval = new List<IXunitTestCase>(/* base.Discover(discoveryOptions, testMethod, factAttribute) */);

            var expectedExceptionType = factAttribute.GetNamedArgument<Type>("ExpectedExceptionType");
            var expectedMessage = factAttribute.GetNamedArgument<string>("ExpectedMessage");
            var testMethodDisplay = discoveryOptions.MethodDisplayOrDefault();

            retval.Add(new ExceptionTestCase(DiagnosticMessageSink, testMethodDisplay, testMethod, expectedExceptionType, expectedMessage));
            return retval;
        }

        /*
        override Di
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions,
                                                    ITestMethod testMethod,
                                                    IAttributeInfo factAttribute)
        {
            _diagnosticMessageSink.OnMessage(new DiagnosticMessage($"■ Discover called for: {testMethod.Method.Name}"));

            var expectedExceptionType = factAttribute.GetNamedArgument<Type>("ExpectedExceptionType");
            var expectedMessage = factAttribute.GetNamedArgument<string>("ExpectedMessage");
            var testMethodDisplay = discoveryOptions.MethodDisplayOrDefault();

            // return Array.Empty<IXunitTestCase>();

        }
        */
    }

    public class XXXTests
    {
        /*
        [ExceptionFact2]
        public static void ExceptionFact_Fail_IfNoExceptionThrown()
        {
            Task.Delay(1000).Wait();
        }

        [ExceptionFact2]
        public static void ExceptionFact_Success_IfExceptionThrown()
        {
            throw new ArgumentNullException("hoge", "This is a test exception for test method.");
        }
        */

        /*
        [ExceptionFact(typeof(InvalidOperationException))]
        public static void ExceptionFact_WithType_Fail_IfNoExceptionThrown()
        {
        }

        [ExceptionFact(typeof(InvalidOperationException))]
        public static void ExceptionFact_WithType_Fail_IfExceptionTypeDiffer()
        {
            throw new Exception("This is a test exception for test method.");
        }

        [ExceptionFact(typeof(InvalidOperationException))]
        public static void ExceptionFact_WithType_Success_IfExceptionTypeIsExpected()
        {
            throw new InvalidOperationException("This is a test exception for test method.");
        }

        [ExceptionFact(typeof(ArgumentNullException), "expected message")]
        public static async Task ExceptionFact_WithTypeAndMessage_Fail_IfNoExceptionThrown()
        {
            await Task.Run(() =>
            {
            });
        }
        */
    }
}