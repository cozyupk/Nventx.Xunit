using System;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface IProofForFunc
    {
        T? Probe<T>(
            Func<T> func,
            string? label = null,
            string? callerFilePath = null,
            int callerLineNumber = 0,
            string? callerMemberName = null
        );
    }
}
