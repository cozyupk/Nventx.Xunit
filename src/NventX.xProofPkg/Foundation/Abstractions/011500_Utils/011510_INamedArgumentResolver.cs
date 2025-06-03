namespace NventX.xProof.Abstractions.Utils
{
    /// <summary>
    /// Interface for resolving named arguments in test proofs.
    /// </summary>
    public interface INamedArgumentResolver
    {
        /// <summary>
        /// Resolves a named argument of type T.
        /// </summary>
        T Resolve<T>(string name);
    }
}
