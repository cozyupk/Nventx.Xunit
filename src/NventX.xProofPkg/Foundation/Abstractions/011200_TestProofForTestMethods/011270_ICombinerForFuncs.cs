using System;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForFuncs
    {
        IProvable<T> Combine<T>(params Func<T>[] functions);
    }
}
