namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.CreationNotifiedFactory
{
    /// <summary>
    /// Defines an interface for initiating object creation using the specified arguments.
    /// Implementations may signal creation completion via event or other out-of-band mechanisms.
    /// </summary>
    /// <typeparam name="TCreationArgs">The type of arguments required to create the object.</typeparam>
    public interface ICreationTrigger<in TCreationArgs>
    {
        /// <summary>
        /// Initiates the creation of an object using the specified arguments.
        /// The created object is not returned from this method.
        /// </summary>
        /// <param name="args">Arguments necessary to perform creation.</param>
        void TriggerCreation(TCreationArgs args);
    }
}
