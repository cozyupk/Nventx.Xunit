
using NventX.Xunit.Generic;
using Xunit.Abstractions;

namespace NventX.Xunit.E2ETests.ForXunitV2.Mock
{
    internal class NonExceptionFactTestCaseDiscoverer(IMessageSink diagnosticMessageSink) : FactTestCaseDiscoverer<NonExceptionProof>(diagnosticMessageSink)
    {
    }
}
