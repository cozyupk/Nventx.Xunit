using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.CreationFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow
{
    /// <summary>
    /// Factory for isolating and notifying payload instances generated from payload arguments.
    /// </summary>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    public class PayloadFactory<TPayloadMeta, TPayloadBody>
        : IsolatedByNotificationFactory<IPayload<TPayloadMeta, TPayloadBody>, IPayloadArgs<TPayloadMeta, TPayloadBody>>
        where TPayloadMeta : class
    {
    }
}
