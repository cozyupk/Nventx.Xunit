using System;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForFuncs<T>
    {
        IProvable Combine(params Func<T>[] functions);
    }
}
