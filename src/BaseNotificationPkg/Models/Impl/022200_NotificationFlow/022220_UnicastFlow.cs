namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.NotificationFlow
{
    /// <summary>
    /// A simple unicast notification flow where the source and target types are the same.
    /// No adaptation is performed; the source is passed through as-is.
    /// </summary>
    public class UnicastFlow<TTarget> : UnicastProjectionFlow<TTarget, TTarget>
    {
        /// <summary>
        /// Initializes a new instance of the UnicastFlow class.
        /// </summary>
        public UnicastFlow() : base(source => source)
        {
        }
    }
}
