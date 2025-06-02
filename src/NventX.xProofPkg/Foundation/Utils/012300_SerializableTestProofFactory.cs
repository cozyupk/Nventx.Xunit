using NventX.xProof.Abstractions;

namespace NventX.xProof.Utils
{
    public class SerializableTestProofFactory<TTestProof> : SerializableFactory<TTestProof>, ISerializableTestProofFactory<TTestProof>
        where TTestProof : ITestProof
    {
    }
}
