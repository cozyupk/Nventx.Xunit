namespace NventX.FoundationPkg.Abstractions.Observing
{
    /// <summary>
    /// Represents an observable entity that allows handlers to subscribe or unsubscribe for notifications of type <typeparamref name="TType"/>.
    /// </summary>
    public interface IObservableX<out TType>
    {
        /// <summary>
        /// Enables observation by the specified handlers.
        /// </summary>
        /// <param name="handlers">Handlers to be added as observers.</param>
        void EnableObservationBy(params IHandlerOf<TType>[] handlers);

        /// <summary>
        /// Disables observation by the specified handlers.
        /// </summary>
        /// <param name="handlers">Handlers to be removed from observers.</param>
        void DisableObservationBy(params IHandlerOf<TType>[] handlers);
    }
}
