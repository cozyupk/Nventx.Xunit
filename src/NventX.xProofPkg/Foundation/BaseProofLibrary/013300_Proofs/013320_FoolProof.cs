using NventX.xProof.BaseProofLibrary.DefaultModules;

namespace NventX.xProof.BaseProofLibrary.Proofs
{
    public class FoolProof : AssembledProof
    {
        /// <summary>
        /// A fool-proof implementation of the proof system.
        /// </summary>
        public FoolProof() {
            // Injecting default implementations for fool-proofing
            InjectProofForAction(new ProofForAction(this));
            InjectCombinerForActions(new CombinerForActions(this));
            // 
            InjectProofForFunc(new ProofForFunc(this));
            InjectCombinerForFuncs(new CombinerForFuncs(this));
        }
    }
}
