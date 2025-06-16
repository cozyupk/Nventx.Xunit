using System;
using System.Threading.Tasks;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForTasks<in TLabelAxes>
    {
        ICombinedProvable<TLabelAxes> Combine(params Func<Task>[] tasks);
    }
}
