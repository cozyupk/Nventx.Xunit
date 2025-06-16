using System;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForFuncs<in TLabelAxes>
    {
        IProvable<T, TLabelAxes> Combine<T>(params Func<T>[] functions);
    }
}
