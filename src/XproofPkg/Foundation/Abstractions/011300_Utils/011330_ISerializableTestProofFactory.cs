using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.Abstractions.Utils
{
    /// <summary>
    /// Factory interface for creating serializable test proofs of a specific type.
    /// </summary>
    public interface ISerializableTestProofFactory<out TTestProof> : ISerializableTestProofFactoryBase
        where TTestProof : IInvokableProofBase
    {
        /// <summary>
        /// Creates an instance of TTarget using the provided parameters.
        /// </summary>
        TTestProof Create();
    }
}
