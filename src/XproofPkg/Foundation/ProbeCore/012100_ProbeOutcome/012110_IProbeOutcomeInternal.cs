namespace Xproof.ProbeCore.ProbeOutcome
{
    /// <summary>
    /// Internal interface for probe outcomes that expose an unwrapping mechanism.
    /// This interface is intended for internal use to retrieve the underlying value or throw the stored exception.
    /// </summary>
    internal interface IProbeOutcomeInternal<out T>
    {
        /// <summary>
        /// Returns the wrapped value if the probe was successful;
        /// otherwise, throws the exception that represents the failure.
        /// </summary>
        T Unwrap();
    }
}
