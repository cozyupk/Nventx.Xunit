using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xproof.Abstractions.Sdk;
using Xproof.Abstractions.TestProofForTestMethods;
using Xproof.Abstractions.TestProofForTestRunner;
using Xproof.BaseProofLibrary.ScopeAndResults;

namespace Xproof.BaseProofLibrary.DefaultModules
{
    /// <summary>
    /// A proof implementation that allows for probing actions for failures, recording any exceptions that occur during execution.
    /// </summary>
    internal class ProofForAction<TAxes> : IProofForAction<TAxes>, IRawProofForAction<TAxes>
    {
        /// <summary>
        /// The MethodInfo for the Probe method, used to invoke the probing logic.
        /// </summary>
        static MethodInfo InvokedMethodInfo { get; }
            = typeof(ProofForAction<TAxes>).GetMethod(nameof(Probe), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
              ?? throw new InvalidOperationException("Method 'Probe' not found in ProofForAction class.");

        /// <summary>
        /// The invokable proof that will be used to record probing failures.
        /// </summary>
        IInvokableProof<TAxes> InvokableProof { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofForAction"/> class with the specified invokable proof.
        /// </summary>
        public ProofForAction(IInvokableProof<TAxes> invokableProof)
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
            TAxes? axes = default,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null,
            IPositionInArray? combinedPosition = null
        )
        {
            // Validate the parameters (CallerXXX also must not be null.)
            _ = act ?? throw new ArgumentNullException(nameof(act), "Action cannot be null.");
            _ = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath), "Caller file path cannot be null.");
            _ = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName), "Caller member name cannot be null.");

            // Get the list of delegates from the action
            var delegates = act.GetInvocationList();

            // Process each delegate in the invocation list
            var i = 0;

            foreach (var del in delegates) {
                // If there are multiple delegates, set the position to indicate the current index and total count
                IPositionInArray? delegatePosition = (1 < delegates.Length) ? new PositionInArray(i + 1, delegates.Length) : null;

                // Get the parameters for the probe scope
                // Note: Parameters are shared across all delegate invocations for this probe.
                //       If per-delegate parameters are needed, this logic should be refactored accordingly.
                object?[] parameters = invokedParameters ?? new object?[] {
                    act, axes
                };
                // Create a probe scope for the current delegate invocation
                using IProbeScope scope
                    = new ProbeScope<TAxes>(
                        InvokableProof, InvokableProof.ProofInvocationKind,
                        invokedMethodInfo ?? InvokedMethodInfo, parameters,
                        callerFilePath, callerLineNumber, callerMemberName, axes,
                        combinedPosition, delegatePosition
                      );
                try
                {
                    // Execute the action that may throw an exception
                    ((Action)del)();
                    scope.RecordSuccess();
                }
                catch (Exception ex)
                {
                    // Record the probing failure with the label and exception
                    scope.RecordFailure(ex);
                }
                ++i;
            }
        }
    }
}
