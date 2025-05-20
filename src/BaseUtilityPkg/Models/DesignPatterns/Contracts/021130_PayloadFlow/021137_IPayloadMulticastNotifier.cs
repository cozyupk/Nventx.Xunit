using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Defines a contract for a multicast notifier that can register triggers and add consumers for payload notifications.
    /// </summary>
    public interface IPayloadMulticastNotifier<TSenderMeta, TPayloadMeta, TPayloadBody>
    {
        /// <summary>
        /// Registers a handler that will be notified when a payload is emitted.
        /// </summary>
        /// <param name="notificationHandler">The handler to register for payload notification.</param>
        /// <param name="runner">An optional runner to control the execution context of the handler (e.g., for scheduling or threading).</param>
        void RegisterHandler(INotificationHandler<IPayload<TPayloadMeta, TPayloadBody>> notificationHandler, Action<Action>? runner = null);

        /// <summary>
        /// Adds a consumer that will receive payload notifications.
        /// </summary>
        /// <param name="consumer">The consumer to add for receiving payload notifications.</param>
        void AddConsumer(IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer);

        /// <summary>
        /// Removes a consumer from the list of payload consumers.
        /// </summary>
        /// <param name="consumer">The consumer to remove from receiving payload notifications.</param>
        void RemoveConsumer(IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer);
    }
}