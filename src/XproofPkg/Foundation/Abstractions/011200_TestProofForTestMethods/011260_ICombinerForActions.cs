using System;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForActions
    {
        IProvable Combine(params Action[] actions);
    }
}
