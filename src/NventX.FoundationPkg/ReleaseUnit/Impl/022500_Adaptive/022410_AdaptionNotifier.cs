using NventX.FoundationPkg.Abstractions.Traits;
using NventX.FoundationPkg.Impl.Observing;
using System;

namespace NventX.FoundationPkg.Impl.Adaptive
{
    /*
    /// <summary>
    /// A notifier that adapts input arguments of type <typeparamref name="TArgs"/> to 
    /// a target type <typeparamref name="TTarget"/> using a custom projection function,
    /// then notifies all registered handlers with the adapted result.
    /// 
    /// This is useful when the notification consumers expect a different representation
    /// than the original input.    /// </summary>
    public class AdaptionNotifier<TTarget, TArgs> : ProjectionNotifier<TTarget, TArgs>
        where TTarget : class
        where TArgs : IAdaptTo<TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptionNotifier{TArgs, TTarget}"/> class
        /// with a custom projection function that transforms input arguments to the target type.
        /// </summary>
        /// <param name="projection">A function that adapts <typeparamref name="TArgs"/> to <typeparamref name="TTarget"/>.</param>
        public AdaptionNotifier(Action<Action>? notificationsRunner = null) : base(source => source.Adapt(), notificationsRunner)
        {
            // No additional initialization needed
        }
    }
    */
}
