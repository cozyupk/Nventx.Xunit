namespace NventX.FoundationPkg.Abstractions.Observing
{
    /// <summary>
    /// Represents an entity that can notify handlers of type <typeparamref name="TType"/>.
    /// </summary>
    public interface INotifyOf<in TType>
    {
        /// <summary>
        /// Notifies all registered handlers with the specified object.
        /// </summary>
        /// <param name="beingNotified">The object to notify handlers with.</param>
        void Notify(TType beingNotified);
    }
}
