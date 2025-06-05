using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NventX.xProof.Abstractions.TestProofForTestMethods;

namespace NventX.xProof.BaseProofLibrary.DefaultModules
{
    internal class CombinerForFuncs : ICombinerForFuncs
    {
        /// <summary>
        /// The proof for function that this collection belongs to.
        /// </summary>
        IProofForFunc ProofForFunc { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinerForFuncs"/> class with the specified proof for function.
        /// </summary>
        public CombinerForFuncs(IProofForFunc proofForFunc)
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

    internal class ProvableFuncs<T> : IProvable<T>
    {
        /// <summary>
        /// The test proof instance that this collection belongs to.
        /// </summary>
        IProofForFunc TestProof { get; }

        /// <summary>
        /// An array of functions that will be probed for failures.
        /// </summary>
        Func<T>[] Funcs { get; }

        /// <summary>
        /// Represents a collection of functions that can be probed for results and failures.
        /// </summary>
        public ProvableFuncs(IProofForFunc testProof, Func<T>[] functions)
        {
            // Validate and assign the test proof and actions
            TestProof = testProof ?? throw new ArgumentNullException(nameof(testProof), "TestProof cannot be null.");
            Funcs = functions ?? throw new ArgumentNullException(nameof(functions), "Functions cannot be null.");
        }

        /// <summary>
        /// Probes the functions for failures and returns the results.
        /// </summary>
        public IEnumerable<T?> Probe(
            string? label = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        ) {
            // Iterate through each action and execute FailLate for each
            List<T?> results = new(Funcs.Length);
            foreach (var func in Funcs)
            {
                var retval = TestProof.Probe
                     (func, label, callerFilePath, callerLineNumber, callerMemberName);
                results.Add(retval);
            }
            return results;
        }
    }
}
