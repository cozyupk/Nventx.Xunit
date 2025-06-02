using NventX.xProof.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit
{
    internal interface ITestCaseForProof : IXunitTestCase
    {
        ITestProof CreateTestProof();
    }
}
