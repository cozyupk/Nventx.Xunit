using System;

namespace Xproof.Abstractions.TestProofForTestRunner
{
    public interface IProbeResult<out TLabelAxes>
    {
        /// <summary>
        /// The probe scope record associated with this result.
        /// </summary>
        IProbeScopeRecord<TLabelAxes> ProbeScopeRecord { get; }

        TimeSpan Elapsed { get; }

        Exception? Exception { get; }
    }

    /// <summary>
    /// Default interface for probe results, which uses string as the label axes type.
    /// </summary>
    public interface IProbeResult
    {
        /// <summary>
        /// The probe scope record associated with this result.
        /// </summary>
        IProbeScopeRecord<string> ProbeScopeRecord { get; }

        TimeSpan Elapsed { get; }

        Exception? Exception { get; }
    }
}
