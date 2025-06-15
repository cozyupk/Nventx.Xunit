using System;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForActions<in TAxes>
    {
        ICombinedProvable<TAxes> Combine(params Action[] actions);
    }
}
