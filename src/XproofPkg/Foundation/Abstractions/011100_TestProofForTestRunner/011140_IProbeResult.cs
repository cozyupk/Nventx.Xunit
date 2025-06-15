using System;

namespace Xproof.Abstractions.TestProofForTestRunner
{
    public interface IProbeResult<out TAxes>
    {
        /// <summary>
        /// The probe scope record associated with this result.
        /// </summary>
        IProbeScopeRecord<TAxes> ProbeScopeRecord { get; }

        TimeSpan Elapsed { get; }

        Exception? Exception { get; }
    }
}
