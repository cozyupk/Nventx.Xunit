using System;
using System.Runtime.CompilerServices;
using NventX.xProof.Abstractions.TestProofForTestMethods;

namespace NventX.xProof.BaseProofLibrary.DefaultModules
{
    /// <summary>
    /// A combiner for actions that allows for late failure collection during test execution.
    /// </summary>
    internal class CombinerForActions : ICombinerForActions
    {
        /// <summary>
        /// The proof for action that will be used to record probing failures.
        /// </summary>
        IProofForAction ProofForAction { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinerForActions"/> class with the specified proof for action.
        /// </summary>
        public CombinerForActions(IProofForAction proofForAction)
        {
            // Validate and assign the proof for action
            ProofForAction = proofForAction
                ?? throw new ArgumentNullException(nameof(proofForAction), "ProofForAction cannot be null.");
        }

        /// <summary>
        /// Combines multiple actions into a single provable element that can be probed for failures.
        /// </summary>
        public IProvable Combine(params Action[] actions)
        {
            return new ProvableActions(ProofForAction, actions);
        }

        /// <summary>
        /// A class that encapsulates a collection of actions to be probed for failures.
        /// </summary>
        internal class ProvableActions : IProvable
        {
            /// <summary>
            /// The test proof instance that this collection belongs to.
            /// </summary>
            IProofForAction TestProof { get; }

            /// <summary>
            /// An array of actions that will be probed for failures.
            /// </summary>
            Action[] Actions { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ProvableActions"/> class with the specified test proof and actions.
            /// </summary>
            public ProvableActions(IProofForAction testProof, Action[] actions)
            {
                // Validate and assign the test proof and actions
                TestProof = testProof ?? throw new ArgumentNullException(nameof(testProof), "TestProof cannot be null.");
                Actions = actions ?? throw new ArgumentNullException(nameof(actions), "Actions cannot be null.");
            }

            /// <summary>
            /// Probes each action in the collection for failures, executing them and collecting any exceptions that occur.
            /// </summary>
            public virtual void Probe(
                [CallerFilePath] string? callerFilePath = null,
                [CallerLineNumber] int callerLineNumber = 0,
                [CallerMemberName] string? callerMemberName = null
            )
            {
                // Iterate through each action and execute FailLate for each
                foreach (var action in Actions)
                {
                    TestProof.Probe
                        (action, null, callerFilePath, callerLineNumber, callerMemberName);
                }
            }
        }
    }
}
