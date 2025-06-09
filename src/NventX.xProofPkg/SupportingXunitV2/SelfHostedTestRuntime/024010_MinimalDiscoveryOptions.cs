using System;
using Xunit.Abstractions;

namespace NventX.xProof.SupportingXunit.SelfHostedTestRuntime
{
    public class MinimalDiscoveryOptions : ITestFrameworkDiscoveryOptions
    {
        public TValue GetValue<TValue>(string name)
        {
            switch (name)
            {
                case "xunit.discovery.MethodDisplay":
                    if (typeof(TValue).IsAssignableFrom(typeof(string)))
                    {
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
}
