using System;
using Xunit.Sdk;

namespace NventX.Xunit.ExceptionTesting
{
    /// <summary>
    /// Represents a test case that is expected to throw an exception,
    /// with optional validation on exception type and message content.
    /// </summary>
    public interface IExceptionTestCase : IXunitTestCase
    {
        /// <summary>
        /// Gets the type of the expected exception.
        /// If null, the exception type is not validated.
        /// </summary>
        public Type? ExpectedExceptionType { get; }

        /// <summary>
        /// Gets the substring that should be contained in the thrown exception's message.
        /// If null, the message content is not validated.
        /// </summary>
        public string? ExpectedMessageSubstring { get; }
    }
}
