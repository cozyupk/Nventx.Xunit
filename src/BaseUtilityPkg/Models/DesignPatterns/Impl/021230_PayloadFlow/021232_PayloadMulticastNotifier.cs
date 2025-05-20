using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;

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

        private ConcurrentDictionary<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>, byte> Consumers { get; } = new();

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
        /// Returns a thread-safe copy of the current payload consumers, removing any that indicate they should be removed.
        /// </summary>
        private IEnumerable<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>> GetLatestConsumersCppy()
        {
            List<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>> consumersCopy;

            // Lock to ensure thread-safe access to the consumer list
            lock (ConsumerNotifiersLock)
            {
                consumersCopy = new List<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>>();
                foreach (var consumer in Consumers.Keys)
                {
                    // Remove consumers that indicate they should be removed
                    if (consumer is ISelfRemovable selfRemovable && selfRemovable.CanRemove())
                    {
                        Consumers.TryRemove(consumer, out _);
                        continue;
                    }
                    consumersCopy.Add(consumer);
                }
            }

            return consumersCopy;
        }

        /// <summary>
        /// Registers a handler that will be invoked when a payload is emitted.
        /// The handler is set to the notificationHandler, and when triggered, it notifies all registered consumers.
        /// Supports optional execution context via the runner parameter.
        /// </summary>
        /// <param name="notificationHandler">The handler to register for payload notification.</param>
        /// <param name="runner">
        /// Optional action to control the execution context of the notification (e.g., for async or thread dispatch).
        /// If null, notifications are invoked synchronously.
        /// </param>
        public void RegisterHandler(
            INotificationHandler<IPayload<TPayloadMeta, TPayloadBody>> notificationHandler,
            Action<Action>? runner = null)
        {
            if (notificationHandler == null)
                throw new ArgumentNullException(nameof(notificationHandler));

            // Use synchronous invocation if no runner is provided
            runner ??= a => a();

            // Assign the handler to be called when a payload is received
            notificationHandler.Handle = payload =>
            {
                // Wrap the payload with sender metadata
                var senderPayload = new SenderPayload(SenderMeta, payload);

                // Get a thread-safe copy of the current consumers
                var consumersCopy = GetLatestConsumersCppy();

                // Notify all eligible consumers via the injected execution context.
                // This allows consumers to be notified synchronously, asynchronously, or with custom logic.
                runner(() =>
                {
                    foreach (var consumer in consumersCopy)
                    {
                        // Skip notification if the consumer's condition is not met
                        if (consumer is IShouldNotify<TSenderMeta, TPayloadMeta> conditionalNotified
                            && !conditionalNotified.ShouldNotify(SenderMeta, payload.Meta))
                        {
                            continue;
                        }

                        // Notify the consumer of the payload arrival
                        consumer.PayloadArrivalNotifier.Notify(senderPayload);
                    }
                });
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
                Consumers.TryAdd(consumer,byte.MinValue);
            }
        }

        /// <summary>
        /// Removes a consumer from the list of payload consumers.
        /// </summary>
        /// <param name="consumer">The consumer to remove from receiving payload notifications.</param>

        public void RemoveConsumer(IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer)
        {
            Consumers.TryRemove(consumer, out _);
        }

        /// <summary>
        /// Returns a thread-safe copy of the current payload consumers.
        /// </summary>
        protected internal IEnumerable<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>> GetConsumers()
        {
            return GetLatestConsumersCppy();
        }

        /// <summary>
        /// Clears all registered payload consumers in a thread-safe manner.
        /// </summary>
        protected internal void ClearConsumers()
        {
            // Lock to ensure thread safety when modifying the consumers list
            lock (ConsumerNotifiersLock)
            {
                Consumers.Clear();
            }
        }
    }
}
