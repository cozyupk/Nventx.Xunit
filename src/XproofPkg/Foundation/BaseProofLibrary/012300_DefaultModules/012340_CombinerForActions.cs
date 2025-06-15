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
    /// A combiner for actions that allows for late failure collection during test execution.
    /// </summary>
    internal class CombinerForActions<TAxes> : ICombinerForActions<TAxes>
    {
        /// <summary>
        /// The proof for action that will be used to record probing failures.
        /// </summary>
        IRawProofForAction<TAxes> ProofForAction { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinerForActions"/> class with the specified proof for action.
        /// </summary>
        public CombinerForActions(IRawProofForAction<TAxes> proofForAction)
        {
            // Validate and assign the proof for action
            ProofForAction = proofForAction
                ?? throw new ArgumentNullException(nameof(proofForAction), "ProofForAction cannot be null.");
        }

        /// <summary>
        /// Combines multiple actions into a single provable element that can be probed for failures.
        /// </summary>
        public ICombinedProvable<TAxes> Combine(params Action[] actions)
        {
            return new ProvableActions(ProofForAction, actions);
        }

        /// <summary>
        /// A class that encapsulates a collection of actions to be probed for failures.
        /// </summary>
        internal class ProvableActions : ICombinedProvable<TAxes>, IRawCombinedProvable<TAxes>
        {
            /// <summary>
            /// The method info for the invoked method that will be used to record probings.
            /// </summary>
            static MethodInfo InvokedMethodInfo { get; }
                = typeof(CombinerForActions<TAxes>).GetMethod(nameof(Probe), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                  ?? throw new InvalidOperationException("Method 'Probe' not found in ProofForAction class.");

            /// <summary>
            /// The test proof instance that this collection belongs to.
            /// </summary>
            IRawProofForAction<TAxes> TestProof { get; }

            /// <summary>
            /// An array of actions that will be probed for failures.
            /// </summary>
            Action[] Actions { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ProvableActions"/> class with the specified test proof and actions.
            /// </summary>
            public ProvableActions(IRawProofForAction<TAxes> testProof, Action[] actions)
            {
                // Validate and assign the test proof and actions
                TestProof = testProof ?? throw new ArgumentNullException(nameof(testProof), "TestProof cannot be null.");
                Actions = actions ?? throw new ArgumentNullException(nameof(actions), "Actions cannot be null.");
            }

            /// <summary>
            /// Probes each action in the collection for failures, executing them and collecting any exceptions that occur.
            /// </summary>
            public virtual void Probe(
                TAxes? axes = default,
                [CallerFilePath] string? callerFilePath = null,
                [CallerLineNumber] int callerLineNumber = 0,
                [CallerMemberName] string? callerMemberName = null,
                MethodInfo? invokedMethodInfo = null,
                object?[]? invokedParameters = null
            )
            {
                // Validate the parameters (CallerXXX also must not be null.)
                _ = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath), "Caller file path cannot be null.");
                _ = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName), "Caller member name cannot be null.");

                // Use the provided invokedMethodInfo or fall back to the default one
                invokedMethodInfo ??= InvokedMethodInfo;

                // Declare a combined position to track the index and total count
                IPositionInArray combinedPosition = new PositionInArray(1, Actions.Length);

                // Iterate through each action and execute FailLate for each
                foreach (var action in Actions)
                {
                    object?[] parameters
                        = invokedParameters
                           ?? new object?[] {action, axes };
                    TestProof.Probe
                         (action, axes, callerFilePath, callerLineNumber, callerMemberName, invokedMethodInfo, parameters, combinedPosition);
                    combinedPosition.MoveToNext(); // Move to the next position
                }
            }
        }
    }
}

