using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.BaseProofLibrary.ScopeAndResults
{
    /// <summary>
    /// Immutable record of a single probe execution, enriched with caller information and positions.
    /// </summary>
    public sealed class ProbeScopeRecord<TLabelAxes> : IProbeScopeRecord<TLabelAxes>
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            // Relaxed encoder so we don't get \uXXXX for Japanese etc.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            // Skip extra whitespaces – we want something compact for test output
            WriteIndented = false
        };

        /// <summary>
        /// Converts an array of objects into a comma‑separated string where
        /// strings are JSON‑encoded (thereby handling quotes, newlines, etc.).
        /// Non‑strings fall back to <c>ToString()</c>.
        /// </summary>
        private static string FormatParameters(object?[] parameters)
        {
            return string.Join(", ", parameters.Select(p => p switch
            {
                null => "null",
                string s => JsonSerializer.Serialize(s, _jsonOptions), // e.g. "apple"
                _ => p.ToString()
            }));
        }
        public MethodInfo InvokedProbeMethod { get; }
        public object?[] InvocationParameters { get; }
        public ProofInvocationKind InvocationKind { get; }
        public string CallerFilePath { get; }
        public int CallerLineNumber { get; }
        public string CallerMemberName { get; }
        public TLabelAxes? Label { get; }
        public IPositionInArray? CombinedPosition { get; }
        public IPositionInArray? DelegatePosition { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeScopeRecord"/> class with the specified parameters.
        /// </summary>
        public ProbeScopeRecord(
            ProofInvocationKind invocationKind,
            MethodInfo invokedProbeMethod,
            object?[] invocationParameters,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            TLabelAxes? label,
            IPositionInArray? combinedPosition,
            IPositionInArray? delegatePosition)
        {
            // Guard clauses
            InvokedProbeMethod = invokedProbeMethod ?? throw new ArgumentNullException(nameof(invokedProbeMethod));
            InvocationParameters = invocationParameters?.ToArray() // defensive copy
                ?? throw new ArgumentNullException(nameof(invocationParameters));
            InvocationKind = invocationKind;
            CallerFilePath = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath));
            CallerLineNumber = callerLineNumber;
            CallerMemberName = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName));
            Label = label;
            CombinedPosition = combinedPosition;
            DelegatePosition = delegatePosition;
        }

        private static string FormatType(Type t)
        {
            if (t.IsGenericType)
            {
                var name = t.Name.Split('`')[0];
                var inner = string.Join(", ", t.GetGenericArguments().Select(FormatType));
                return $"{name}<{inner}>";
            }

            // プリミティブ型の短縮表記などお好みで
            return t == typeof(int) ? "int"
                 : t == typeof(string) ? "string"
                 : t == typeof(bool) ? "bool"
                 : t.Name;
        }

        /// <summary>
        /// Formats the method name, including generic type parameters if applicable.
        /// </summary>
        private static string FormatMethodNameWithGenerics(MethodInfo method)
        {
            if (!method.IsGenericMethod)
                return method.Name;

            bool isConstructed = !method.ContainsGenericParameters;

            if (!isConstructed)
                return $"{method.Name}(opened)<{string.Join(", ", method.GetGenericArguments().Select(a => a.Name))}>";

            var genericArgs = method.GetGenericArguments();
            var genericPart = string.Join(", ", genericArgs.Select(FormatType));
            return $"{method.Name}<{genericPart}>";
        }


        /// <summary>
        /// Returns a string representation of the probe scope record,
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            // 1) Start with the probe method name
            if (Label != null)
            {
                sb.Append($"[${JsonSerializer.Serialize(Label, _jsonOptions)}] ");
            }
            var fileName = Path.GetFileName(CallerFilePath);
            if (string.IsNullOrEmpty(fileName)) fileName = "?";

            // 2) Format the method invocation with file name, line number, and method name
            sb.AppendLine($"{fileName}: {CallerLineNumber}: {FormatMethodNameWithGenerics(InvokedProbeMethod)}({FormatParameters(InvocationParameters)})");

            // 3) Invocation kind and caller member name
            // TODO: it would be nice to output the class name as well, but that would require more work
            sb.Append($"{CallerMemberName} / {InvocationKind}");
            sb.Append(Environment.NewLine);

            // 4) Positions, if any
            if (CombinedPosition != null)
                sb.AppendLine($"Combined Position: {CombinedPosition}");
            if (DelegatePosition != null)
                sb.AppendLine($"Delegate Position: {DelegatePosition}");

            // 5) Full path at the end for IDE click‑through
            sb.Append($"Full path to caller source, line {CallerLineNumber}: {CallerFilePath}");

            return sb.ToString();
        }
    }
}
