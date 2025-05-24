using System;
using NventX.FoundationPkg.Impl.Observing;

namespace NventX.FoundationPkg.Impl.Passthrough
{
    /// <summary>
    /// A simplified switchboard that receives notifications and dispatches them directly to all handlers,
    /// without any transformation.
    /// 
    /// This is a shortcut for <see cref="ProjectionSwitchBoard{TTarget, TSource}"/> when both source and target
    /// types are the same.
    /// </summary>
    public class SwitchBoard<TType> : ProjectionSwitchBoard<TType, TType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchBoard{TType}"/> class,
        /// using an identity projection (i.e., <c>x => x</c>).
        /// </summary>
        public SwitchBoard(Action<Action>? notificationsRunner = null) : base(source => source, notificationsRunner)
        {
            // No additional initialization needed
        }
    }
}
