using System;
using NventX.FoundationPkg.Abstractions.Traits;
using NventX.FoundationPkg.Impl.Observing;

namespace NventX.FoundationPkg.Impl.Adaptive
{
    /*
    /// <summary>
    /// A switchboard that adapts incoming notifications using the <see cref="IAdaptTo{T}"/> interface,
    /// and forwards the adapted values to all registered handlers.
    ///
    /// This is a convenience implementation of <see cref="ProjectionSwitchBoard{TTarget, TSource}"/>
    /// that automatically invokes <c>Adapt()</c> on each received item.
    /// </summary>
    public class AdaptionSwitchBoard<TArgs, TTarget> : ProjectionSwitchBoard<TTarget, TArgs>
        where TTarget : class
        where TArgs : IAdaptTo<TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptionSwitchBoard{TTarget, TArgs}"/> class,
        /// using <c>x => x.Adapt()</c> as the projection function.
        /// </summary>
        public AdaptionSwitchBoard(Action<Action>? notificationsRunner = null) : base(source => source.Adapt(), notificationsRunner)
        {
            // No additional initialization needed
        }
    }
    */
}
