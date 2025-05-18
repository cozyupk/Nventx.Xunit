using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.CreationFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Notifies registered consumers and triggers with payloads containing subject and payload metadata.
    /// </summary>
    /// <typeparam name="TSenderMeta">The type of the subject metadata.</typeparam>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    public interface IPayloadNotifier<TSenderMeta, TPayloadMeta, TPayloadBody>
    {
        /// <summary>
        /// Registers a trigger that will be notified when a payload is created.
        /// </summary>
        /// <param name="trigger">The trigger to register.</param>
        void RegisterTrigger(ICreationNotified<IPayload<TPayloadMeta, TPayloadBody>> trigger);

        /// <summary>
        /// Adds a consumer that conditionally receives payload notifications.
        /// </summary>
        /// <param name="consumer">The consumer to add.</param>
        void AddConsumer(IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer);
    }
}