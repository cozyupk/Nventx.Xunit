using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.ComponentRoots.Playgrounds.NotificationDemo
{
    /// <summary>
    /// Represents metadata information for a sender.
    /// </summary>
    interface ISenderMeta : IAdaptTo<ISenderMeta>
    {
        /// <summary>
        /// Gets the name of the sender.
        /// </summary>
        string SenderName { get; }
    }

    /// <summary>
    /// Provides an implementation of <see cref="ISenderMeta"/> and adapts to it.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SenderMeta"/> class with the specified sender name.
    /// </remarks>
    /// <param name="senderName">The name of the sender.</param>
    class SenderMeta(string senderName) : ISenderMeta
    {
        /// <summary>
        /// Gets the name of the sender.
        /// </summary>
        public string SenderName { get; } = senderName;

        /// <summary>
        /// Adapts this instance to <see cref="ISenderMeta"/>.
        /// This implementation is idempotent: it returns itself.
        /// </summary>
        /// <returns>This instance as <see cref="ISenderMeta"/>.</returns>
        public ISenderMeta Adapt()
        {
            return this;
        }
    }
}
