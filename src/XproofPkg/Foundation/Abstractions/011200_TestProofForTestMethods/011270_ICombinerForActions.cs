using System;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForActions<in TLabelAxes>
    {
        ICombinedProvable<TLabelAxes> Combine(params Action[] actions);
    }
}
