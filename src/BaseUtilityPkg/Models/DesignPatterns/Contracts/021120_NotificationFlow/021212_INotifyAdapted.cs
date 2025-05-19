namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow
{
    /// <summary>
    /// Defines a contract for notifying an adapted result derived from the specified source argument.
    /// </summary>
    /// <typeparam name="TSource">The type of the input argument.</typeparam>
    public interface INotifyAdapted<in TSource>
    {
        /// <summary>
        /// Notifies an adapted object derived from the specified source argument.
        /// </summary>
        /// <param name="args">The source argument to adapt and notify.</param>
        void Notify(TSource args);
    }
}
