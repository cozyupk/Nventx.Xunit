using System;
using System.Threading.Tasks;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface ICombinerForTasks
    {
        IProvable Combine(params Func<Task>[] tasks);
    }
}
