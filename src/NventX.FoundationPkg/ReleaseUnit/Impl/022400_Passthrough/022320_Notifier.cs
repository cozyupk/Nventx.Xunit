using System;
using NventX.FoundationPkg.Impl.Observing;

namespace NventX.FoundationPkg.Impl.Passthrough
{
    /// <summary>
    /// Notifies handlers with the same type as the input argument.
    /// </summary>
    public class Notifier<TType> : ProjectionNotifier<TType, TType>
    {
        /// <summary>
        /// A default implementation of <see cref="ProjectionNotifier{TArgs, TTarget}"/> that 
        /// directly notifies all registered handlers with the same type as the input argument.
        /// This is useful when no transformation is required between the sender and receiver types.
        /// </summary>
        public Notifier(Action<Action>? notificationsRunner = null) : base(source => source, notificationsRunner)
        {
            // No additional initialization needed
        }
    }
}
