using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestMethods;
using Xproof.Abstractions.TestProofForTestRunner;
using Xproof.SupportingXunit.TypeBasedProofDiscoverer;
using Xunit;

namespace Xproof.SupportingXunit.E2ETests
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
        private static class Utils
        {
            public static void Delay(int milliseconds)
            {
                Task.Delay(milliseconds).Wait();
            }
        }

        public class SetUpThrowsExceptionProof : IInvokableProof
        {
            public void Setup(ProofInvocationKind proofInvocationKind)
            {
                throw new Exception("An exception occured for testing purpose.");
            }

            public ProofInvocationKind ProofInvocationKind { get; private set; }

            public void RecordProbeResult(IProbeResult probeResult)
            {
                // nothing to do here for this test
            }

            public void Commit()
            {
                // nothing to do here for this test
            }

            public IEnumerable<IProbeResult> GetResults()
            {
                return []; // just return an empty enumerable for this test
            }
        }

        public class SetUpThrowsExceptionWithDelay50msProof : IInvokableProof
        {
            public void Setup(ProofInvocationKind proofInvocationKind)
            {
                Utils.Delay(50); // Simulate some delay before throwing the exception
                throw new Exception("An exception occured for testing purpose.");
            }
            public ProofInvocationKind ProofInvocationKind { get; private set; }

            public IEnumerable<IProbeResult> GetResults()
            {
                return []; // just return an empty enumerable for this test
            }

            public void RecordProbeResult(IProbeResult probeResult)
            {
                // nothing to do here for this test
            }

            public void Commit()
            {
                // nothing to do here for this test
            }
        }

        public class ProbeThrowsExceptionProof : IInvokableProof, IProofForAction<string>
        {
            public void Setup(ProofInvocationKind proofInvocationKind)
            {
                ProofInvocationKind = proofInvocationKind;
            }
            public ProofInvocationKind ProofInvocationKind { get; private set; }

            public IEnumerable<IProbeResult> GetResults()
            {
                return []; // just return an empty enumerable for this test
            }

            public void RecordProbeResult(IProbeResult probeResult)
            {
                // nothing to do here for this test
            }

            public void Commit()
            {
                // nothing to do here for this tes
            }

            public void Probe(Action act, string? label = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string? callerMemberName = null, MethodInfo? invokedMethodInfo = null, object?[]? invokedParameters = null, IPositionInArray? combinedPosition = null)
            {
                throw new Exception("An exception occured for testing purpose.");
            }
        }

        public class ProbeThrowsExceptionWithDelay50msProof : IInvokableProof, IProofForAction<string>
        {
            public void Setup(ProofInvocationKind proofInvocationKind)
            {
                ProofInvocationKind = proofInvocationKind;
            }
            public ProofInvocationKind ProofInvocationKind { get; private set; }

            public IEnumerable<IProbeResult> GetResults()
            {
                return []; // just return an empty enumerable for this test
            }

            public void RecordProbeResult(IProbeResult probeResult)
            {
                // nothing to do here for this test
            }

            public void Commit()
            {
                // nothing to do here for this tes
            }

            public void Probe(Action act, string? label = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string? callerMemberName = null, MethodInfo? invokedMethodInfo = null, object?[]? invokedParameters = null, IPositionInArray? combinedPosition = null)
            {
                Utils.Delay(50); // Simulate some delay before throwing the exception
                throw new Exception("An exception occured for testing purpose.");
            }
        }

        [Fact]
        public void SUCCESS_Fact()
        {
            Utils.Delay(100); // Simulate some delay
            Assert.True(true);
        }

        [Fact]
        public void FAIL_Fact()
        {
            Utils.Delay(100); // Simulate some delay
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

        [ProofFact(typeof(SetUpThrowsExceptionProof))]
        public void FAIL_ProofFact_SetupThrowsException(SetUpThrowsExceptionProof _)
        {
            // This test is expected to fail because the setup throws an exception
            // The exception is caught by the ProofFact attribute and reported as a failure
        }

        [ProofFact(typeof(SetUpThrowsExceptionWithDelay50msProof))]
        public void FAIL_ProofFact_SetupThrowsExceptionWithDelay50ms(SetUpThrowsExceptionWithDelay50msProof _)
        {
            // This test is expected to fail because the setup throws an exception
            // The exception is caught by the ProofFact attribute and reported as a failure
        }

        [ProofFact(typeof(ProbeThrowsExceptionProof))]
        public void FAIL_ProofFact_ProbeThrowsException(ProbeThrowsExceptionProof xp)
        {
            // This test is expected to fail because the probe throws an exception
            // The exception is caught by the ProofFact attribute and reported as a failure
            xp.Probe(() => { });
        }

        [ProofFact(typeof(ProbeThrowsExceptionWithDelay50msProof))]
        public void FAIL_ProofFact_ProbeThrowsExceptionWithDelay50ms(ProbeThrowsExceptionWithDelay50msProof xp)
        {
            // This test is expected to fail because the probe throws an exception
            // The exception is caught by the ProofFact attribute and reported as a failure
            xp.Probe(() => { });
        }

        [ProofFact]
        public void FAIL_ProofFact_NullProbeAction(BaseProofLibrary.Proofs.Xproof xp)
        {
            Action? act = null;
            xp.Probe(act!);
        }

        [ProofFact]
        public void SUCCESS_ProofFact_DoNothingMultiTimes(BaseProofLibrary.Proofs.Xproof xp)
        {
            xp.Probe(() =>
            {
                // Do nothing
                Utils.Delay(300); // Simulate some delay
                // Console.WriteLine("Finished (1)");
            });
            xp.Probe(() =>
            {
                // Do nothing
                Utils.Delay(200); // Simulate some delay
                // Console.WriteLine("Finished (2)");
            });
            xp.Probe(() =>
            {
                // Do nothing
                Utils.Delay(100); // Simulate some delay
                // Console.WriteLine("Finished (3)");
            });
        }

        [ProofFact]
        public void FAIL_ProofFact_ThrowExceptionInProbeAction(BaseProofLibrary.Proofs.Xproof xp)
        {
            xp.Probe(() => throw new Exception("Failure for testing purpose"));
        }

        [ProofFact]
        public void FAIL_ProofFact_ThrowExceptionWithDelay50msInProbeAction(BaseProofLibrary.Proofs.Xproof xp)
        {
            xp.Probe(() => {
                Utils.Delay(50); // Simulate some delay
                throw new Exception("Failure in B");
            });
        }

        [ProofFact]
        public void FAIL_ProofFact_CombinedDelegates_ThrowExceptionInProbeAction(BaseProofLibrary.Proofs.Xproof xp)
        {
            Action a = () => { }; // Console.WriteLine("A");
            Action b = () => throw new Exception("Failure in B");
            xp.Probe(a + b);
        }

        [ProofFact]
        public void FAIL_ProofFact_ThrowMultipleDifferentExceptionsWithDelaysInProbeAction(BaseProofLibrary.Proofs.Xproof xp)
        {
            xp.Probe(() =>
            {
                Utils.Delay(100); // Simulate some delay
                throw new InvalidOperationException("Invalid op");
            });
            xp.Probe(() =>
            {
                Utils.Delay(200); // Simulate some delay
                throw new ArgumentException("Bad arg");
            });
        }

        [ProofFact]
        public void FAIL_ProofFact_ThrowMultipleDifferentExceptionsInProbeAction(BaseProofLibrary.Proofs.Xproof xp)
        {
            xp.Probe(() => throw new InvalidOperationException("Invalid op"));
            xp.Probe(() => throw new ArgumentException("Bad arg"));
        }

        /*
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
