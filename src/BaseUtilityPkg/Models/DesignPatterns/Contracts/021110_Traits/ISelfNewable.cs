namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits
{
    /// <summary>
    /// Defines a contract for creating a new instance of the target type from the current instance.
    /// </summary>
    public interface ISelfNewable<out TTarget> where TTarget : class
    {
        /// <summary>
        /// Creates a new instance of <typeparamref name="TTarget"/> based on the current instance.
        /// </summary>
        /// <returns>A new instance of <typeparamref name="TTarget"/>.</returns>
        TTarget NewFromSelf();
    }
}
