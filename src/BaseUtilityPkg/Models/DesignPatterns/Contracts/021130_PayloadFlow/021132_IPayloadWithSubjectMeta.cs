namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Represents a payload that includes subject metadata along with the payload metadata and body.
    /// </summary>
    public interface IPayloadWithSubjectMeta<out TSubjectMeta, out TPayloadMeta, out TPayloadBody>
    {
        /// <summary>
        /// Gets the metadata associated with the subject.
        /// </summary>
        TSubjectMeta SubjectMeta { get; }

        /// <summary>
        /// Gets the payload containing metadata and body.
        /// </summary>
        IPayload<TPayloadMeta, TPayloadBody> Payload { get; }
    }
}