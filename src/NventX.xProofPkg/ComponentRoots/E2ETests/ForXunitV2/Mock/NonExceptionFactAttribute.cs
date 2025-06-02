using Xunit;
using Xunit.Sdk;

namespace NventX.Xunit.E2ETests.ForXunitV2.Mock
{
    [XunitTestCaseDiscoverer($"NventX.Xunit.E2ETests.ForXunitV2.Mock.{nameof(NonExceptionFactTestCaseDiscoverer)}", "NventX.Xunit.E2ETests.ForXunitV2")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class NonExceptionFactAttribute : FactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonExceptionFactAttribute"/> class.
        /// </summary>
        public NonExceptionFactAttribute()
        {
            // This attribute is used to mark test methods that do not throw exceptions.
            // It can be used for tests that are expected to complete successfully without exceptions.
        }
    }
}
