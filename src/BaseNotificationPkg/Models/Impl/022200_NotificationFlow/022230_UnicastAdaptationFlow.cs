using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.NotificationFlow
{
    /// <summary>
    /// A unicast notification flow that uses IAdaptTo to convert the source to the target type before notifying.
    /// </summary>
    public class UnicastAdaptationFlow<TTarget, TSource> : UnicastProjectionFlow<TTarget, TSource>
        where TTarget : class
        where TSource : IAdaptTo<TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the UnicastAdaptationFlow class.
        /// </summary>
        public UnicastAdaptationFlow() : base(source => source.Adapt())
        {
        }
    }
}
