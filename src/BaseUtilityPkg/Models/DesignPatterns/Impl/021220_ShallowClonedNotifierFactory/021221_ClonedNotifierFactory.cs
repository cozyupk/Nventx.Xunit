using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.Traits;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.ShallowClonedNotifierFactory
{
    /// <summary>
    /// Factory for creating <see cref="ShallowCloned{TCreationArgs}"/> instances and invoking a handler on creation.
    /// </summary>
    /// <typeparam name="TSource">Type of the creation arguments.</typeparam>
    public class ClonedNotifierFactory<TSource>
        where TSource : class, IShallowClonable<TSource>
    {
        /// <summary>
        /// Handler to be invoked when a new object is created.
        /// </summary>
        private Action<ShallowCloned<TSource>>? _onObjectCreated;

        /// <summary>
        /// Thread-safe lock object for synchronizing access to the handler.
        /// </summary>
        private object LockObject { get; } = new();

        /// <summary>
        /// Sets the handler to be invoked when an object is created.
        /// Can only be set once.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the handler is already set.</exception>
        public Action<ShallowCloned<TSource>> OnObjectCreated
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
        /// Creates a new <see cref="ShallowCloned{TCreationArgs}"/> and invokes the handler if set.
        /// </summary>
        /// <param name="args">The creation arguments to use.</param>
        public void Create(TSource args)
        {
            // Validate input arguments
            if (args == null)
                throw new ArgumentNullException(nameof(args), "Creation arguments cannot be null.");

            Action<ShallowCloned<TSource>>? handler;
            // Retrieve the handler in a thread-safe manner
            lock (LockObject)
            {
                handler = _onObjectCreated;
            }

            // Ensure a handler is registered before proceeding
            if (handler == null)
                throw new ArgumentNullException(nameof(_onObjectCreated), "No handler is registered for object creation.");

            // Create a shallow-cloned object and invoke the handler
            var obj = new ShallowCloned<TSource>(args);
            handler(obj);
        }
    }
}
