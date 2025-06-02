namespace NventX.xProof.Abstractions
{
    public interface ISerializableTestProofFactory<out TTestProof>
        where TTestProof : ITestProof
    {  
        /// <summary>
        /// Injects parameters for object creation via resolver. Can only be called once.
        /// </summary>
        void SetParameter(INamedArgumentResolver resolver);

        /// <summary>
        /// Creates an instance of TTarget using the provided parameters.
        /// </summary>
        TTestProof Create();
        
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
