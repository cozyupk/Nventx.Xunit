using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    public interface IPayloadUnicastNotifier<TPayloadMeta, TPayloadBody>
        : INotificationFlow<IPayload<TPayloadMeta, TPayloadBody>, IAdaptTo<IPayload<TPayloadMeta, TPayloadBody>>>
    {
    }
}
