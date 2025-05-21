using System.Collections.Generic;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.PayloadFlow
{
    /// <summary>
    /// Builder for constructing a multicast notifier that can notify multiple payload consumers.
    /// </summary>
    /// <typeparam name="TSenderMeta">The type of the sender metadata.</typeparam>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    public class PayloadMulticastNotifierBuilder<TSenderMeta, TPayloadMeta, TPayloadBody>
        : IPayloadMulticastNotifierBuilder<TSenderMeta, TPayloadMeta, TPayloadBody>
        where TSenderMeta : class
        where TPayloadMeta : class
    {
        /// <summary>
        /// List of registered payload consumers.
        /// </summary>
        private List<ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>> Consumers { get; } = new();

        /// <summary>
        /// Lock object for thread-safe access to the Consumers list.
        /// </summary>
        private object ConsumersLock { get; } = new object();

        /// <summary>
        /// Adds a payload consumer to the builder in a thread-safe manner.
        /// </summary>
        /// <param name="consumer">The consumer to add.</param>
        public void AddConsumer(ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer)
        {
            lock (ConsumersLock)
            {
                Consumers.Add(consumer);
            }
        }

        /// <summary>
        /// Builds and returns a multicast notifier for the specified sender metadata.
        /// All registered consumers are attached to the notifier.
        /// </summary>
        /// <param name="senderMeta">The sender metadata to associate with the notifier.</param>
        /// <returns>A multicast notifier instance with all consumers attached.</returns>
        public IPayloadMulticastNotifier<TSenderMeta, TPayloadMeta, TPayloadBody> Build(TSenderMeta senderMeta)
        {
            // Create a new PayloadNotifier instance with the provided sender metadata.
            var notifier = new PayloadMulticastNotifier<TSenderMeta, TPayloadMeta, TPayloadBody>(senderMeta);

            // Prepare a copy of the current consumers list in a thread-safe manner.
            List<ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>> consumersCopy;
            lock (ConsumersLock)
            {
                consumersCopy = new List<ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>>(Consumers);
            }

            // Add each registered consumer to the notifier.
            foreach (var consumer in consumersCopy)
            {
                notifier.AddConsumer(consumer);
            }

            // Return the fully constructed notifier with all consumers attached.
            return notifier;
        }
    }
}