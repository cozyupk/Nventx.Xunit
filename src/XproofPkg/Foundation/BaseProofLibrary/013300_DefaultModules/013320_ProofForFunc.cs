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
    /// A proof implementation that allows for probing functions for failures, recording any exceptions that occur during execution.
    /// </summary>
    internal class ProofForFunc<TLabelAxes> : IProofForFunc<TLabelAxes>, IRawProofForFunc<TLabelAxes>
    {
        /// <summary>
        /// The MethodInfo for the Probe method, used to invoke the probing logic.
        /// </summary>
        static MethodInfo InvokedMethodInfo { get; }
            = typeof(ProofForFunc<TLabelAxes>).GetMethod(nameof(Probe), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
              ?? throw new InvalidOperationException("Method 'Probe' not found in ProofForFunc class.");

        /// <summary>
        /// The invokable proof that will be used to record probing failures.
        /// </summary>
        IInvokableProof<TLabelAxes> InvokableProof { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProofForFunc"/> class with the specified invokable proof.
        /// </summary>
        public ProofForFunc(IInvokableProof<TLabelAxes> invokableProof)
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
            TLabelAxes? label = default,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null,
            IPositionInArray? combinedPosition = null
        )
        {
            Console.WriteLine($"***** new Func Probe *****: {invokedMethodInfo}");

            // Validate the parameters (CallerXXX also must not be null.)
            _ = func ?? throw new ArgumentNullException(nameof(func), $"Function(Func<{typeof(T)}>) cannot be null.");
            _ = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath), "Caller file path cannot be null.");
            _ = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName), "Caller member name cannot be null.");

            // Validate the invoked method info
            invokedMethodInfo ??= InvokedMethodInfo;
            if (invokedMethodInfo.IsGenericMethod && invokedMethodInfo.ContainsGenericParameters)
            {
                throw new InvalidOperationException(
                    $"The invoked method '{invokedMethodInfo}' is a generic method with unbound type parameters. \n" +
                    "[HINT] Use MakeGenericMethod(...) to provide concrete types like int, string, etc. before passing it to the probe."
                );
            }

            // Get the list of delegates from the function
            var delegates = func.GetInvocationList();

            // Process each delegate in the invocation list
            var i = 0;

            // Initialize the return value
            // Note: If multiple delegates are present, only the return value of the last one is preserved.
            T? retval = default;

            foreach (var del in delegates)
            {
                // If there are multiple delegates, set the position to indicate the current index and total count
                IPositionInArray? delegatePosition = (1 < delegates.Length) ? new PositionInArray(i + 1, delegates.Length) : null;

                // Get the parameters for the probe scope
                // Note: Parameters are shared across all delegate invocations for this probe.
                //       If per-delegate parameters are needed, this logic should be refactored accordingly.
                object?[] parameters = invokedParameters ?? new object?[] {
                    func, label
                };
                // Create a probe scope for the current delegate invocation
                using IProbeScope scope
                    = new ProbeScope<TLabelAxes>(
                        InvokableProof, InvokableProof.ProofInvocationKind,
                        invokedMethodInfo, parameters,
                        callerFilePath, callerLineNumber, callerMemberName, label,
                        combinedPosition, delegatePosition
                      );
                try
                {
                    // Execute the function that may throw an exception
                    retval = ((Func<T>)del)();
                    scope.RecordSuccess();
                }
                catch (Exception ex)
                {
                    // Record the probing failure with the label and exception
                    scope.RecordFailure(ex);
                }
                ++i;
            }
            return retval;
        }
    }
}
