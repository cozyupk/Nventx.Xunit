using System;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForActions
    {
        ICombinedProvable Combine(params Action[] actions);
    }
}
