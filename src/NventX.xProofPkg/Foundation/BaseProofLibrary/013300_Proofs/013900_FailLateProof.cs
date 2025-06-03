using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NventX.xProof.Utils;

/*
namespace NventX.xProof.BaseProofLibrary
{
    /// <summary>
    /// Interface for provable elements that can be probed for failures.
    /// </summary>
    public interface IProvable
    {
        void Probe(
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        );
    }

    /// <summary>
    /// A proof implementation that allows for late failure collection during test execution.
    /// </summary>
    public class FailLateProof : InvokableProofBase
    {


        /// <summary>
        /// A class that encapsulates a collection of functions that return values to be probed for failures.
        /// </summary>
        protected internal class ProvableFunctions<T> : IProvable
        {
            /// <summary>
            /// The test proof instance that this collection belongs to.
            /// </summary>
            ITestProof TestProof { get; }

            /// <summary>
            /// An array of functions that will be probed for failures.
            /// </summary>
            Func<T>[] Functions { get; }

            public ProvableFunctions(ITestProof testProof, Func<T>[] funcs)
            {
                // Validate and assign the test proof and functions
                TestProof = testProof ?? throw new ArgumentNullException(nameof(testProof), "TestProof cannot be null.");
                Functions = funcs ?? throw new ArgumentNullException(nameof(funcs), "Funcs cannot be null.");
            }

            /// <summary>
            /// Probes each action in the collection for failures, executing them and collecting any exceptions that occur.
            /// </summary>
            public virtual IEnumerable<T?> Probe(
                [CallerFilePath] string? callerFilePath = null,
                [CallerLineNumber] int callerLineNumber = 0,
                [CallerMemberName] string? callerMemberName = null
            )
            {
                // Create a concurrent queue to store the results of the functions
                ConcurrentQueue<T?> results = new();

                // Iterate through each function and execute FailLate for each
                foreach (var func in Functions)
                {
                    try
                    {
                        // Execute the action that may throw an exception
                        results.Enqueue(func());
                    }
                    catch (Exception ex)
                    {
                        // Record the probing failure with the label and exception
                        TestProof.RecordProbingFailure(null, func, ex, callerFilePath, callerLineNumber, callerMemberName);
                        results.Enqueue(default);
                    }
                }

                // Return the results as an enumerable
                return results.ToArray();
            }
        }

        /// <summary>
        /// Combines multiple actions into a single provable element that can be probed for failures.
        /// </summary>
        public IProvable Combine(params Action[] actions)
        {
            return new ProvableActions(this, actions);
        }

        public IProvable Combine<T>(params Func<T>[] functions)
        {
            return new ProvableFunctions<T>(this, functions);
        }

        public Func<Task>[] Combine(params Func<Task>[] tasks)
        {
        }

        /// <summary>
        /// Collects probing failures encountered during the test execution.
        /// </summary>
        public void FailLate(
            Action act, string? label = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        ) {
            try
            {
                // Execute the action that may throw an exception
                act();
            }
            catch (Exception ex)
            {
                // Record the probing failure with the label and exception
                RecordProbingFailure(label, act, ex, callerFilePath, callerLineNumber, callerMemberName);
            }
        }

        /// <summary>
        /// Collects probing failures encountered during the test execution for a function that returns a value.
        /// </summary>
        public T? FailLate<T>(
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
                RecordProbingFailure(label, func, ex, callerFilePath, callerLineNumber, callerMemberName);
            }
            return default;
        }

        /// <summary>
        /// Collects probing failures encountered during the test execution for multiple functions that return values.
        /// </summary>
        public IEnumerable<T?> FailLate<T>(
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            params Func<T>[] funcs
        ) {


            // Return the results as an enumerable
            return results;
        }

        /// <summary>
        /// Asynchronously collects probing failures encountered during the test execution.
        /// </summary>
        public async Task FailLateAsync(
            Func<Task> act, 
            string? label = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        ) {
            try
            {
                // Execute the asynchronous action that may throw an exception
                await act();
            }
            catch (Exception ex)
            {
                // Record the probing failure with the label and exception
                RecordProbingFailure(label, act, ex, callerFilePath, callerLineNumber, callerMemberName);
            }
        }

        /// <summary>
        /// Asynchronously collects probing failures encountered during the test execution for multiple actions. 
        /// </summary>
        public async Task FailLateAsync(
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            params Func<Task>[] funcs
        ) {
            // Iterate through each action and execute FailLateAsync for each
            foreach (var action in funcs)
            {
                await FailLateAsync(action, null, callerFilePath, callerLineNumber, callerMemberName);
            }
        }
    }
}
*/