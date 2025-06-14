using System;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.BaseProofLibrary.DefaultModules
{
    internal class CombinerForTasks : ICombinerForTasks
    {
        public Abstractions.TestProofForTestMethods.IProvable Combine(params Func<Task>[] tasks)
        {
            throw new NotImplementedException();
        }
    }
}
