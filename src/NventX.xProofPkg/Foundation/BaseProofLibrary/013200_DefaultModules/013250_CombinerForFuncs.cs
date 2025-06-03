using System;
using NventX.xProof.Abstractions.TestProofForTestMethods;

namespace NventX.xProof.BaseProofLibrary.DefaultModules
{
    internal class CombinerForFuncs<T> : ICombinerForFuncs<T>
    {
        public Abstractions.TestProofForTestMethods.IProvable Combine(params Func<T>[] functions)
        {
            throw new NotImplementedException();
        }
    }
}
