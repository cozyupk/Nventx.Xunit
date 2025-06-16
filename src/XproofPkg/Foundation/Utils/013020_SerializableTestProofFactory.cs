using Xproof.Abstractions.TestProofForTestRunner;
using Xproof.Abstractions.Utils;

namespace Xproof.Utils
{
    /// <summary>
    /// A factory for creating serializable test proofs that implement the ITestProof interface.
    /// </summary>
    public class SerializableTestProofFactory<TTestProof, TLabelAxes> : SerializableFactory<TTestProof>, ISerializableTestProofFactory<TTestProof, TLabelAxes>
        where TTestProof : IInvokableProof<TLabelAxes>
    {
    }
}
