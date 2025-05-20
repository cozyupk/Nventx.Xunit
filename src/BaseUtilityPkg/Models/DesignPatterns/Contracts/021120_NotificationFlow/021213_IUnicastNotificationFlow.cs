namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow
{
    /// <summary>
    /// Defines a flow that adapts a source object and notifies the adapted result.
    /// </summary>
    /// <typeparam name="TTarget">The type of the adapted target.</typeparam>
    /// <typeparam name="TSource">The type of the source argument.</typeparam>
    public interface IUnicastNotificationFlow<out TTarget, in TSource>
        : INotificationHandler<TTarget>, IUnicastNotifier<TSource>
    {
    }
}
