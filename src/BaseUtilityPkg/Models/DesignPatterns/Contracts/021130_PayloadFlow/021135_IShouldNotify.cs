namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow
{
    /// <summary>
    /// Defines a contract for determining whether a notification should be sent
    /// based on sender and payload metadata.
    /// </summary>
    /// <typeparam name="TSenderMeta">Type of the sender metadata.</typeparam>
    /// <typeparam name="TPayloadMeta">Type of the payload metadata.</typeparam>
    public interface IShouldNotify<in TSenderMeta, in TPayloadMeta>
    {
        /// <summary>
        /// Determines whether a notification should be sent for the given sender and payload metadata.
        /// </summary>
        /// <param name="senderMeta">Metadata about the sender.</param>
        /// <param name="payloadMeta">Metadata about the payload.</param>
        /// <returns>True if notification should be sent; otherwise, false.</returns>
        bool ShouldNotify(TSenderMeta senderMeta, TPayloadMeta payloadMeta);
    }
}