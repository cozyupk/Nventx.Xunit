using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;

namespace NventX
{
    /// <summary>
    /// Represents a handler that processes notifications of type <typeparamref name="TType"/>.
    /// </summary>
    public interface IHandlerOf<TType>
    {
        /// <summary>
        /// Handles a notification of the specified type.
        /// </summary>
        /// <param name="type">The notification object.</param>
        void Handle(TType type);
    }

    /// <summary>
    /// Represents an observable entity that allows handlers to subscribe or unsubscribe for notifications of type <typeparamref name="TType"/>.
    /// </summary>
    public interface IObservableX<TType>
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

    /// <summary>
    /// Represents an entity that can notify handlers of type <typeparamref name="TType"/>.
    /// </summary>
    public interface INotifiable<TType>
    {
        /// <summary>
        /// Notifies all registered handlers with the specified object.
        /// </summary>
        /// <param name="beingNotified">The object to notify handlers with.</param>
        void Notify(TType beingNotified);
    }

    /// <summary>
    /// Provides a thread-safe observable pattern implementation for handling notifications to registered handlers.
    /// </summary>
    public abstract class ObservableX<TType> : IObservableX<TType>
    {
        /// <summary>
        /// Thread-safe dictionary holding the current set of handlers.
        /// </summary>
        private ConcurrentDictionary<IHandlerOf<TType>, byte> Handlers { get; } = new();

        /// <summary>
        /// Lock object for synchronizing access to the handlers collection.
        /// </summary>
        private object HandlersLock { get; } = new object();

        /// <summary>
        /// Action that runs notifications. This allows for custom execution contexts (e.g., UI thread, background thread).
        /// </summary>
        private Action<Action> NotificationsRunner { get; }

        /// <summary>
        /// Returns a thread-safe copy of the current handlers, removing any that indicate they should be removed.
        /// </summary>
        /// <returns>A copy of the current handlers.</returns>
        protected internal IEnumerable<IHandlerOf<TType>> GetLatestHandlersSnapshot()
        {
            List<IHandlerOf<TType>> snapshot = new();

            // Lock to ensure thread-safe access to the handler list
            lock (HandlersLock)
            {
                foreach (var receiver in Handlers.Keys)
                {
                    // Remove handlers that indicate they should be removed
                    if (receiver is ISelfRemovable selfRemovable && selfRemovable.CanRemove())
                    {
                        Handlers.TryRemove(receiver, out _);
                        continue;
                    }
                    snapshot.Add(receiver);
                }
            }

            return snapshot;
        }

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

            // Get a thread-safe copy of the current handlers
            var handlersCopy = GetLatestHandlersSnapshot();

            // Notify all eligible handlers via the injected execution context.
            // This allows handlers to be notified synchronously, asynchronously, or with custom logic.
            NotificationsRunner(() =>
            {
                // Notify all handlers of the adapted output if they accept it
                foreach (var handler in handlersCopy)
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
        /// Enables observation by the specified handlers.
        /// </summary>
        /// <param name="handlers">Handlers to be added as observers.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void EnableObservationBy(params IHandlerOf<TType>[] handlers)
        {
            lock (HandlersLock)
            {
                if (handlers == null)
                    throw new ArgumentNullException(nameof(handlers), "handlers cannot be null.");

                foreach (var handler in handlers)
                {
                    if (handler == null)
                        throw new ArgumentNullException(nameof(handlers), "handlers contains a null receiver.");
                    // Add the handler to the dictionary
                    Handlers.TryAdd(handler, byte.MinValue);
                }
            }
        }

        /// <summary>
        /// Disables observation by the specified handlers.
        /// </summary>
        /// <param name="handlers">Handlers to be removed from observers.</param>
        public void DisableObservationBy(params IHandlerOf<TType>[] handlers)
        {
            lock (HandlersLock)
            {
                foreach (var handler in handlers)
                {
                    if (handler == null)
                        continue;

                    // Remove the handler from the dictionary
                    Handlers.TryRemove(handler, out _);
                }
            }
        }
    }

    /// <summary>
    /// Notifies handlers by projecting input arguments to a target type before notification.
    /// </summary>
    public class ProjectionNotifier<TArgs, TTarget> : ObservableX<TTarget>, INotifiable<TArgs>
    {
        private Func<TArgs, TTarget> Projection { get; }

        public ProjectionNotifier(Func<TArgs, TTarget> projection)
        {
            Projection = projection;
        }

        public void Notify(TArgs beingNotified)
        {
            InvokeHandlers(Projection(beingNotified));
        }
    }

    /// <summary>
    /// Notifies handlers with the same type as the input argument.
    /// </summary>
    public class Notifier<TType> : ProjectionNotifier<TType, TType>
    {
        public Notifier() : base(source => source)
        {
        }
    }

    /// <summary>
    /// Notifies handlers by adapting input arguments to a target type using a projection function.
    /// </summary>
    public class AdaptionNotifier<TArgs, TTarget> : ProjectionNotifier<TArgs, TTarget>
        where TTarget : class
        where TArgs : IAdaptTo<TTarget>
    {
        public AdaptionNotifier(Func<TArgs, TTarget> projection) : base(projection)
        {
        }
    }

    /// <summary>
    /// Consumes notifications by executing a provided action for each notification.
    /// </summary>
    public class Consumer<TType> : IHandlerOf<TType>
    {
        private Action<TType> Handle { get; }

        public Consumer(Action<TType> handle)
        {
            Handle = handle;
        }

        void IHandlerOf<TType>.Handle(TType notified)
        {
            Handle(notified);
        }
    }

    /// <summary>
    /// Receives notifications of a source type, projects them to a target type, and notifies registered handlers.
    /// </summary>
    public class ProjectionSwitchBoard<TTarget, TSource> : ObservableX<TTarget>, IHandlerOf<TSource>
    {
        private Action<TSource> Handle { get; }

        public ProjectionSwitchBoard(Func<TSource, TTarget> projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException(nameof(projection), "Projection function cannot be null.");
            }
            Handle = source =>
            {
                InvokeHandlers(projection(source));
            };
        }

        void IHandlerOf<TSource>.Handle(TSource notified)
        {
            Handle(notified);
        }
    }

    /// <summary>
    /// Receives and notifies handlers with the same type as the input argument.
    /// </summary>
    public class SwitchBoard<TType> : ProjectionSwitchBoard<TType, TType>
    {
        public SwitchBoard() : base(source => source)
        {
            // No additional initialization needed
        }
    }

    /// <summary>
    /// Receives notifications of a source type, adapts them to a target type, and notifies registered handlers.
    /// </summary>
    public class AdaptionSwitchBoard<TTarget, TArgs> : ProjectionSwitchBoard<TTarget, TArgs>
        where TTarget : class
        where TArgs : IAdaptTo<TTarget>
    {
        public AdaptionSwitchBoard() : base(source => source.Adapt())
        {
            // No additional initialization needed
        }
    }
}
