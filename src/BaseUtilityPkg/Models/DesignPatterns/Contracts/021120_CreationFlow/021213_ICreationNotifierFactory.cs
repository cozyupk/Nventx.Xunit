namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.CreationFlow
{
    /// <summary>
    /// Defines a flow interface that creates objects without returning them directly.
    /// Creation completion is typically observed via an delegate.
    /// </summary>
    /// <typeparam name="TTarget">The type of the object that will be created.</typeparam>
    /// <typeparam name="TSource">The type of arguments required to create the object.</typeparam>
    public interface ICreationFlow<out TTarget, in TSource>
        : ICreationNotified<TTarget>, ICreationNotifier<TSource>
    {
    }
}
