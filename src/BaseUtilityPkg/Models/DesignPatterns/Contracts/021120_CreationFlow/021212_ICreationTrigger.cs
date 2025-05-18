namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.CreationFlow
{
    /// <summary>
    /// Defines a contract for notifying when an object of type <typeparamref name="TSource"/> is created.
    /// </summary>
    public interface ICreationNotifier<in TSource>
    {
        /// <summary>
        /// Creates an object and notifies with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments used for creation and notification.</param>
        void CreateAndNotify(TSource args);
    }
}
