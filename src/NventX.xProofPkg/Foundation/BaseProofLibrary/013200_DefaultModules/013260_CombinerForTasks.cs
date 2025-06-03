using System;
using System.Threading.Tasks;
using NventX.xProof.Abstractions.TestProofForTestMethods;

namespace NventX.xProof.BaseProofLibrary.DefaultModules
{
    internal class CombinerForTasks : ICombinerForTasks
    {
        public Abstractions.TestProofForTestMethods.IProvable Combine(params Func<Task>[] tasks)
        {
            throw new NotImplementedException();
        }
    }
}
