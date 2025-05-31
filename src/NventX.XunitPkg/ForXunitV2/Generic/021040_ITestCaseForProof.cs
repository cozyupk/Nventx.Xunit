using Xunit.Sdk;

namespace NventX.Xunit.Generic
{
    public interface ITestCaseForProof : IXunitTestCase
    {
        ISerializableTestProofFactory TestProofFactory { get; }
    }
}
