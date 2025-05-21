using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.NotificationFlow;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.PayloadFlow
{
    /// <summary>
    /// Provides a unicast notification mechanism for payloads, allowing a single handler to be notified
    /// when a payload with metadata and body is adapted and delivered. This class specializes the
    /// <see cref="UnicastNotificationFlow{TTarget, TSource}"/> for payload types.
    /// </summary>
    public class PayloadUnicastNotifier<TPayloadMeta, TPayloadBody>
        : UnicastNotificationFlow<IPayload<TPayloadMeta, TPayloadBody>, IAdaptTo<IPayload<TPayloadMeta, TPayloadBody>>>,
          IPayloadUnicastNotificationFlow<TPayloadMeta, TPayloadBody>
        where TPayloadMeta : class
    {
    }
}
