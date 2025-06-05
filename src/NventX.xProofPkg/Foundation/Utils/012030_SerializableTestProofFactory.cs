using NventX.xProof.Abstractions.TestProofForTestRunner;
using NventX.xProof.Abstractions.Utils;

namespace NventX.xProof.Utils
{
    /// <summary>
    /// A factory for creating serializable test proofs that implement the ITestProof interface.
    /// </summary>
    public class SerializableTestProofFactory<TTestProof> : SerializableFactory<TTestProof>, ISerializableTestProofFactory<TTestProof>
        where TTestProof : IInvokableProof
    {
    }
}
