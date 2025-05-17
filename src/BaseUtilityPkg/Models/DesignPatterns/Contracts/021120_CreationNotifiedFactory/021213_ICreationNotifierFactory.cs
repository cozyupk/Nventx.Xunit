namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.CreationNotifiedFactory
{
    /// <summary>
    /// Defines a factory interface that creates objects without returning them directly.
    /// Creation completion is typically observed via an event.
    /// </summary>
    /// <typeparam name="TCreatedObject">The type of the object that will be created.</typeparam>
    /// <typeparam name="TCreationArgs">The type of arguments required to create the object.</typeparam>
    public interface ICreationNotifierFactory<TCreatedObject, in TCreationArgs>
        : ICreationNotified<TCreatedObject>, ICreationTrigger<TCreationArgs>
    {
    }
}
