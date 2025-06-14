using Xproof.Abstractions.Sdk;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.BaseProofLibrary.ProofBase
{
    partial class DeclarativeCustomizableProof
    {
        protected interface IProofComponentConfiguration
        {
            void SetProofForAction(IRawProofForAction proofForAction);
            void SetCombinerForActions(ICombinerForActions combinerForActions);
            void SetProofForFunc(IRawProofForFunc proofForFunc);
            void SetCombinerForFuncs(ICombinerForFuncs combinerForFuncs);
        }
    }
}
