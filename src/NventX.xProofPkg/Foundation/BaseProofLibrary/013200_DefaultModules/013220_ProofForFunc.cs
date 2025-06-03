using System;
using System.Runtime.CompilerServices;
using NventX.xProof.Abstractions.TestProofForTestMethods;
using NventX.xProof.Abstractions.TestProofForTestRunner;

namespace NventX.xProof.BaseProofLibrary.DefaultModules
{
    /// <summary>
    /// A proof implementation that allows for probing functions for failures, recording any exceptions that occur during execution.
    /// </summary>
    internal class ProofForFunc : IProofForFunc
    {
        /// <summary>
        /// The invokable proof that will be used to record probing failures.
        /// </summary>
        IInvokableProof InvokableProof { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofForFunc"/> class with the specified invokable proof.
        /// </summary>
        public ProofForFunc(IInvokableProof invokableProof)
        {
            // Validate and assign the invokable proof
            InvokableProof
                = invokableProof
                ?? throw new ArgumentNullException(nameof(invokableProof), "InvokableProof cannot be null.");
        }

        /// <summary>
        /// Probes a function for failures, executing it and recording any exceptions that occur.
        /// </summary>
        public T? Probe<T>(
            Func<T> func,
            string? label = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        ) {
            try
            {
                // Execute the action that may throw an exception
                return func();
            }
            catch (Exception ex)
            {
                // Record the probing failure with the label and exception
                InvokableProof.RecordProbingFailure(
                    label, func, ex, callerFilePath, callerLineNumber, callerMemberName
                );
            }
            return default;
        }
    }
}
