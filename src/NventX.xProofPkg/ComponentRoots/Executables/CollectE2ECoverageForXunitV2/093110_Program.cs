using System.Reflection;
using NventX.xProof.SupportingXunit.SelfHostedTestRuntime;

namespace NventX.xProof.SupportingXunit.E2ETests
{
    internal class Program
    {
        public static void Main(string[] _)
        {
            ProofTestRuntimeHost proofTestRuntimeHost = new ProofTestRuntimeHost();
            Assembly asm = typeof(XProofFactActionTests).Assembly;
            proofTestRuntimeHost.Execute(asm);
        }
    }
}