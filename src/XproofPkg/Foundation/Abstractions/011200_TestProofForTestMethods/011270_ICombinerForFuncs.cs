using System;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForFuncs<in TAxes>
    {
        IProvable<T, TAxes> Combine<T>(params Func<T>[] functions);
    }
}
