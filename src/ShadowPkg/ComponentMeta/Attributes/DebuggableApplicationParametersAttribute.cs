using System;

namespace Cozyupk.HelloShadowDI.ShadowPkg.ComponentMeta.Attributes
{
    /// <summary>
    /// An attribute to define application parameters related to debugging.
    /// This attribute allows specifying whether the application should target debug output.
    /// The value is injected as a string (e.g., by MSBuild) and internally converted to a boolean for type-safe access.
    /// </summary>
    public sealed class DebuggableApplicationParametersAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the debug output target as a string.
        /// The value is parsed into a boolean to determine the debug output target state.
        /// If the provided value is not a valid boolean string, an exception is thrown.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided value cannot be parsed into a boolean.
        /// </exception>
        public string IsDebugOutputTarget
        {
            get => _isDebugOutputTargetString;
            set
            {
                // Store the raw string value
                _isDebugOutputTargetString = value;

                // Attempt to parse the string into a boolean
                if (!bool.TryParse(value, out var result))
                {
                    // Throw an exception if parsing fails
                    throw new ArgumentException(
                        $"Invalid boolean string for IsDebugOutputTarget: '{value}'", nameof(value));
                }

                // Update the internal boolean state
                _isDebugOutputTarget = result;
            }
        }

        // Internal state for string and bool
        private string _isDebugOutputTargetString = "true";
        private bool _isDebugOutputTarget = true;

        /// <summary>
        /// Gets the debug output target as a boolean.
        /// This property provides a type-safe way to access the debug output target state.
        /// </summary>
        public bool IsDebugOutputTargetBool => _isDebugOutputTarget;
    }
}
