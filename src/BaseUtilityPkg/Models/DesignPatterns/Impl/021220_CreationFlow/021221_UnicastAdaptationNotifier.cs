using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.NotificationFlow
{
    /// <summary>
    /// Represents a unicast notification flow that adapts an input source to a target type
    /// and notifies a single registered handler upon adaptation.
    /// The target object is not returned to the caller, preserving strict isolation.
    /// </summary>
    public class UnicastAdaptationNotifier<TTarget, TSource> : INotificationFlow<TTarget, TSource>
        where TTarget : class
        where TSource : class, IAdaptTo<TTarget>
    {
        /// <summary>
        /// Handler to be invoked when a new object is created.
        /// </summary>
        private Action<TTarget>? Handler { get; set; }

        /// <summary>
        /// Thread-safe lock object for synchronizing access to the handler.
        /// </summary>
        private object LockObject { get; } = new();

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Thrown if the handler is already set.</exception>
        public Action<TTarget> Handle
        {
            set
            {
                // Ensure thread-safety when setting the handler
                lock (LockObject)
                {
                    // Prevent multiple assignments of the handler
                    if (Handler != null)
                        throw new InvalidOperationException("Handler has already been assigned.");

                    // Handler must not be null
                    Handler = value ?? throw new ArgumentNullException(nameof(value), "Handler cannot be null.");
                }
            }
        }

        /// <inheritdoc/>
        public void Notify(TSource source)
        {
            // Validate input arguments
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Notification source cannot be null.");

            Action<TTarget>? handler;

            // Retrieve the handler in a thread-safe manner
            lock (LockObject)
            {
                handler = Handler;
            }

            // Ensure a handler is registered before proceeding
            if (handler == null)
                throw new ArgumentNullException(nameof(Handler), "No handler has been registered.");

            // Adapt the source to the target type and invoke the handler
            var adapted = source.Adapt();
            handler(adapted);
        }
    }
}
