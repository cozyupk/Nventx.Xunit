namespace NventX.FoundationPkg.Abstractions.Observing
{
    /// <summary>
    /// Represents a handler that processes notifications of type <typeparamref name="TType"/>.
    /// </summary>
    public interface IHandlerOf<in TType>
    {
        /// <summary>
        /// Handles a notification of the specified type.
        /// </summary>
        /// <param name="type">The notification object.</param>
        void Handle(TType type);
    }
}
