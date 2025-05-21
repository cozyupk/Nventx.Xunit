using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.SenderPayloadFlow
{
    /// <summary>
    /// Represents a consumer that receives payload notifications.
    /// </summary>
    public interface ISenderPayloadConsumer<in TSenderMeta, in TPayloadMeta, in TPayloadBody>
    {
        /// <summary>
        /// Gets the notifier responsible for handling arrival of a payload notification.
        /// This notifier is expected to follow unicast semantics—i.e., only a single handler is supported.
        /// </summary>
        IUnicastNotifier<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>> SenderPayloadArrivalNotifier { get; }
    }
}