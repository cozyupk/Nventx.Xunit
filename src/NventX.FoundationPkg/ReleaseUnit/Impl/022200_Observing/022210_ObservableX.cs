using System;
using NventX.FoundationPkg.Abstractions.Observing;

namespace NventX.FoundationPkg.Impl.Observing
{
    /*
    /// <summary>
    /// Provides a thread-safe observable pattern implementation for handling notifications to registered handlers.
    /// </summary>
    public abstract class ObservableX<TType> : IObservableX<TType>
    {
        /// <summary>
        /// Action that runs notifications. This allows for custom execution contexts (e.g., UI thread, background thread).
        /// </summary>
        private Action<Action> NotificationsRunner { get; }

        private ConcurrentSelfCleaningSet<IHandlerOf<TType>> Handlers { get; } = new();

        /// <summary>
        /// Handles exceptions that occur during notification.
        /// Override this method to provide custom exception handling logic.
        /// </summary>
        /// <param name="handler">The handler that caused the exception during notification.</param>
        /// <param name="beingNotified">The object that was being notified when the exception occurred.</param>
        /// <param name="ex">The exception that was thrown during notification.</param>
        protected internal virtual void OnExceptionInNotification(IHandlerOf<TType> handler, TType beingNotified, Exception ex)
        {
            // Handle exceptions that occur during notification
            // This can be overridden to provide custom exception handling logic
            System.Diagnostics.Debug.WriteLine($"[Warning] Exception during notification: {ex.Message}");
        }

        /// <summary>
        /// Determines whether a handler should be notified with the given output.
        /// Override to implement custom filtering logic.
        /// </summary>
        /// <param name="handler">The handler to check.</param>
        /// <param name="beingNotified">The output to be notified.</param>
        /// <returns>True if the handler should be notified; otherwise, false.</returns>
        protected internal virtual bool IsNotifiable(IHandlerOf<TType> handler, TType beingNotified)
        {
            return true;
        }

        /// <summary>
        /// Notifies all registered handlers with the specified object.
        /// </summary>
        /// <param name="beingNotified">The object to notify handlers with.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected internal virtual void InvokeHandlers(TType beingNotified)
        {
            // Check if the beingNotified is null
            if (beingNotified == null)
                throw new ArgumentNullException(nameof(beingNotified));

            // Notify all eligible handlers via the injected execution context.
            // This allows handlers to be notified synchronously, asynchronously, or with custom logic.
            NotificationsRunner(() =>
            {
                // Notify all handlers of the adapted output if they accept it
                foreach (var handler in Handlers)
                {
                    // Check whether the handler accepts the output
                    if (!IsNotifiable(handler, beingNotified))
                    {
                        continue;
                    }

                    // Notify the handler with the adapted output
                    try { handler.Handle(beingNotified); }
                    catch (Exception ex)
                    {
                        // handle exceptions that occur during notification
                        OnExceptionInNotification(handler, beingNotified, ex);
                    }
                }
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableX{TType}"/> class.
        /// Optionally accepts a custom notifications runner; if not provided, notifications are run synchronously.
        /// </summary>
        public ObservableX(Action<Action>? notificationsRunner = null)
        {
            NotificationsRunner = notificationsRunner ?? (a => a());
        }

        /// <summary>
        /// Enables observation by the specified handlers by adding them to the internal handler set.
        /// </summary>
        public void EnableObservationBy(params IHandlerOf<TType>[] handlers)
        {
            Handlers.AddElements(handlers);
        }

        /// <summary>
        /// Disables observation by removing the specified handlers from the internal handler set.
        /// </summary>
        public void DisableObservationBy(params IHandlerOf<TType>[] handlers)
        {
            Handlers.RemoveElements(handlers);
        }
    }
    */
}
