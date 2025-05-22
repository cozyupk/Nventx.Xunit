namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow
{
    /// <summary>
    /// Represents a unidirectional notification flow that adapts input of type <typeparamref name="TSource"/>
    /// and notifies the adapted result as <typeparamref name="TTarget"/> to a single handler.
    /// </summary>
    /// <typeparam name="TTarget">The type of the adapted and notified result.</typeparam>
    /// <typeparam name="TSource">The type of the input argument being adapted.</typeparam>
    public interface IUnicastNotificationFlow<out TTarget, in TSource>
        : INotificationHandler<TTarget>, IUnicastNotifier<TSource>
    {
    }
}
