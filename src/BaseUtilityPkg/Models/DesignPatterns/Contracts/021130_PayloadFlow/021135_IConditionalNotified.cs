namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Defines a contract for determining whether a notification is needed based on subject and payload metadata.
    /// </summary>
    /// <typeparam name="TSubjectMeta">The type of the subject metadata.</typeparam>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    public interface IConditionalNotified<in TSubjectMeta, in TPayloadMeta>
    {
        /// <summary>
        /// Determines whether notification is needed for the given subject and payload metadata.
        /// </summary>
        /// <param name="subjectMeta">The subject metadata.</param>
        /// <param name="payloadMeta">The payload metadata.</param>
        /// <returns>True if notification is needed; otherwise, false.</returns>
        bool IsNotifyNeeded(TSubjectMeta subjectMeta, TPayloadMeta payloadMeta);
    }
}