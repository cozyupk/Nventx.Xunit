using System;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.NotificationFlow
{
    /// <summary>
    /// Provides a base class for unicast notification flows, where a single handler
    /// receives notifications after the source is converted by injected Lambda function.
    /// </summary>
    public class UnicastProjectionFlow<TTarget, TSource> : IUnicastNotificationFlow<TTarget, TSource>
    {
        // The registered handler that receives the adapted object.
        private Action<TTarget>? Handler { get; set; }

        // Adaptation logic to convert source into target.
        protected Func<TSource, TTarget> AdaptTo { get; }

        // Synchronization object to ensure thread safety during registration and retrieval.
        private object LockObject { get; } = new();

        /// <summary>
        /// Initializes a new instance of the LambdaInjectable class with the provided adaptation function.
        /// </summary>
        public UnicastProjectionFlow(Func<TSource, TTarget> adaptTo)
        {
            AdaptTo = adaptTo ?? throw new ArgumentNullException(nameof(adaptTo), "Adaptation function cannot be null.");
        }

        /// <summary>
        /// Registers a handler to be notified when an adapted object is available.
        /// Can only be called once; subsequent calls will throw an exception.
        /// </summary>
        /// <param name="handle">The handler to register.</param>
        /// <exception cref="ArgumentNullException">If the handler is null.</exception>
        /// <exception cref="InvalidOperationException">If a handler is already registered.</exception>
        public void RegisterReceivingHandler(Action<TTarget> handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle), "Handler cannot be null.");

            lock (LockObject)
            {
                if (Handler != null)
                    throw new InvalidOperationException("Handler has already been assigned.");

                Handler = handle;
            }
        }

        /// <summary>
        /// Notifies the registered handler with an adapted version of the provided source.
        /// </summary>
        /// <param name="source">The source to adapt and pass to the handler.</param>
        /// <exception cref="ArgumentNullException">If the source is null.</exception>
        /// <exception cref="InvalidOperationException">If no handler has been registered.</exception>
        public void Notify(TSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Notification source cannot be null.");

            Action<TTarget>? handler;

            lock (LockObject)
            {
                handler = Handler;
            }

            if (handler == null)
                throw new InvalidOperationException("No handler has been registered.");

            TTarget adapted = AdaptTo(source);
            handler(adapted);
        }
    }
}