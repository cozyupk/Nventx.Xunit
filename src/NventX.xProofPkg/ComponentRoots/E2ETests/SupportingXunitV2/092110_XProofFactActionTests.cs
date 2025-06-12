using System;
using System.Threading.Tasks;
using NventX.xProof.BaseProofLibrary.Proofs;
using NventX.xProof.SupportingXunit.TypeBasedProofDiscoverer;
using Xunit;

namespace NventX.xProof.SupportingXunit.E2ETests
{
    /*
    public class XPoofTheoryActionTests
    {
        [Fact]
        public void SUCCESS_Theory()
        {
            Assert.True(true);
        }
        [Fact]
        public void FAIL_Theory()
        {
            Assert.True(false);
        }

        [ProofFact]
        public void FAIL_Fact_ThrowException(XProof xp)
        {
            xp.Probe(() => throw new Exception($"(1) Failing in xProof."));
        }

        [ProofFact]
        public void FAIL_Fact_ThrowExceptionMultiTimes(XProof xp)
        {
            xp.Probe(() => throw new Exception($"(1) Failing in xProof."));
            xp.Probe(() => throw new Exception($"(1) Failing in xProof."));
        }

        [ProofTheory,
            InlineData(345),
            InlineData(678)]
        public void SUCCESS_Theory_WithNoProbe(XProof _1, int _2)
        {
        }

        [ProofTheory,
            InlineData(345)]
        public void SUCCESS_Theory_DoNothing(XProof xp, int _)
        {
            xp.Probe(() => {
                // Do nothing
            });
        }

        [ProofTheory,
            InlineData(345)]
        public void SUCCESS_Theory_DoNothingMultiTimes(XProof xp, int _)
        {
            xp.Probe(() => {
                // Do nothing
            });
            xp.Probe(() => {
                // Do nothing
            });
        }

        [ProofTheory,
            InlineData(345),
            InlineData(678)]
        public void FAIL_Theory_ThrowException(XProof xp, int x)
        {
            xp.Probe(() => throw new Exception($"(1) Failing with value {x} in xProof."));
        }

        [ProofTheory,
            InlineData(345),
            InlineData(678)]
        public void FAIL_Theory_ThrowExceptionMultiTimes(XProof xp, int x)
        {
            xp.Probe(() => throw new Exception($"(1) Failing with value {x} in xProof."));
            xp.Probe(() => throw new Exception($"(1) Failing with value {x} in xProof."));
        }
    }
    */

    public class XProofFactActionTests
    {
        private static void Delay(int milliseconds)
        {
            Task.Delay(milliseconds).Wait();
        }

        [Fact]
        public void SUCCESS_Fact()
        {
            Delay(100); // Simulate some delay
            Assert.True(true);
        }

        [Fact]
        public void FAIL_Fact()
        {
            Delay(100); // Simulate some delay
            Assert.True(false);
        }

        /*
        [ProofFact]
        public void FAIL_ProofFact_WithNoProbe(XProof _)
        {
        }

        [ProofFact]
        public void SUCCESS_ProofFact_DoNothing(XProof xp)
        {
            xp.Probe(() => {
                // Do nothing
                Delay(100); // Simulate some delay
            });
        }
        */

        [ProofFact]
        public void SUCCESS_ProofFact_DoNothingMultiTimes(XProof xp)
        {
            xp.Probe(() => {
                // Do nothing
                Delay(300); // Simulate some delay
                // Console.WriteLine("Finished (1)");
            });
            xp.Probe(() => {
                // Do nothing
                Delay(200); // Simulate some delay
                // Console.WriteLine("Finished (2)");
            });
            xp.Probe(() => {
                // Do nothing
                Delay(100); // Simulate some delay
                // Console.WriteLine("Finished (3)");
            });
        }

        [ProofFact]
        public void FAIL_ProofFact_CombinedDelegates_Throw(XProof xp)
        {
            Action a = () => { }; // Console.WriteLine("A");
            Action b = () => throw new Exception("Failure in B");
            xp.Probe(a + b);
        }

        [ProofFact]
        public void FAIL_ProofFact_MultipleDifferentExceptions(XProof xp)
        {
            xp.Probe(() => throw new InvalidOperationException("Invalid op"));
            xp.Probe(() => throw new ArgumentException("Bad arg"));
        }

        /*
        [ProofFact]
        public void FAIL_Fact_NullProbeAction(XProof _)
        {
            // Action? act = null;
            // xp.Probe(act!);
        }

        [ProofFact]
        public void SUCCESS_Fact_CombinedDelegates_NoThrow(XProof xp)
        {
            Action a = () => Console.WriteLine("A");
            Action b = () => Console.WriteLine("B");
            xp.Probe(a + b);
        }



        [ProofFact]
        public void SUCCESS_Fact_ProbeWithLabel(XProof xp)
        {
            xp.Probe(() => { }, label: "LabeledSuccess");
        }



        [ProofFact]
        public void SUCCESS_Fact_ProbeWithCallerInfo(XProof xp)
        {
            xp.Probe(() => Console.WriteLine("Action"), callerMemberName: "TestCaller");
        }
        */
    }
}
