using Xproof.Abstractions.Sdk;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.BaseProofLibrary.ProofBase
{
    partial class DeclarativeCustomizableProof<TAxes>
    {
        protected interface IProofComponentConfiguration
        {
            void SetProofForAction(IRawProofForAction<TAxes> proofForAction);
            void SetCombinerForActions(ICombinerForActions<TAxes> combinerForActions);
            void SetProofForFunc(IRawProofForFunc<TAxes> proofForFunc);
            void SetCombinerForFuncs(ICombinerForFuncs<TAxes> combinerForFuncs);
        }
    }
}
