using System;
using Xunit;
using Xunit.Sdk;

namespace NventX.Xunit.ExceptionTesting
{
    /// <summary>
    /// Attribute to mark a test method that is expected to throw an exception.
    /// </summary>
    [XunitTestCaseDiscoverer("NventX.Xunit.ExceptionFact.ExceptionFactDiscoverer", "NventX.Xunit.ForXunitV2")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExceptionFactAttribute : FactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFactAttribute"/> class.
        /// </summary>
        public ExceptionFactAttribute(Type? expectedExceptionType = null, string? expectedMessageSubstring = null)
        {
            ExpectedExceptionType = expectedExceptionType;
            ExpectedMessageSubstring = expectedMessageSubstring;
        }

        /// <summary>
        /// Gets the type of the expected exception that the test method should throw.
        /// </summary>
        public Type? ExpectedExceptionType { get; }

        /// <summary>
        /// Gets the substring that should be contained in the message of the expected exception.
        /// </summary>
        public string? ExpectedMessageSubstring { get; }
    }
}
