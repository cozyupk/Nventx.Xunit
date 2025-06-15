using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.BaseProofLibrary.ScopeAndResults
{
    /// <summary>
    /// Represents a scope for probing within a test proof, allowing for recording success or failure of the probe.
    /// </summary>
    public sealed class ProbeScope<TAxes> : IProbeScope
    {
        /// <summary>
        /// The invokable proof instance that this scope belongs to, used for recording results.
        /// </summary>
        private IInvokableProof<TAxes> InvokableProof { get; }

        /// <summary>
        /// A stopwatch to measure the duration of the probe scope execution.
        /// </summary>
        private Stopwatch Stopwatch { get; } = new Stopwatch();

        /// <summary>
        /// Flag indicating whether the scope has been disposed.
        /// </summary>
        private bool Disposed { get; set; } = false;

        /// <summary>
        /// Whether the probe has been explicitly marked as completed (success or failure).
        /// </summary>
        private bool IsCompleted { get; set; } = false;

        /// <summary>
        /// The record of the probe scope, containing details about the invocation and parameters.
        /// </summary>
        private IProbeScopeRecord<TAxes> ScopeRecord { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeScope"/> class, representing a scope for probing within a test proof.
        /// </summary>
        public ProbeScope(
            IInvokableProof<TAxes> invokableProof,
            ProofInvocationKind invocationKind,
            MethodInfo invokedProbeMethod,
            object?[] invocationParameters,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            TAxes? axes,
            IPositionInArray? combinedPosition,
            IPositionInArray? delegatePosition
        ) {
            // Validate parameters
            _ = invokableProof ?? throw new ArgumentNullException(nameof(invokableProof));
            _ = invokedProbeMethod ?? throw new ArgumentNullException(nameof(invokedProbeMethod));
            _ = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath));
            _ = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName));

            // Store the invokable proof instance
            InvokableProof = invokableProof;

            // Create the ProbeScope instance, for internal use at this time, and goint to be exposed later.
            ScopeRecord = new ProbeScopeRecord<TAxes>(
                invocationKind, invokedProbeMethod, invocationParameters,
                callerFilePath, callerLineNumber, callerMemberName, axes,
                combinedPosition, delegatePosition
            );

            // Start the stopwatch
            Stopwatch.Start();
        }

        /// <summary>
        /// Records the result of the probe scope execution, marking it as completed and stopping the stopwatch.
        /// </summary>
        private void RecordResult(Exception? ex = null)
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                Stopwatch.Stop();
                InvokableProof.RecordProbeResult(
                    new ProbeResult<TAxes>(ScopeRecord, Stopwatch.Elapsed, ex)
                );
            }
        }

        /// <summary>
        /// Records a successful probe result, marking the scope as completed and stopping the stopwatch.
        /// </summary>
        public void RecordSuccess()
        {
            if (IsCompleted) return;
            RecordResult();
        }

        /// <summary>
        /// Records a failure in the probe scope, marking it as completed and stopping the stopwatch.
        /// </summary>
        public void RecordFailure(Exception ex)
        {
            if (IsCompleted) return;
            RecordResult(ex);
        }

        /// <summary>
        /// Disposes of the probe scope, ensuring that it is properly cleaned up and that results are recorded.
        /// </summary>
        public void Dispose()
        {
            // Check if already disposed
            if (Disposed) return;

            // If not completed, record an invalid operation exception
            if (!IsCompleted)
            {
                RecordResult(
                    new InvalidOperationException("ProbeScope.Dispose() called without Success() or Failure().")
                );
            }

            // Mark as disposed and suppress finalization
            Disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Asynchronously disposes of the probe scope, ensuring that it is properly cleaned up and that results are recorded.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            Dispose();

            await Task.Yield(); // force async path if awaited
        }

        /// <summary>
        /// Finalizer for the ProbeScope, ensuring that it is disposed of if not already done so.
        /// </summary>
        ~ProbeScope()
        {
            Dispose();
        }
    }
}
