using NventX.xProof.Abstractions.TestProofForTestMethods;

namespace NventX.xProof.BaseProofLibrary.BaseImpl
{
    partial class DeclarativeCustomizableProof
    {
        protected interface IProofComponentConfiguration
        {
            void SetProofForAction(IProofForAction proofForAction);
            void SetCombinerForActions(ICombinerForActions combinerForActions);
            void SetProofForFunc(IProofForFunc proofForFunc);
            void SetCombinerForFuncs(ICombinerForFuncs combinerForFuncs);
        }
    }
}
