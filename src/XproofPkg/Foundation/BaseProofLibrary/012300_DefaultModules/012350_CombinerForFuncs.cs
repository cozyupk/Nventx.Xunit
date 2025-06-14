using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xproof.Abstractions.Sdk;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.BaseProofLibrary.DefaultModules
{
    internal class CombinerForFuncs : ICombinerForFuncs
    {
        /// <summary>
        /// The proof for function that this collection belongs to.
        /// </summary>
        IRawProofForFunc ProofForFunc { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinerForFuncs"/> class with the specified proof for function.
        /// </summary>
        public CombinerForFuncs(IRawProofForFunc proofForFunc)
        {
            // Validate and assign the proof for action
            ProofForFunc = proofForFunc
                ?? throw new ArgumentNullException(nameof(proofForFunc), "ProofForFunc cannot be null.");
        }

        /// <summary>
        /// Combines multiple functions into a single provable instance.
        /// </summary>
        public IProvable<T> Combine<T>(params Func<T>[] functions)
        {
            return new ProvableFuncs<T>(ProofForFunc, functions);
        }
    }

    internal class ProvableFuncs<T> : IProvable<T>, IRawCombinedProvable<T>
    {
        /// <summary>
        /// The method info for the invoked method that will be used to record probings.
        /// </summary>
        static MethodInfo InvokedMethodInfo { get; }
            = typeof(CombinerForActions).GetMethod(nameof(Probe), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
              ?? throw new InvalidOperationException("Method 'Probe' not found in ProofForAction class.");

        /// <summary>
        /// The test proof instance that this collection belongs to.
        /// </summary>
        IRawProofForFunc TestProof { get; }

        /// <summary>
        /// An array of functions that will be probed for failures.
        /// </summary>
        Func<T>[] Funcs { get; }

        /// <summary>
        /// Represents a collection of functions that can be probed for results and failures.
        /// </summary>
        public ProvableFuncs(IRawProofForFunc testProof, Func<T>[] functions)
        {
            // Validate and assign the test proof and actions
            TestProof = testProof ?? throw new ArgumentNullException(nameof(testProof), "TestProof cannot be null.");
            Funcs = functions ?? throw new ArgumentNullException(nameof(functions), "Functions cannot be null.");
        }

        /// <summary>
        /// Probes the functions for failures and returns the results.
        /// </summary>
        public IEnumerable<T?> Probe(
            object? axes = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null
        ) {
            // Validate the parameters (CallerXXX also must not be null.)
            _ = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath), "Caller file path cannot be null.");
            _ = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName), "Caller member name cannot be null.");

            // Use the provided invokedMethodInfo or fall back to the default one
            invokedMethodInfo ??= InvokedMethodInfo;

            // Iterate through each action and execute FailLate for each
            List<T?> results = new(Funcs.Length);

            // Declare a combined position to track the index and total count
            (int Index, int TotalCount) combinedPosition = (1, Funcs.Length);

            // Probe each function and collect the results
            foreach (var func in Funcs)
            {
                object?[] parameters
                            = invokedParameters
                                ?? new object?[] { func, axes, callerFilePath, callerLineNumber, callerMemberName };
                var retval = TestProof.Probe
                     (func, axes, callerFilePath, callerLineNumber, callerMemberName, invokedMethodInfo, parameters, combinedPosition);
                results.Add(retval);
                ++combinedPosition.Index;
            }
            return results;
        }
    }
}
