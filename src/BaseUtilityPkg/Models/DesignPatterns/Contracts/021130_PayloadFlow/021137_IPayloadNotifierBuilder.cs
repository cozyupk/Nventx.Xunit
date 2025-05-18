namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Builder interface for constructing a payload notifier with registered consumers.
    /// </summary>
    /// <typeparam name="TSubjectMeta">The type of the subject metadata.</typeparam>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    public interface IPayloadNotifierBuilder<TSubjectMeta, TPayloadMeta, TPayloadBody>
    {
        /// <summary>
        /// Adds a consumer that conditionally receives payload notifications.
        /// </summary>
        /// <param name="consumer">The consumer to add.</param>
        void AddConsumer(IPayloadConsumer<TSubjectMeta, TPayloadMeta, TPayloadBody> consumer);

        /// <summary>
        /// Builds and returns a payload notifier for the specified subject metadata.
        /// </summary>
        /// <param name="subjectMeta">The subject metadata.</param>
        /// <returns>A payload notifier instance.</returns>
        IPayloadNotifier<TSubjectMeta, TPayloadMeta, TPayloadBody> Build(TSubjectMeta subjectMeta);
    }
}