namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits
{
    /// <summary>
    /// Defines a contract for adapting an object to a specified target type.
    /// </summary>
    /// <typeparam name="TTarget">The target type to adapt to.</typeparam>
    public interface IAdaptTo<out TTarget> where TTarget : class
    {
        /// <summary>
        /// Adapts the current object to the specified target type.
        /// </summary>
        /// <returns>An instance of the target type.</returns>
        TTarget Adapt();
    }
}
