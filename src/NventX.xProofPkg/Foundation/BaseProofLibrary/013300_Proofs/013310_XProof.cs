using NventX.xProof.BaseProofLibrary.BaseImpl;
using NventX.xProof.BaseProofLibrary.DefaultModules;

namespace NventX.xProof.BaseProofLibrary.Proofs
{
    /// <summary>
    /// A proof implementation that allows for late failure collection during test execution.
    /// </summary>
    public class XProof : DeclarativeCustomizableProof 
    {
        /// <summary>
        /// The default proof used by [ProofFact] when no other is specified.
        /// This proof collects multiple failures during test execution and reports them at once.
        /// </summary>
        /// <param name="config"></param>
        protected override void InitializeProofComponent(IProofComponentConfiguration config)
        {
            // Call base Implementation to ensure any base setup is done first
            // Optional: if base does something
            // base.InitializeProofComponent(config); 

            // Override the proof components with specific implementations
            config.SetProofForAction(new ProofForAction(this));
            config.SetCombinerForActions(new CombinerForActions(this));
            config.SetProofForFunc(new ProofForFunc(this));
            config.SetCombinerForFuncs(new CombinerForFuncs(this));
        }
    }
}
