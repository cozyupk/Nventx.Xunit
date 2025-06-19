using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.Abstractions.Utils
{
    /// <summary>
    /// Factory interface for creating serializable test proofs.
    /// </summary>
    public interface ISerializableTestProofFactoryBase
    {
        /// <summary>
        /// Injects parameters for object creation via resolver. Can only be called once.
        /// </summary>
        void SetParameter(INamedArgumentResolver resolver);

        /// <summary>
        /// Serializes the factory state to a string.
        /// </summary>
        string SerializeToString();

        /// <summary>
        /// Deserializes the factory state from a string.
        /// </summary>
        void DeserializeFromString(string serializedData);
    }
}
