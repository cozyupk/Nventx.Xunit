using System.Collections.Generic;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow
{
    /// <summary>
    /// Provides multicast notification functionality for payloads, allowing multiple consumers to be notified
    /// when a payload is triggered. Supports conditional notification for consumers that implement
    /// IConditionalPayloadConsumer.
    /// </summary>
    /// <typeparam name="TSenderMeta">Type of sender metadata.</typeparam>
    /// <typeparam name="TPayloadMeta">Type of payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">Type of payload body.</typeparam>
    public class PayloadMulticastNotifier<TSenderMeta, TPayloadMeta, TPayloadBody> : IPayloadMulticastNotifier<TSenderMeta, TPayloadMeta, TPayloadBody>
        where TSenderMeta : class
        where TPayloadMeta : class
    {
        /// <summary>
        /// Lock object for thread-safe access to the Consumers list.
        /// </summary>
        private object ConsumerNotifiersLock { get; } = new object();

        /// <summary>
        /// Represents a payload with sender metadata.
        /// </summary>
        protected internal class SenderPayload : ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>
        {
            /// <summary>
            /// Gets the sender metadata.
            /// </summary>
            public TSenderMeta SenderMeta { get; }

            /// <summary>
            /// Gets the payload.
            /// </summary>
            public IPayload<TPayloadMeta, TPayloadBody> Payload { get; }

            /// <summary>
            /// Initializes a new instance of the SenderPayload class.
            /// </summary>
            /// <param name="senderMeta">Sender metadata.</param>
            /// <param name="payload">Payload instance.</param>
            public SenderPayload(TSenderMeta senderMeta, IPayload<TPayloadMeta, TPayloadBody> payload)
            {
                SenderMeta = senderMeta;
                Payload = payload;
            }
        }

        /// <summary>
        /// List of registered payload consumers.
        /// </summary>
        private List<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>> Consumers { get; } = new();

        /// <summary>
        /// Gets the sender metadata associated with this notifier.
        /// </summary>
        private TSenderMeta SenderMeta { get; }

        /// <summary>
        /// Initializes a new instance of the PayloadMulticastNotifier class.
        /// </summary>
        /// <param name="senderMeta">Sender metadata.</param>
        public PayloadMulticastNotifier(TSenderMeta senderMeta)
        {
            SenderMeta = senderMeta;
        }

        /// <summary>
        /// Registers a trigger that will notify all registered consumers when a payload is created.
        /// </summary>
        /// <param name="notificationHandler">The notification handler to register.</param>
        public void RegisterHandler(INotificationHandler<IPayload<TPayloadMeta, TPayloadBody>> notificationHandler)
        {
            // Set the action to be performed when a payload is created
            notificationHandler.Handle = (payload) =>
            {
                // Create a sender payload that includes sender metadata and the payload
                var senderPayload = new SenderPayload(SenderMeta, payload);

                // Make a copy of the current list of consumers to ensure thread safety during notification
                List<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>> consumersCopy;

                // Lock to ensure thread safety when accessing the consumers list
                lock (ConsumerNotifiersLock)
                {
                    consumersCopy = new List<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>>(Consumers);
                }

                // Notify all registered consumer notifiers
                foreach (var consumer in consumersCopy)
                {
                    // If the consumer is conditional and notification is not needed, skip
                    if (consumer is IConditionalPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consitionalConsumer
                        && !consitionalConsumer.IsNotifyNeeded(SenderMeta, payload.Meta))
                    {
                        continue;
                    }
                    // Notify the consumer of the new payload
                    consumer.PayloadArrivalNotifier.Notify(senderPayload);
                }
            };
        }

        /// <summary>
        /// Adds a consumer to the list of payload consumers.
        /// </summary>
        /// <param name="consumer">The payload consumer to add.</param>
        public void AddConsumer(IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer)
        {
            // Lock to ensure thread safety when modifying the consumers list
            lock (ConsumerNotifiersLock)
            {
                Consumers.Add(consumer);
            }
        }
    }
}
