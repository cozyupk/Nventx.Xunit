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
        public TLabelAxes? label { get; }
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
            label = label;
            CombinedPosition = combinedPosition;
            DelegatePosition = delegatePosition;
        }

        /// <summary>
        /// Returns a string representation of the probe scope record,
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            // 1) Headline with location and optional label
            var fileName = Path.GetFileName(CallerFilePath);
            if (string.IsNullOrEmpty(fileName)) fileName = "?";

            sb.Append($"{fileName}: {CallerLineNumber}: {CallerMemberName}");
            if (label != null)
            {
                sb.Append($" [${JsonSerializer.Serialize(label, _jsonOptions)}]");
            }
            sb.Append(Environment.NewLine);

            // 2) Probe method + parameters + invocation kind
            // TODO: it would be nice to output the class name as well, but that would require more work
            sb.Append($"{InvokedProbeMethod.Name}({FormatParameters(InvocationParameters)}) / {InvocationKind}");
            sb.Append(Environment.NewLine);

            // 3) Positions, if any
            if (CombinedPosition != null)
                sb.AppendLine($"Combined Position: {CombinedPosition}");
            if (DelegatePosition != null)
                sb.AppendLine($"Delegate Position: {DelegatePosition}");

            // 4) Full path at the end for IDE click‑through
            sb.Append($"Full path to caller source, line {CallerLineNumber}: {CallerFilePath}");

            return sb.ToString();
        }
    }
}
