using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NventX.xProof.Abstractions.TestProofForTestMethods;
using NventX.xProof.Abstractions.TestProofForTestRunner;

namespace NventX.xProof.BaseProofLibrary.DefaultModules

{
    internal class ProofForTask : IProofForTask
    {
        /// <summary>
        /// The invokable proof that will be used to record probing failures.
        /// </summary>
        IInvokableProof InvokableProof { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofForTask"/> class with the specified invokable proof.
        /// </summary>
        public ProofForTask(IInvokableProof invokableProof)
        {
            // Validate and assign the invokable proof
            InvokableProof
                = invokableProof
                ?? throw new ArgumentNullException(nameof(invokableProof), "InvokableProof cannot be null.");
        }

        /// <summary>
        /// Probes an asynchronous action for failures, executing it and recording any exceptions that occur.
        /// </summary>
        public async Task ProbeAsync(
            Func<Task> act,
            string? label = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        )
        {
            try
            {
                // Execute the asynchronous action that may throw an exception
                await act();
            }
            catch (Exception ex)
            {
                // Record the probing failure with the label and exception
                InvokableProof.RecordProbingFailure(
                    label, act, ex, callerFilePath, callerLineNumber, callerMemberName
                );
            }
        }
    }
}
