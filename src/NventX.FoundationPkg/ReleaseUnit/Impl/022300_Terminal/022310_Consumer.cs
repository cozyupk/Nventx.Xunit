using NventX.FoundationPkg.Abstractions.Observing;
using System;

namespace NventX.FoundationPkg.Impl.Simple
{
    /// <summary>
    /// A generic notification consumer that invokes a specified <see cref="Action{T}"/> for each received notification.
    /// 
    /// This class is a lightweight implementation of <see cref="IHandlerOf{TType}"/>, and is useful
    /// for plugging ad-hoc logic into notification pipelines.
    /// </summary>
    public class Consumer<TType> : IHandlerOf<TType>
    {
        /// <summary>
        /// The action to be executed when a notification is received.
        /// </summary>
        protected internal Action<TType> Handler { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Consumer{TType}"/> class with the specified action.
        /// </summary>
        /// <param name="handler">The action to execute when a notification of type <typeparamref name="TType"/> is received.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is null.</exception>
        public Consumer(Action<TType> handler)
        {
            Handler = handler ?? throw new ArgumentNullException(nameof(handler), "Handler function cannot be null."); ;
        }

        /// <summary>
        /// Executes the configured action with the given notification payload.
        /// </summary>
        /// <param name="notified">The notification data to handle.</param>
        public virtual void Handle(TType notified)
        {
            Handler(notified);
        }
    }
}
