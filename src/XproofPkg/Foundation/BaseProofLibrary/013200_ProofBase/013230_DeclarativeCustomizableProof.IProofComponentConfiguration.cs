using Xproof.Abstractions.Sdk;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.BaseProofLibrary.ProofBase
{
    partial class DeclarativeCustomizableProof<TLabelAxes>
    {
        protected interface IProofComponentConfiguration
        {
            void SetProofForAction(IRawProofForAction<TLabelAxes> proofForAction);
            void SetCombinerForActions(ICombinerForActions<TLabelAxes> combinerForActions);
            void SetProofForFunc(IRawProofForFunc<TLabelAxes> proofForFunc);
            void SetCombinerForFuncs(ICombinerForFuncs<TLabelAxes> combinerForFuncs);
        }
    }
}
