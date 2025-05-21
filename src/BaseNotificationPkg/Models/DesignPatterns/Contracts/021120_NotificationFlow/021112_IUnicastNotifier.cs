namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow
{
    /// <summary>
    /// Defines a unicast notifier that processes a source argument of type <typeparamref name="TSource"/>
    /// and produces a derived notification.
    /// </summary>
    /// <typeparam name="TSource">The type of the input argument to be adapted and notified.</typeparam>

    public interface IUnicastNotifier<in TSource>
    {
        /// <summary>
        /// Sends a notification based on the specified input argument.
        /// </summary>
        /// <param name="args">The input argument used to produce the notification.</param>
        void Notify(TSource args);
    }
}
