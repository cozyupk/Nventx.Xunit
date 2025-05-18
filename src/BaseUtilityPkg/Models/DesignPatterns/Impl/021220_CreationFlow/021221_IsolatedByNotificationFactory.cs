using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.CreationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.CreationFlow
{
    /// <summary>
    /// A factory that constructs an object and notifies a registered handler upon completion.
    /// The created object is never returned to the caller, preserving strict isolation.
    /// Thread-safe and enforces single-assignment of the notification handler.
    /// </summary>
    public class IsolatedByNotificationFactory<TTarget, TSource> : ICreationFlow<TTarget, TSource>
        where TTarget : class
        where TSource : class, ISelfNewable<TTarget>
    {
        /// <summary>
        /// Handler to be invoked when a new object is created.
        /// </summary>
        private Action<TTarget>? _onObjectCreated;

        /// <summary>
        /// Thread-safe lock object for synchronizing access to the handler.
        /// </summary>
        private object LockObject { get; } = new();

        /// <summary>
        /// Sets the handler to be invoked when an object is created.
        /// Can only be set once.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the handler is already set.</exception>
        public Action<TTarget> OnObjectCreated
        {
            set
            {
                // Ensure thread-safety when setting the handler
                lock (LockObject)
                {
                    // Prevent multiple assignments of the handler
                    if (_onObjectCreated != null)
                        throw new InvalidOperationException("Handler already set.");

                    // Handler must not be null
                    _onObjectCreated = value ?? throw new ArgumentNullException(nameof(value), "Handler cannot be null.");
                }
            }
        }

        /// <summary>
        /// Creates a new shallow-cloned object from the provided arguments and invokes the handler if set.
        /// </summary>
        /// <param name="args">The creation arguments to use.</param>
        public void CreateAndNotify(TSource args)
        {
            // Validate input arguments
            if (args == null)
                throw new ArgumentNullException(nameof(args), "Creation arguments cannot be null.");

            Action<TTarget>? handler;

            // Retrieve the handler in a thread-safe manner
            lock (LockObject)
            {
                handler = _onObjectCreated;
            }

            // Ensure a handler is registered before proceeding
            if (handler == null)
                throw new ArgumentNullException(nameof(_onObjectCreated), "No handler is registered for object creation.");

            // Create a shallow-cloned object and invoke the handler
            var obj = args.NewFromSelf();
            handler(obj);
        }
    }
}
