using System;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.BaseProofLibrary.DefaultModules
{
    internal class CombinerForTasks<TLabelAxes> : ICombinerForTasks<TLabelAxes>
    {
        public Abstractions.TestProofForTestMethods.ICombinedProvable<TLabelAxes> Combine(params Func<Task>[] tasks)
        {
            throw new NotImplementedException();
        }
    }
}
