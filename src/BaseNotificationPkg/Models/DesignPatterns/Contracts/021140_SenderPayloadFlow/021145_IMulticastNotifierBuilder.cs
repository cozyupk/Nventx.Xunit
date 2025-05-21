namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.SenderPayloadFlow
{
    /// <summary>
    /// Defines a builder for constructing a multicast notifier that can notify multiple payload consumers.
    /// </summary>
    /// <typeparam name="TSenderMeta">The type of the subject metadata.</typeparam>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    public interface IMulticastNotifierBuilder<TSenderMeta, TPayloadMeta, TPayloadBody>
    {
        /// <summary>
        /// Adds a payload consumer to the builder.
        /// </summary>
        /// <param name="consumer">The consumer to add.</param>
        void AddConsumer(ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> consumer);

        /// <summary>
        /// Builds and returns a multicast notifier for the specified subject metadata.
        /// </summary>
        /// <param name="subjectMeta">The subject metadata to associate with the notifier.</param>
        /// <returns>A multicast notifier instance.</returns>
        IMulticastNotifier<TSenderMeta, TPayloadMeta, TPayloadBody> Build(TSenderMeta subjectMeta);
    }
}