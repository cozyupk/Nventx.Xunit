namespace Xproof.Abstractions.TestProofForTestRunner
{
    /// <summary>
    /// An interface for probe results, which uses a generic type for label axes.
    /// </summary>
    /// <typeparam name="TLabelAxes"></typeparam>
    public interface IProbeResult<out TLabelAxes> : IProbeResultBase
    {
        /// <summary>
        /// The probe scope record associated with this result.
        /// </summary>
        IProbeScopeRecord<TLabelAxes> ProbeScopeRecord { get; }
    }

    /// <summary>
    /// Default interface for probe results, which uses string as the label axes type.
    /// </summary>
    public interface IProbeResult : IProbeResultBase
    {
        /// <summary>
        /// The probe scope record associated with this result.
        /// </summary>
        IProbeScopeRecord<string> ProbeScopeRecord { get; }
    }

}
