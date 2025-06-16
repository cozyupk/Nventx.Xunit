using System;
using Xproof.Abstractions.Sdk;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.BaseProofLibrary.ProofBase
{
    partial class DeclarativeCustomizableProof<TLabelAxes>
    {
        private class ProofComponentConfiguration : IProofComponentConfiguration
        {
            private bool IsFrozen = false;

            private object LockObject { get; } = new object();

            private DeclarativeCustomizableProof<TLabelAxes> Proof { get; }

            public ProofComponentConfiguration(DeclarativeCustomizableProof<TLabelAxes> proof)
            {
                Proof = proof ?? throw new ArgumentNullException(nameof(proof), $"{nameof(DeclarativeCustomizableProof<TLabelAxes>)} cannot be null.");
            }

            public void SetProofForAction(IRawProofForAction<TLabelAxes> proofForAction)
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

            public void SetCombinerForActions(ICombinerForActions<TLabelAxes> combinerForActions)
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

            public void SetProofForFunc(IRawProofForFunc<TLabelAxes> proofForFunc)
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

            public void SetCombinerForFuncs(ICombinerForFuncs<TLabelAxes> combinerForFuncs)
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
