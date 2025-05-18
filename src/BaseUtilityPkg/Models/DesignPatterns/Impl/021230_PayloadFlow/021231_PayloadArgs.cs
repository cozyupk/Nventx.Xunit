using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow
{
    /// <summary>
    /// Represents the arguments required to create a payload, including metadata and body.
    /// </summary>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    public abstract class PayloadArgs<TPayloadMeta, TPayloadBody> : IPayloadArgs<TPayloadMeta, TPayloadBody>
    {
        /// <summary>
        /// Creates a new payload instance from the current arguments.
        /// </summary>
        /// <returns>A new payload instance.</returns>
        public abstract IPayload<TPayloadMeta, TPayloadBody> NewFromSelf();
    }
}
