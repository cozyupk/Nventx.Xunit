using System.Reflection;
using Xproof.SupportingXunit.E2ETests;
using Xproof.SupportingXunit.SelfHostedTestRuntime;

namespace Xproof.SupportingXunit.CollectE2ECoverageForXunitV2
{
    internal class Program
    {
        public static void Main(string[] _)
        {
            ProofTestRuntimeHost proofTestRuntimeHost = new();
            Assembly asm = typeof(DefaultProofFactActionTests).Assembly;
            proofTestRuntimeHost.Execute(asm);
        }
    }
}