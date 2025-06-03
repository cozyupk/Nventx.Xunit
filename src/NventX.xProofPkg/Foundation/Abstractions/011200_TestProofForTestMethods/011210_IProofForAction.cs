using System;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface IProofForAction
    {
        void Probe(
            Action act,
            string? label,
            string? callerFilePath,
            int callerLineNumber,
            string? callerMemberName
        );
    }
}
