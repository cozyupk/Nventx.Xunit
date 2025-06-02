using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NventX.xProof.Abstractions;
using NventX.xProof.Utils;

namespace NventX.xProof.BaseProofLibrary
{
    /// <summary>
    /// A proof implementation that allows for late failure collection during test execution.
    /// </summary>
    public class FailLateProof : ITestProof
    {
        /// <summary>
        /// A collection to store probing failures that occur during the test execution.
        /// </summary>
        protected ConcurrentBag<IProbingFailure> ProbingFailures { get; } = new();

        /// <summary>
        /// Records a probing failure with a label and exception details.
        /// </summary>
        protected void RecordProbingFailure(string? label, Delegate act, Exception ex)
        {
            // If a label is not provided, use the target method's name as the label
            if (string.IsNullOrEmpty(label))
            {
                label = act.Target?.GetType().Name + "." + act.Method?.Name;
                label ??= "<anonymous>";
            }
            // Record the probing failure with the label and exception
            ProbingFailures.Add(new ProbingFailure(label!, ex));
        }

        /// <summary>
        /// Sets up the test proof environment.
        /// </summary>
        public void Setup(ProofInvocationKind _)
        {
            // No initialization needed for this proof implementation.
        }

        /// <summary>
        /// Collects probing failures encountered during the test execution.
        /// </summary>
        public void FailLate(Action act, string? label = null)
        {
            try
            {
                // Execute the action that may throw an exception
                act();
            }
            catch (Exception ex)
            {
                // Record the probing failure with the label and exception
                RecordProbingFailure(label, act, ex);
            }
        }

        /// <summary>
        /// Collects probing failures encountered during the test execution for multiple actions.
        /// </summary>
        public void FailLate(params Action[] actions)
        {
            // Iterate through each action and execute FailLate for each
            foreach (var action in actions)
            {
                FailLate(action);
            }
        }

        /// <summary>
        /// Collects probing failures encountered during the test execution for a function that returns a value.
        /// </summary>
        public T? FailLate<T>(Func<T> func, string? label = null) {
            try
            {
                // Execute the action that may throw an exception
                return func();
            }
            catch (Exception ex)
            {
                // Record the probing failure with the label and exception
                RecordProbingFailure(label, func, ex);
            }
            return default;
        }

        /// <summary>
        /// Asynchronously collects probing failures encountered during the test execution.
        /// </summary>
        public async Task FailLateAsync(Func<Task> act, string? label = null)
        {
            try
            {
                // Execute the asynchronous action that may throw an exception
                await act();
            }
            catch (Exception ex)
            {
                // Record the probing failure with the label and exception
                RecordProbingFailure(label, act, ex);
            }
        }

        /// <summary>
        /// Asynchronously collects probing failures encountered during the test execution for multiple actions. 
        /// </summary>
        public async Task FailLateAsync(params Func<Task>[] actions)
        {
            // Iterate through each action and execute FailLateAsync for each
            foreach (var action in actions)
            {
                await FailLateAsync(action);
            }
        }

        /// <summary>
        /// Collects probing failures encountered during the test execution.
        /// </summary>
        public IEnumerable<IProbingFailure> CollectProbingFailure()
        {
            // Return a copy of the probing failures to avoid external modifications
            return ProbingFailures.ToArray();
        }
    }
}
