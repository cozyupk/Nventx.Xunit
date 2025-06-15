using System;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.BaseProofLibrary.DefaultModules
{
    internal class CombinerForTasks<TAxes> : ICombinerForTasks<TAxes>
    {
        public Abstractions.TestProofForTestMethods.ICombinedProvable<TAxes> Combine(params Func<Task>[] tasks)
        {
            throw new NotImplementedException();
        }
    }
}
