using System;

namespace Xproof.Abstractions.TestProofForTestRunner
{
    public interface IProbeResult
    {
        IProbeScopeRecord ProbeScopeRecord { get; }

        TimeSpan Elapsed { get; }

        Exception? Exception { get; }
    }
}
