using System.Collections.Generic;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.CreationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow
{
    /// <summary>
    /// Notifies registered consumers and triggers with payloads containing sender and payload metadata.
    /// </summary>
    /// <typeparam name="TSenderMeta">The type of the sender metadata.</typeparam>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    public class PayloadNotifier<TSenderMeta, TPayloadMeta, TPayloadBody> : IPayloadNotifier<TSenderMeta, TPayloadMeta, TPayloadBody>
        where TSenderMeta : class
        where TPayloadMeta : class
    {
        private object ConsumerNotifiersLock { get; } = new object();

        /// <summary>
        /// Represents a payload that includes sender metadata along with the payload.
        /// </summary>
        protected internal class SenderPayload : ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>
        {
            public TSenderMeta SenderMeta { get; }
            public IPayload<TPayloadMeta, TPayloadBody> Payload { get; }

            public SenderPayload(TSenderMeta senderMeta, IPayload<TPayloadMeta, TPayloadBody> payload)
            {
                SenderMeta = senderMeta;
                Payload = payload;
            }
        }

        private List<ICreationNotifier<
                ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>
            >> _consumerNotifiers = new();

        /// <summary>
        /// Gets the list of consumer creation notifiers that will be notified when a sender payload is created.
        /// </summary>
        public IReadOnlyList<ICreationNotifier<
                ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>
            >> ConsumerNotifiers
        {
            get
            {
                lock (ConsumerNotifiersLock)
                {
                    return new List<ICreationNotifier<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>>>(_consumerNotifiers);
                }
            }
        }

        /// <summary>
        /// Gets the sender metadata associated with this notifier.
        /// </summary>
        private TSenderMeta SenderMeta { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayloadNotifier{TSenderMeta, TPayloadMeta, TPayloadBody}"/> class.
        /// </summary>
        /// <param name="senderMeta">The metadata of the sender.</param>
        public PayloadNotifier(TSenderMeta senderMeta)
        {
            SenderMeta = senderMeta;
        }

        /// <summary>
        /// Registers a trigger that will be notified when a payload is created.
        /// The trigger's OnObjectCreated action is set to notify all registered consumers with a sender payload.
        /// </summary>
        /// <param name="trigger">The trigger to register.</param>
        public void RegisterTrigger(ICreationNotified<IPayload<TPayloadMeta, TPayloadBody>> trigger)
        {
            // Set the action to be performed when a payload is created
            trigger.OnObjectCreated = (payload) =>
            {
                // Create a sender payload that includes sender metadata and the payload
                var senderPayload = new SenderPayload(SenderMeta, payload);
                List<ICreationNotifier<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>>> notifiersCopy;
                // Lock to ensure thread safety when accessing the notifiers list
                lock (ConsumerNotifiersLock)
                {
                    notifiersCopy = new List<ICreationNotifier<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>>>(_consumerNotifiers);
                }
                // Notify all registered consumer notifiers
                foreach (var consumerCreationNotifier in notifiersCopy)
                {
                    consumerCreationNotifier.CreateAndNotify(senderPayload);
                }
            };
        }

        /// <summary>
        /// Adds a consumer to the list of notifiers that will be notified when a sender payload is created.
        /// </summary>
        /// <param name="consumer">The consumer to add.</param>
        public void AddConsumer(IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer)
        {
            // Lock to ensure thread safety when modifying the notifiers list
            lock (ConsumerNotifiersLock)
            {
                _consumerNotifiers.Add(consumer.CreationNotifier);
            }
        }
    }
}
