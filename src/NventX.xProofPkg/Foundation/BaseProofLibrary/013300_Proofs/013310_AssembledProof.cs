using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using NventX.xProof.Abstractions.TestProofForTestMethods;
using NventX.xProof.BaseProofLibrary.BaseImpl;

namespace NventX.xProof.BaseProofLibrary.Proofs
{
    public class AssembledProof
        : InvokableProofBase,
          IProofForAction, ICombinerForActions,
          IProofForFunc, ICombinerForFuncs
    {
        private IProofForAction? ProofForAction { get; set; }

        private ICombinerForActions? CombinerForActions { get; set; }

        private IProofForFunc? ProofForFunc { get; set; }
        private ICombinerForFuncs? CombinerForFuncs { get; set; }

        private ConcurrentDictionary<string, object> LockObjects { get; } = new ConcurrentDictionary<string, object>()
        {
            [nameof(ProofForAction)] = new object(),
            [nameof(CombinerForActions)] = new object(),
            [nameof(ProofForFunc)] = new object(),
            [nameof(CombinerForFuncs)] = new object()
        };

        public void InjectProofForAction(IProofForAction proofForAction)
        {
            lock (LockObjects[nameof(ProofForAction)])
            {
                if (ProofForAction != null)
                {
                    throw new InvalidOperationException($"{nameof(ProofForAction)} is already injected and cannot be changed.");
                }
                ProofForAction = proofForAction
                                 ?? throw new ArgumentNullException(nameof(proofForAction), $"{nameof(ProofForAction)} cannot be null.");
            }
        }

        public void InjectCombinerForActions(ICombinerForActions combinerForActions)
        {
            lock (LockObjects[nameof(CombinerForActions)])
            {
                if (CombinerForActions != null)
                {
                    throw new InvalidOperationException($"CombinerForActions is already injected and cannot be changed.");
                }
                CombinerForActions = combinerForActions
                                     ?? throw new ArgumentNullException(nameof(combinerForActions), $"{nameof(CombinerForActions)} cannot be null.");
            }
        }

        public void InjectProofForFunc(IProofForFunc proofForFunc)
        {
            lock (LockObjects[nameof(ProofForFunc)])
            {
                if (ProofForFunc != null)
                {
                    throw new InvalidOperationException($"{nameof(ProofForFunc)} is already injected and cannot be changed.");
                }
                ProofForFunc = proofForFunc
                               ?? throw new ArgumentNullException(nameof(proofForFunc), $"{nameof(ProofForFunc)} cannot be null.");
            }
        }

        public void InjectCombinerForFuncs(ICombinerForFuncs combinerForFuncs)
        {
            lock (LockObjects[nameof(CombinerForFuncs)])
            {
                if (CombinerForFuncs != null)
                {
                    throw new InvalidOperationException($"{nameof(CombinerForFuncs)} is already injected and cannot be changed.");
                }
                CombinerForFuncs = combinerForFuncs
                                   ?? throw new ArgumentNullException(nameof(combinerForFuncs), $"{nameof(CombinerForFuncs)} cannot be null.");
            }
        }

        public void Probe(
            Action act, 
            string? label = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        ) {
            if (ProofForAction == null)
            {
                throw new NotImplementedException($"{nameof(ProofForAction)} is not implemented {this.GetType().FullName}.");
            }
            ProofForAction.Probe(act, label, callerFilePath, callerLineNumber, callerMemberName);
        }

        public IProvable Combine(Action[] actions)
        {
            return CombinerForActions?.Combine(actions)
                   ?? throw new NotImplementedException($"{nameof(CombinerForActions)} is not implemented {this.GetType().FullName}.");
        }

        public T? Probe<T>(
            Func<T> func,
            string? label = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        ) {
            if (ProofForFunc == null)
            {
                throw new NotImplementedException($"{nameof(ProofForFunc)} is not implemented {this.GetType().FullName}.");
            }
            return ProofForFunc.Probe(func, label, callerFilePath, callerLineNumber, callerMemberName);
        }

        public IProvable<T> Combine<T>(params Func<T>[] functions)
        {
            return CombinerForFuncs?.Combine(functions)
                   ?? throw new NotImplementedException($"{nameof(CombinerForFuncs)} is not implemented {this.GetType().FullName}.");
        }
    }
}
