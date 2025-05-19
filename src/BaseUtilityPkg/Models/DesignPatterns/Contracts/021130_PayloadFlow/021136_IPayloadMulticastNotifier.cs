using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Defines a contract for a multicast notifier that can register triggers and add consumers for payload notifications.
    /// </summary>
    /// <typeparam name="TSenderMeta">The type of the sender metadata.</typeparam>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    public interface IPayloadMulticastNotifier<TSenderMeta, TPayloadMeta, TPayloadBody>
    {
        /// <summary>
        /// Registers a handler that will be notified when a payload is emitted.
        /// </summary>
        /// <param name="notificationHandler">The handler to register for payload notification.</param>
        void RegisterHandler(INotificationHandler<IPayload<TPayloadMeta, TPayloadBody>> notificationHandler);

        /// <summary>
        /// Adds a consumer that will receive payload notifications.
        /// </summary>
        void AddConsumer(IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer);
    }
}