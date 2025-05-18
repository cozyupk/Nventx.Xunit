using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.CreationFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Represents a consumer that receives payload notifications.
    /// </summary>
    public interface IPayloadConsumer<in TSenderMeta, in TPayloadMeta, in TPayloadBody>
    {
        /// <summary>
        /// Gets the creation notifier for payloads with subject metadata.
        /// </summary>
        ICreationNotifier<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>> CreationNotifier { get; }
    }
}