using System;
using System.Runtime.CompilerServices;
using NventX.xProof.Abstractions.TestProofForTestMethods;

namespace NventX.xProof.BaseProofLibrary.BaseImpl
{
    public abstract partial class DeclarativeCustomizableProof 
        : InvokableProofBase,
          IProofForAction, ICombinerForActions,
          IProofForFunc, ICombinerForFuncs
    {
        private IProofForAction? ProofForAction { get; set; }

        private ICombinerForActions? CombinerForActions { get; set; }

        private IProofForFunc? ProofForFunc { get; set; }
        private ICombinerForFuncs? CombinerForFuncs { get; set; }

        public DeclarativeCustomizableProof ()
        {
            var config = new ProofComponentConfiguration(this);
            InitializeProofComponent(config);
            config.Freeze();
        }

        protected abstract void InitializeProofComponent(IProofComponentConfiguration config);

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
