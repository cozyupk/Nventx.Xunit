namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits
{
    /// <summary>
    /// Interface for shallow cloning capability.
    /// </summary>
    public interface IShallowClonable<T>
    {
        /// <summary>
        /// Creates a shallow clone of the current object.
        /// </summary>
        /// <returns>A shallow-cloned instance of the object.</returns>
        T ShallowClone();
    }
}