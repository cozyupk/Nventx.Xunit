using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestMethods;
using Xproof.Abstractions.TestProofForTestRunner;
using Xproof.BaseProofLibrary.ProofBase;
using Xproof.BaseProofLibrary.Proofs;
using Xproof.SupportingXunit.AssertBinding;
using Xproof.SupportingXunit.TypeBasedProofDiscoverer;
using Xunit;

namespace Xproof.SupportingXunit.E2ETests
{
    /*
    public class proofoofTheoryActionTests
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
        public void FAIL_Fact_ThrowException(proofroof proof)
        {
            proof.Probe(() => throw new Exception($"(1) Failing in proofroof."));
        }

        [ProofFact]
        public void FAIL_Fact_ThrowExceptionMultiTimes(proofroof proof)
        {
            proof.Probe(() => throw new Exception($"(1) Failing in proofroof."));
            proof.Probe(() => throw new Exception($"(1) Failing in proofroof."));
        }

        [ProofTheory,
            InlineData(345),
            InlineData(678)]
        public void SUCCESS_Theory_WithNoProbe(proofroof _1, int _2)
        {
        }

        [ProofTheory,
            InlineData(345)]
        public void SUCCESS_Theory_DoNothing(proofroof proof, int _)
        {
            proof.Probe(() => {
                // Do nothing
            });
        }

        [ProofTheory,
            InlineData(345)]
        public void SUCCESS_Theory_DoNothingMultiTimes(proofroof proof, int _)
        {
            proof.Probe(() => {
                // Do nothing
            });
            proof.Probe(() => {
                // Do nothing
            });
        }

        [ProofTheory,
            InlineData(345),
            InlineData(678)]
        public void FAIL_Theory_ThrowException(proofroof proof, int x)
        {
            proof.Probe(() => throw new Exception($"(1) Failing with value {x} in proofroof."));
        }

        [ProofTheory,
            InlineData(345),
            InlineData(678)]
        public void FAIL_Theory_ThrowExceptionMultiTimes(proofroof proof, int x)
        {
            proof.Probe(() => throw new Exception($"(1) Failing with value {x} in proofroof."));
            proof.Probe(() => throw new Exception($"(1) Failing with value {x} in proofroof."));
        }
    }
    */


    public class DefaultProofFactActionTests
    {
        private static class Utils
        {
            public static void Delay(int milliseconds)
            {
                Task.Delay(milliseconds).Wait();
            }
        }

        public class SetUpThrowsExceptionProof : InvokableProofBase<string>
        {
            public override void Setup(ProofInvocationKind proofInvocationKind)
            {
                throw new Exception("An exception occured for testing purpose.");
            }
        }

        public class SetUpThrowsExceptionWithDelay50msProof : InvokableProofBase<string>
        {
            public override void Setup(ProofInvocationKind proofInvocationKind)
            {
                Utils.Delay(50); // Simulate some delay before throwing the exception
                throw new Exception("An exception occured for testing purpose.");
            }
        }

        public class ProbeThrowsExceptionProof : InvokableProofBase<string>, IProofForAction<string>
        {
            public void Probe(Action act, string? label = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string? callerMemberName = null, MethodInfo? invokedMethodInfo = null, object?[]? invokedParameters = null, IPositionInArray? combinedPosition = null)
            {
                throw new Exception("An exception occured for testing purpose.");
            }
        }

        public class ProbeThrowsExceptionWithDelay50msProof : InvokableProofBase<string>, IProofForAction<string>
        {
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
        public void FAIL_ProofFact_WithNoProbe(proofroof _)
        {
        }

        [ProofFact]
        public void SUCCESS_ProofFact_DoNothing(proofroof proof)
        {
            proof.Probe(() => {
                // Do nothing
                Delay(100); // Simulate some delay
            });
        }
        */

        [ProofFact(typeof(SetUpThrowsExceptionProof))]
        public void FAIL_ProofFact_SetupThrowsException(SetUpThrowsExceptionProof _)
        {
            // This test is eproofected to fail because the setup throws an exception
            // The exception is caught by the ProofFact attribute and reported as a failure
        }

        [ProofFact(typeof(SetUpThrowsExceptionWithDelay50msProof))]
        public void FAIL_ProofFact_SetupThrowsExceptionWithDelay50ms(SetUpThrowsExceptionWithDelay50msProof _)
        {
            // This test is eproofected to fail because the setup throws an exception
            // The exception is caught by the ProofFact attribute and reported as a failure
        }

        [ProofFact(typeof(ProbeThrowsExceptionProof))]
        public void FAIL_ProofFact_ProbeThrowsException(ProbeThrowsExceptionProof proof)
        {
            // This test is eproofected to fail because the probe throws an exception
            // The exception is caught by the ProofFact attribute and reported as a failure
            proof.Probe(() => { });
        }

        [ProofFact(typeof(ProbeThrowsExceptionWithDelay50msProof))]
        public void FAIL_ProofFact_ProbeThrowsExceptionWithDelay50ms(ProbeThrowsExceptionWithDelay50msProof proof)
        {
            // This test is eproofected to fail because the probe throws an exception
            // The exception is caught by the ProofFact attribute and reported as a failure
            proof.Probe(() => { });
        }

        [ProofFact]
        public void FAIL_ProofFact_NullProbeAction(AssertProof proof)
        {
            Action? act = null;
            proof.Probe(act!);
        }

        [ProofFact]
        public void SUCCESS_ProofFact_DoNothingMultiTimes(AssertProof proof)
        {
            proof.Probe(() =>
            {
                // Do nothing
                Utils.Delay(300); // Simulate some delay
                // Console.WriteLine("Finished (1)");
            });
            proof.Probe(() =>
            {
                // Do nothing
                Utils.Delay(200); // Simulate some delay
                // Console.WriteLine("Finished (2)");
            });
            proof.Probe(() =>
            {
                // Do nothing
                Utils.Delay(100); // Simulate some delay
                // Console.WriteLine("Finished (3)");
            });
        }

        [ProofFact]
        public void FAIL_ProofFact_ThrowExceptionInProbeAction(AssertProof proof)
        {
            proof.Probe(() => throw new Exception("Failure for testing purpose"));
        }

        [ProofFact]
        public void FAIL_ProofFact_ThrowExceptionWithDelay50msInProbeAction(AssertProof proof)
        {
            proof.Probe(() => {
                Utils.Delay(50); // Simulate some delay
                throw new Exception("Failure in B");
            });
        }

        [ProofFact]
        public void FAIL_ProofFact_CombinedDelegates_ThrowExceptionInProbeAction(AssertProof proof)
        {
            Action a = () => { }; // Console.WriteLine("A");
            Action b = () => throw new Exception("Failure in B");
            proof.Probe(a + b);
        }

        [ProofFact]
        public void FAIL_ProofFact_ThrowMultipleDifferentExceptionsWithDelaysInProbeAction(AssertProof proof)
        {
            proof.Probe(() =>
            {
                Utils.Delay(100); // Simulate some delay
                throw new InvalidOperationException("Invalid op");
            });
            proof.Probe(() =>
            {
                Utils.Delay(200); // Simulate some delay
                throw new ArgumentException("Bad arg");
            });
        }

        [ProofFact]
        public void FAIL_ProofFact_ThrowMultipleDifferentExceptionsInProbeAction(AssertProof proof)
        {
            proof.Probe(() => throw new InvalidOperationException("Invalid op"));
            proof.Probe(() => throw new ArgumentException("Bad arg"));
        }

        [ProofFact]
        public void Fail_ProofFact_UsingAssert(AssertProof ap)
        {
            var message = "Long live the GPT!";

            ap.Null(message);
            ap.NotNull(message);
            var dummy = ap.IsType<int>(message); // This will fail because message is a string, not an int
            ap.NotNull(dummy);
            var stringMessage = ap.IsType<string>(message);
            ap.NotNull(dummy);
            ap.Equal("Long live the GPT!", stringMessage);
        }

        /*
        [ProofFact]
        public void SUCCESS_Fact_CombinedDelegates_NoThrow(proofroof proof)
        {
            Action a = () => Console.WriteLine("A");
            Action b = () => Console.WriteLine("B");
            proof.Probe(a + b);
        }



        [ProofFact]
        public void SUCCESS_Fact_ProbeWithLabel(proofroof proof)
        {
            proof.Probe(() => { }, label: "LabeledSuccess");
        }



        [ProofFact]
        public void SUCCESS_Fact_ProbeWithCallerInfo(proofroof proof)
        {
            proof.Probe(() => Console.WriteLine("Action"), callerMemberName: "TestCaller");
        }
        */
    }
}
