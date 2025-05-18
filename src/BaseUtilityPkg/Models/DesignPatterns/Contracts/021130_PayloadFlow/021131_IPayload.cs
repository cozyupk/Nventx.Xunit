using System.Collections.Generic;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Represents an envelope containing metadata and a (potentially lazily-evaluated) sequence of payload bodies.
    /// </summary>
    public interface IPayload<out TPayloadMeta, out TPayloadBody>
    {
        /// <summary>
        /// Gets the metadata associated with the payload.
        /// </summary>
        TPayloadMeta Meta { get; }

        /// <summary>
        /// Gets the payload itself.
        /// </summary>
        IEnumerable<TPayloadBody> Bodies { get; }
    }
}