using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow
{
    public interface IPayloadUnicastNotificationFlow<TPayloadMeta, TPayloadBody>
        : IUnicastNotificationFlow<IPayload<TPayloadMeta, TPayloadBody>, IAdaptTo<IPayload<TPayloadMeta, TPayloadBody>>>
    {
    }
}
