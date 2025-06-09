using NventX.xProof.BaseProofLibrary.Proofs;

namespace NventX.xProof.SupportingXunit.E2ETests
{
    public class XProofFactActionTests
    {
#pragma warning disable CA1822 // Test data attribute should only be used on a Theory
        /*
        [ProofFact]
        public void SUCCESS_Fact_WithNoProbe(XProof _)
        {
        }

        [ProofFact]
        public void SUCCESS_Fact_DoNothing(XProof xp)
        {
            xp.Probe(() => { 
                // Do nothing
            });
        }

        [ProofFact]
        public void SUCCESS_Fact_DoNothingMultiTimes(XProof xp)
        {
            xp.Probe(() => { 
                // Do nothing
            });
            xp.Probe(() => { 
                // Do nothing
            });
            xp.Probe(() => { 
                // Do nothing
            });
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
*/
        /*
        [MockProofTheory,
            InlineData(345),
            InlineData(678)]
        public void SUCCESS_Theory_WithNoProbe(XProof _1, int _2)
        {
        }

        [MockProofTheory,
            InlineData(345)]
        public void SUCCESS_Theory_DoNothing(XProof xp, int _)
        {
            xp.Probe(() => { 
                // Do nothing
            });
        }

        [MockProofTheory,
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

        [MockProofTheory,
            InlineData(345),
            InlineData(678)]
        public void FAIL_Theory_ThrowException(XProof xp, int x)
        {
            xp.Probe(() => throw new Exception($"(1) Failing with value {x} in xProof."));
        }

        [MockProofTheory,
            InlineData(345),
            InlineData(678)]
        public void FAIL_Theory_ThrowExceptionMultiTimes(XProof xp, int x)
        {
            xp.Probe(() => throw new Exception($"(1) Failing with value {x} in xProof."));
            xp.Probe(() => throw new Exception($"(1) Failing with value {x} in xProof."));
        }
        */

#pragma warning restore CA1822 // Test data attribute should only be used on a Theory
    }
}
