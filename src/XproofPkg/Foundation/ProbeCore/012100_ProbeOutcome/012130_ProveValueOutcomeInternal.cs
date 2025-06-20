namespace Xproof.ProbeCore.ProbeOutcome
{
    /// <summary>
    /// Internal implementation of <see cref="ProbeValueOutcome{T}"/> that exposes the unwrapping mechanism.
    /// </summary>
    internal sealed class ProbeValueOutcomeInternal<T> : ProbeValueOutcome<T>, IProbeOutcomeInternal<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeValueOutcomeInternal{T}"/> class with the specified value.
        /// </summary>
        public ProbeValueOutcomeInternal(T value) : base(value)
        {
            // No additional initialization needed.
        }

        /// <summary>
        /// Returns the wrapped value of the probe outcome.
        /// </summary>
        public T Unwrap() => Value;
    }
}
