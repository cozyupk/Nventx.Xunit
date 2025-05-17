using System;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl
{
    /// <summary>
    /// Arguments for creating a DefaultShadowDiagnosableMeta instance.
    /// </summary>
    public class DefaultShadowDiagnosableCreateArgs
    {
        /// <summary>
        /// The type of the sender object.
        /// </summary>
        public Type? SenderType { get; set; }

        /// <summary>
        /// The hash code of the sender object.
        /// </summary>
        public int? SenderHashCode { get; set; }
    }

    /// <summary>
    /// Factory for creating DefaultShadowDiagnosableMeta instances.
    /// </summary>
    public class DefaultShadowDiagnosableMetaFactory : IShadowDiagnosableMetaFactory<DefaultShadowDiagnosableCreateArgs>
    {
        /// <summary>
        /// Implementation of IShadowDiagnosableMeta for default diagnosable metadata.
        /// </summary>
        protected internal class DefaultShadowDiagnosableMeta : IShadowDiagnosableMeta
        {
            /// <summary>
            /// Gets a human-readable label representing this diagnosable component.
            /// </summary>
            public string Label { get; }

            /// <summary>
            /// Initializes a new instance of the DefaultShadowDiagnosableMeta class.
            /// </summary>
            /// <param name="senderType">The type of the sender object.</param>
            /// <param name="senderHashCode">The hash code of the sender object.</param>
            public DefaultShadowDiagnosableMeta(Type? senderType, int? senderHashCode)
            {
                var sb = new System.Text.StringBuilder();

                // Build the label using the sender type name or a default value if null.
                sb.Append('(');
                sb.Append(senderType?.Name ?? "[Unknown]");

                // Append the sender hash code if it is provided.
                if (senderHashCode.HasValue)
                {
                    sb.Append('/');
                    sb.Append(senderHashCode.Value.ToString("x"));
                }

                // Close the label string.
                sb.Append(')');

                // Set the Label property.
                Label = sb.ToString();
            }
        }

        /// <summary>
        /// Creates a DefaultShadowDiagnosableMeta instance from the specified arguments.
        /// </summary>
        /// <param name="args">The arguments for creating the diagnosable meta instance.</param>
        /// <returns>An instance of IShadowDiagnosableMeta.</returns>
        /// <exception cref="ArgumentNullException">Thrown if args is null.</exception>
        public IShadowDiagnosableMeta Create(DefaultShadowDiagnosableCreateArgs args)
        {
            // Validate the input arguments.
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Arguments cannot be null.");
            }

            // Return a new meta instance.
            return new DefaultShadowDiagnosableMeta(
                args.SenderType,
                args.SenderHashCode
            );
        }
    }
}
