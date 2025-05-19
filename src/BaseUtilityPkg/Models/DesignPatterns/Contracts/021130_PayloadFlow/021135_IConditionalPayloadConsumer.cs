namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Represents a consumer that conditionally receives payload notifications based on subject and payload metadata.
    /// </summary>
    /// <remarks>
    /// This method should be side-effect free.
    /// </remarks>
    public interface IConditionalPayloadConsumer<in TSubjectMeta, in TPayloadMeta, in TPayloadBody>
        : IPayloadConsumer<TSubjectMeta, TPayloadMeta, TPayloadBody>
    {
        /// <summary>
        /// Determines whether notification is needed for the given subject and payload metadata.
        /// </summary>
        /// <param name="subjectMeta">The metadata of the subject.</param>
        /// <param name="payloadMeta">The metadata of the payload.</param>
        /// <returns>True if notification is needed; otherwise, false.</returns>
        bool IsNotifyNeeded(TSubjectMeta subjectMeta, TPayloadMeta payloadMeta);
    }
}