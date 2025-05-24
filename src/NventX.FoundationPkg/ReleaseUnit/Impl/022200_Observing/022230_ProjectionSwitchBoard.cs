using NventX.FoundationPkg.Abstractions.Observing;
using System;

namespace NventX.FoundationPkg.Impl.Observing
{
    /// <summary>
    /// A projection-enabled notification relay that receives notifications of type <typeparamref name="TSource"/>,
    /// transforms them into <typeparamref name="TTarget"/> using a projection function,
    /// and notifies all registered handlers with the transformed result.
    ///
    /// This class acts as a switchboard that adapts input signals to a different type
    /// before distributing them to observers, enabling flexible and decoupled notification pipelines.
    /// </summary>
    public class ProjectionSwitchBoard<TTarget, TSource> : ObservableX<TTarget>, IHandlerOf<TSource>
    {
        /// <summary>
        /// The projection function that transforms the input argument to the target type.
        /// </summary>
        protected internal Func<TSource, TTarget> Projection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionSwitchBoard{TTarget, TSource}"/> class
        /// with a projection function that transforms source notifications into target notifications.
        /// </summary>
        /// <param name="projection">
        /// A function that projects a <typeparamref name="TSource"/> value into a <typeparamref name="TTarget"/>.
        /// </param>
        /// <param name="notificationsRunner">
        /// An optional execution context for running notification handlers (e.g., synchronously or asynchronously).
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="projection"/> is null.
        /// </exception>
        public ProjectionSwitchBoard(Func<TSource, TTarget> projection, Action<Action>? notificationsRunner = null) : base(notificationsRunner)
        {
            Projection = projection ?? throw new ArgumentNullException(nameof(projection), "Projection function cannot be null.");
        }

        /// <summary>
        /// Handles a source notification by applying the projection function
        /// and notifying all observers with the resulting target value.
        /// </summary>
        /// <param name="notified">The original notification of type <typeparamref name="TSource"/>.</param>
        public virtual void Handle(TSource notified)
        {
            InvokeHandlers(Projection(notified));
        }
    }
}
