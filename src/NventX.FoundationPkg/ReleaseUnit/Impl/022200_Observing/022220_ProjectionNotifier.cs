using NventX.FoundationPkg.Abstractions.Observing;
using System;

namespace NventX.FoundationPkg.Impl.Observing
{
    /// <summary>
    /// Notifies handlers by projecting input arguments to a target type before notification.
    /// </summary>
    public class ProjectionNotifier<TTarget, TArgs> : ObservableX<TTarget>, INotifyOf<TArgs>
    {
        /// <summary>
        /// The projection function that transforms the input argument to the target type.
        /// </summary>
        private Func<TArgs, TTarget> Projection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionNotifier{TArgs, TTarget}"/> class.
        /// </summary>
        /// <param name="projection"></param>
        /// <param name="notificationsRunner"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProjectionNotifier(Func<TArgs, TTarget> projection, Action<Action>? notificationsRunner = null) : base(notificationsRunner)
        {
            Projection = projection ?? throw new ArgumentNullException(nameof(projection), "Projection function cannot be null.");
        }

        public void Notify(TArgs beingNotified)
        {
            InvokeHandlers(Projection(beingNotified));
        }
    }
}
