using System;
using System.Threading.Tasks;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForTasks
    {
        ICombinedProvable Combine(params Func<Task>[] tasks);
    }
}
