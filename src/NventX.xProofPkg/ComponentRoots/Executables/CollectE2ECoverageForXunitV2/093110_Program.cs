using System.Reflection;
using NventX.xProof.SupportingXunit.E2ETests;
using NventX.xProof.SupportingXunit.SelfHostedTestRuntime;

namespace NventX.xProof.SupportingXunit.CollectE2ECoverageForXunitV2
{
    internal class Program
    {
        public static void Main(string[] _)
        {
            ProofTestRuntimeHost proofTestRuntimeHost = new();
            Assembly asm = typeof(XProofFactActionTests).Assembly;
            proofTestRuntimeHost.Execute(asm);
        }
    }
}