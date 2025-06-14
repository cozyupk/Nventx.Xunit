using System;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForFuncs
    {
        IProvable<T> Combine<T>(params Func<T>[] functions);
    }
}
