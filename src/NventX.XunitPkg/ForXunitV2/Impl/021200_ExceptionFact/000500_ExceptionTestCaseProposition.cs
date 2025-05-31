using System;
using System.Collections.Generic;
using System.Linq;
using NventX.Xunit.Abstractions;

namespace NventX.Xunit.Generic
{
    internal class ExceptionTestCaseProposition : ITestCaseProposition
    {

        public IEnumerable<string?> CheckDiscoverArgs(
            TestCasePropositionType testPropositionType
        )
        {
            // Allow exactly one parameter of type IExceptionRecorder
            var parameters = testMethod.Method.GetParameters();
            List<string?> messages = new();
            if (parameters.Count() != 1)
            {
                messages.Add($"[ExceptionFact] method must have exactly one parameter of type IExceptionRecorder. Unexpected parameter count: {parameters.Count()}");
            }
            else
            {
                var parameterType = parameters.First().ParameterType.ToRuntimeType();
                if (!typeof(IExceptionRecorder).IsAssignableFrom(parameterType))
                {
                    messages.Add($"[ExceptionFact] method must have exactly one parameter of type IExceptionRecorder. Unexpected parameter type: {parameterType}");
                }
            }

            return messages;
        }

        public record CreateTestRecord()
        {

        }

        internal record ExceptionTestCaseRecord : TestCaseRecord
        {
            public Type? ExpectedExceptionType { get; }
            public string? ExpectedMessageSubstring { get; }

            public ExceptionTestCaseRecord(
                Type? expectedExceptionType,
                string? expectedMessageSubstring
            ) {
                ExpectedExceptionType = expectedExceptionType;
                ExpectedMessageSubstring = expectedMessageSubstring;
            }
        }

        public IEnumerable<ITestCaseProposition.TestCaseRecord> CreateTestCaseProposition(
            TestCasePropositionType testPropositionType,
        )
        {
            // Retrieve the expected exception type and message substring from the attribute
            var expectedExceptionType = propositionAttribute.GetNamedArgument<Type>("ExpectedExceptionType");
            var expectedMessageSubstring = propositionAttribute.GetNamedArgument<string>("ExpectedMessageSubstring");
            var testMethodDisplay = discoveryOptions.MethodDisplayOrDefault();

            // Create a new instance of ExceptionExpectedTestCase with the provided parameters and return it as an IEnumerable<IXunitTestCase>
            return new List<TestCaseRecord>() {
                new ExceptionTestCaseRecord(expectedExceptionType, expectedMessageSubstring)
            };
        }
    }
}
