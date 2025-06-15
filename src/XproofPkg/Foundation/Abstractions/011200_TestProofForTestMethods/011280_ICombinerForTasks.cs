using System;
using System.Threading.Tasks;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForTasks<in TAxes>
    {
        ICombinedProvable<TAxes> Combine(params Func<Task>[] tasks);
    }
}
