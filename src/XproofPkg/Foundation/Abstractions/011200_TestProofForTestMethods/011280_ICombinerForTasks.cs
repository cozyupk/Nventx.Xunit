using System;
using System.Threading.Tasks;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForTasks
    {
        IProvable Combine(params Func<Task>[] tasks);
    }
}
