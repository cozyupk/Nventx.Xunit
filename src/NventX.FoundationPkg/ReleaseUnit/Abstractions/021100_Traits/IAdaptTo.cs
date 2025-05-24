namespace NventX.FoundationPkg.Abstractions.Traits
{
    /// <summary>
    /// Defines a contract for adapting the current instance to a target type <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TTarget">The type to which the current object is adapted.</typeparam>
    public interface IAdaptTo<out TTarget> where TTarget : class
    {
        /// <summary>
        /// Converts the current instance into an object of type <typeparamref name="TTarget"/>.
        /// </summary>
        /// <returns>An instance of <typeparamref name="TTarget"/> representing the adapted form.</returns>
        TTarget Adapt();
    }
}
