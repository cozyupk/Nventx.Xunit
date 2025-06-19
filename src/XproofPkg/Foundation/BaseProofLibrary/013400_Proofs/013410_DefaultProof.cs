using Xproof.BaseProofLibrary.DefaultModules;
using Xproof.BaseProofLibrary.ProofBase;

namespace Xproof.BaseProofLibrary.Proofs
{
    /// <summary>
    /// A proof implementation that allows for late failure collection during test execution.
    /// </summary>
    public class DefaultProof<TLabelAxes> : DeclarativeCustomizableProof<TLabelAxes>
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
            config.SetProofForAction(new ProofForAction<TLabelAxes>(this));
            config.SetCombinerForActions(new CombinerForActions<TLabelAxes>(this));
            config.SetProofForFunc(new ProofForFunc<TLabelAxes>(this));
            config.SetCombinerForFuncs(new CombinerForFuncs<TLabelAxes>(this));
        }
    }

    public class DefaultProof : DefaultProof<string>
    {
        // Including this empty class allows the use of Xproof without specifying axes.
    }
}
