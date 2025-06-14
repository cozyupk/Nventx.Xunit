using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xproof.Abstractions.Sdk;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.BaseProofLibrary.ProofBase
{
    public abstract partial class DeclarativeCustomizableProof 
        : InvokableProofBase,
          IProofForAction, IRawProofForAction, ICombinerForActions,
          IProofForFunc, IRawProofForFunc, ICombinerForFuncs
    {
        private static MethodInfo InvokedMethodInfoForAction { get; } =
            typeof(DeclarativeCustomizableProof)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Single(m =>
                    m.Name == nameof(Probe) &&
                    !m.IsGenericMethodDefinition &&                              // 非ジェネリック
                    m.GetParameters()[0].ParameterType == typeof(Action)         // 先頭引数が Action
                );

        private static MethodInfo InvokedMethodInfoForFunc { get; } =
            typeof(DeclarativeCustomizableProof)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Single(m =>
                    m.Name == nameof(Probe) &&
                    m.IsGenericMethodDefinition &&                               // ジェネリック
                    m.GetParameters()[0].ParameterType.IsGenericType &&
                    m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(Func<>) // 先頭引数が Func<T>
                );
        private IRawProofForAction? ProofForAction { get; set; }

        private ICombinerForActions? CombinerForActions { get; set; }

        private IRawProofForFunc? ProofForFunc { get; set; }
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
            object? axes = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null
        ) {
            if (ProofForAction == null)
            {
                throw new NotImplementedException($"{nameof(ProofForAction)} is not implemented {this.GetType().FullName}.");
            }
            _ = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath), "Caller file path cannot be null.");
            _ = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName), "Caller member name cannot be null.");

            invokedMethodInfo ??= InvokedMethodInfoForAction;
            invokedParameters ??= new object?[] { act, axes, callerFilePath, callerLineNumber, callerMemberName };
            ProofForAction.Probe(act, axes, callerFilePath, callerLineNumber, callerMemberName, invokedMethodInfo, invokedParameters);
        }

        public IProvable Combine(Action[] actions)
        {
            return CombinerForActions?.Combine(actions)
                   ?? throw new NotImplementedException($"{nameof(CombinerForActions)} is not implemented {this.GetType().FullName}.");
        }

        public T? Probe<T>(
            Func<T> func,
            object? axes = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null
        ) {
            if (ProofForFunc == null)
            {
                throw new NotImplementedException($"{nameof(ProofForFunc)} is not implemented {this.GetType().FullName}.");
            }
            _ = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath), "Caller file path cannot be null.");
            _ = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName), "Caller member name cannot be null.");

            invokedMethodInfo ??= InvokedMethodInfoForFunc;
            invokedParameters ??= new object?[] { func, axes, callerFilePath, callerLineNumber, callerMemberName };
            return ProofForFunc.Probe(func, axes, callerFilePath, callerLineNumber, callerMemberName, invokedMethodInfo, invokedParameters);
        }

        public IProvable<T> Combine<T>(params Func<T>[] functions)
        {
            return CombinerForFuncs?.Combine(functions)
                   ?? throw new NotImplementedException($"{nameof(CombinerForFuncs)} is not implemented {this.GetType().FullName}.");
        }

        public void Probe(Action act, object? axes, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string? callerMemberName = null)
        {
            throw new NotImplementedException();
        }
    }
}
