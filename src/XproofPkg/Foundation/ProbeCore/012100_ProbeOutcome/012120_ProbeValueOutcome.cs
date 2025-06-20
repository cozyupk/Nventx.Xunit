using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.ProbeCore.ProbeOutcome
{
    /// <summary>
    /// Represents a successful probe outcome containing a value of type <typeparamref name="T"/>.
    /// </summary>
    public abstract class ProbeValueOutcome<T> : IProbeOutcome<T>
    {
        /// <summary>
        /// Stores the value of the probe outcome.
        /// </summary>
        protected T Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeValueOutcome{T}"/> class with the specified value.
        /// </summary>
        public ProbeValueOutcome(T value)
        {
            Value = value;
        }
    }
}
