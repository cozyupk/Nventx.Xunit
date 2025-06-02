using System;
using Xunit;
using Xunit.Sdk;

namespace NventX.Xunit.SupportingBaseProofLibrary
{
    [XunitTestCaseDiscoverer("NventX.Xunit.E2ETests.ForXunitV2.Mock." + nameof(FailLateFactFactTestCaseDiscoverer), "NventX.Xunit.E2ETests.ForXunitV2")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FailLateFactAttribute : FactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailLateFactAttribute"/> class.
        /// </summary>
        public FailLateFactAttribute()
        {
            // This attribute is used to mark test methods that do not throw exceptions.
            // It can be used for tests that are expected to complete successfully without exceptions.
        }
    }
}
