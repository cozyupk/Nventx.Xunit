using System;
using System.Runtime.CompilerServices;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface IProofForAction
    {
        void Probe(
            Action act,
            string? label,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        );
    }
}
