using System;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.SenderPayloadFlow
{
    /// <summary>
    /// Defines a contract for a multicast notifier that can register triggers and add consumers for payload notifications.
    /// </summary>
    public interface IMulticastNotifier<TSenderMeta, TPayloadMeta, TPayloadBody>
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
        void AddConsumer(ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer);

        /// <summary>
        /// Removes a consumer from the list of payload consumers.
        /// </summary>
        /// <param name="consumer">The consumer to remove from receiving payload notifications.</param>
        void RemoveConsumer(ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer);
    }
}