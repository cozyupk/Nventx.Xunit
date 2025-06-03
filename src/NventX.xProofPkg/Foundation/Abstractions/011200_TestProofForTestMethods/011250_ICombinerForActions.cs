using System;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForActions
    {
        IProvable Combine(params Action[] actions);
    }
}
