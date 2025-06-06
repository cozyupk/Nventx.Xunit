using System;
using NventX.xProof.Abstractions.TestProofForTestMethods;

namespace NventX.xProof.BaseProofLibrary.BaseImpl
{
    partial class DeclarativeCustomizableProof
    {
        private class ProofComponentConfiguration : IProofComponentConfiguration
        {
            private bool IsFrozen = false;

            private object LockObject { get; } = new object();

            private DeclarativeCustomizableProof Proof { get; }

            public ProofComponentConfiguration(DeclarativeCustomizableProof proof)
            {
                Proof = proof ?? throw new ArgumentNullException(nameof(proof), $"{nameof(DeclarativeCustomizableProof)} cannot be null.");
            }

            public void SetProofForAction(IProofForAction proofForAction)
            {
                lock (LockObject)
                {
                    if (IsFrozen)
                    {
                        throw new InvalidOperationException($"{this.GetType().FullName} is already frozen and cannot be changed by {nameof(SetProofForAction)}.");
                    }
                    Proof.ProofForAction = proofForAction
                                     ?? throw new ArgumentNullException(nameof(proofForAction), $"{nameof(ProofForAction)} cannot be null.");
                }
            }

            public void SetCombinerForActions(ICombinerForActions combinerForActions)
            {
                lock (LockObject)
                {
                    if (IsFrozen)
                    {
                        throw new InvalidOperationException($"{this.GetType().FullName} is already frozen and cannot be changed by {nameof(SetCombinerForActions)}.");
                    }
                    Proof.CombinerForActions = combinerForActions
                                         ?? throw new ArgumentNullException(nameof(combinerForActions), $"{nameof(CombinerForActions)} cannot be null.");
                }
            }

            public void SetProofForFunc(IProofForFunc proofForFunc)
            {
                lock (LockObject)
                {
                    if (IsFrozen)
                    {
                        throw new InvalidOperationException($"{this.GetType().FullName} is already frozen and cannot be changed by {nameof(SetProofForFunc)}.");
                    }
                    Proof.ProofForFunc = proofForFunc
                                   ?? throw new ArgumentNullException(nameof(proofForFunc), $"{nameof(ProofForFunc)} cannot be null.");
                }
            }

            public void SetCombinerForFuncs(ICombinerForFuncs combinerForFuncs)
            {
                lock (LockObject)
                {
                    if (IsFrozen)
                    {
                        throw new InvalidOperationException($"{this.GetType().FullName} is already frozen and cannot be changed by {nameof(SetCombinerForFuncs)}.");
                    }
                    Proof.CombinerForFuncs = combinerForFuncs
                                       ?? throw new ArgumentNullException(nameof(combinerForFuncs), $"{nameof(CombinerForFuncs)} cannot be null.");
                }
            }

            public void Freeze()
            {
                lock (LockObject)
                {
                    if (IsFrozen)
                    {
                        throw new InvalidOperationException($"{this.GetType().FullName} is already frozen and cannot be changed.");
                    }
                    IsFrozen = true;
                }
            }
        }
    }
}
