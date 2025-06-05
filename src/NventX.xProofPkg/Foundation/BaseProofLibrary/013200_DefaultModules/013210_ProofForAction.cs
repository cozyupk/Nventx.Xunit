using System;
using System.Runtime.CompilerServices;
using NventX.xProof.Abstractions.TestProofForTestMethods;
using NventX.xProof.Abstractions.TestProofForTestRunner;

namespace NventX.xProof.BaseProofLibrary.DefaultModules
{
    /// <summary>
    /// A proof implementation that allows for probing actions for failures, recording any exceptions that occur during execution.
    /// </summary>
    internal class ProofForAction : IProofForAction
    {
        /// <summary>
        /// The invokable proof that will be used to record probing failures.
        /// </summary>
        IInvokableProof InvokableProof { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofForAction"/> class with the specified invokable proof.
        /// </summary>
        public ProofForAction(IInvokableProof invokableProof)
        {
            // Validate and assign the invokable proof
            InvokableProof
                = invokableProof 
                ?? throw new ArgumentNullException(nameof(invokableProof), "InvokableProof cannot be null.");
        }

        /// <summary>
        /// Probes an action for failures, executing it and recording any exceptions that occur.
        /// </summary>
        public void Probe(
            Action act,
            string? label = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        ) {
            var delegates = act.GetInvocationList();
            var i = 0;
            foreach (var del in delegates) {
                try
                {
                    // Execute the action that may throw an exception
                    ((Action)del)();
                }
                catch (Exception ex)
                {
                    // Record the probing failure with the label and exception
                    InvokableProof.RecordProbingFailure(
                        label, act, ex, callerFilePath, callerLineNumber, callerMemberName, i, delegates.Length
                    );
                }
                ++i;
            }
        }
    }
}
