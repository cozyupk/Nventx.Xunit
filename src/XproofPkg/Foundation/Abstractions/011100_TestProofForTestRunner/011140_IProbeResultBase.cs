using System;

namespace Xproof.Abstractions.TestProofForTestRunner
{
    /// <summary>
    /// Base interface for probe results, which contains common properties like elapsed time and exception.
    /// </summary>
    public interface IProbeResultBase
    {
        /// <summary>
        /// The elapsed time for the probe execution.
        /// </summary>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// The exception that occurred during the probe execution, if any.
        /// </summary>
        Exception? Exception { get; }
    }
}
