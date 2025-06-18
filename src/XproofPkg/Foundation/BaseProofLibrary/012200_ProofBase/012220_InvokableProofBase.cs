using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.BaseProofLibrary.ProofBase
{
    /// <summary>
    /// Base class for invokable proofs, providing setup and failure collection functionality.
    /// </summary>
    public abstract class InvokableProofBase<TLabelAxes> : IInvokableProof<TLabelAxes>
    {
        /// <summary>
        /// The kind of proof invocation, indicating whether it is a single case, parameterized, or unknown.
        /// </summary>
        public ProofInvocationKind ProofInvocationKind { get; private set; }

        /// <summary>
        /// Lock object to ensure thread-safe access to the proof invocation kind.
        /// </summary>
        private object SetupLock { get; } = new object();

        /// <summary>
        /// Flag indicating whether the proof has already been set up.
        /// </summary>
        private bool IsAlreadySetup { get; set; } = false;

        /// <summary>
        /// A collection to store probing failures that occur during the test execution.
        /// </summary>
        protected ConcurrentQueue<IProbeResult<TLabelAxes>> ProbeResult { get; } = new();

        /// <summary>
        /// Sets up the test proof environment with the specified proof invocation kind.
        /// </summary>
        public virtual void Setup(ProofInvocationKind proofInvocationKind)
        {
            lock (SetupLock)
            {
                // If the proof invocation kind is already set, throw an exception
                if (IsAlreadySetup)
                {
                    throw new System.InvalidOperationException(
                        "Setup has already been called. Cannot change the proof invocation kind after setup.");
                }
                IsAlreadySetup = true;

                // Validate the proof invocation kind and set it
                if (!Enum.IsDefined(typeof(ProofInvocationKind), proofInvocationKind))
                {
                    throw new System.ArgumentException(
                        $"Invalid proof invocation kind: {proofInvocationKind}. " +
                        "Must be one of SingleCase, Parameterized, or Unknown.",
                        nameof(proofInvocationKind));
                }
                ProofInvocationKind = proofInvocationKind;
            }
        }

        /// <summary>
        /// Collects the test proof results after the test execution is complete.
        /// </summary>
        public IEnumerable<IProbeResult<TLabelAxes>> GetResults()
        {
            // Return a copy of the probe results as an array
            return ProbeResult.ToArray();
        }

        /// <summary>
        /// Collects the test proof results without labels after the test execution is complete.
        /// </summary>
        public IEnumerable<IProbeResultBase> GetResultBases()
        {
            return GetResults();
        }

        /// <summary>
        /// Records a probe result from the test execution, which can be a failure or success.
        /// </summary>
        public void RecordProbeResult(IProbeResult<TLabelAxes> probeResult)
        {
            // Validate the probe result before adding it to the collection
            if (probeResult == null)
            {
                throw new ArgumentNullException(nameof(probeResult), "Probe result cannot be null.");
            }

            // Enqueue the probe result into the collection
            ProbeResult.Enqueue(probeResult);
        }

        /// <summary>
        /// Commits the test proof results after the test case execution is complete.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual void Commit()
        {
            // Ensure that there are probe results to commit
            if (ProbeResult.IsEmpty)
            {
                throw new InvalidOperationException("No probing was performed. Did you forget to run at least one probe?");
            }
        }
    }
}
