using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.NotificationFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow
{
    /// <summary>
    /// Provides a unicast notification mechanism for payloads, allowing a single handler to be notified
    /// when a payload with metadata and body is adapted and delivered. This class specializes the
    /// <see cref="UnicastAdaptationNotifier{TTarget, TSource}"/> for payload types.
    /// </summary>
    public class PayloadUnicastNotifier<TPayloadMeta, TPayloadBody>
        : UnicastAdaptationNotifier<IPayload<TPayloadMeta, TPayloadBody>, IAdaptTo<IPayload<TPayloadMeta, TPayloadBody>>>,
          IPayloadUnicastNotifier<TPayloadMeta, TPayloadBody>
        where TPayloadMeta : class
    {
    }
}
