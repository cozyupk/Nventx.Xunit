using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Represents a consumer that receives payload notifications.
    /// </summary>
    public interface IPayloadConsumer<in TSenderMeta, in TPayloadMeta, in TPayloadBody>
    {
        /// <summary>
        /// Gets the notifier responsible for handling arrival of a payload notification.
        /// This notifier is expected to follow unicast semantics—i.e., only a single handler is supported.
        /// </summary>
        IUnicastNotifier<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>> PayloadArrivalNotifier { get; }
    }
}