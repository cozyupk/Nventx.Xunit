using System;
using NventX.xProof.Abstractions.Utils;
using Xunit.Abstractions;

namespace NventX.xProof.ForXunit
{
    /// <summary>
    /// A wrapper for <see cref="IAttributeInfo"/> that implements <see cref="INamedArgumentResolver"/>.
    /// </summary>
    public class XunitAttributeInfoWrapper : INamedArgumentResolver
    {
        /// <summary>
        /// The underlying attribute information that this wrapper encapsulates.
        /// </summary>
        private IAttributeInfo AttributeInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitAttributeInfoWrapper"/> class with the specified attribute information.
        /// </summary>
        public XunitAttributeInfoWrapper(IAttributeInfo attributeInfo)
        {
            // Validate the argument
            _ = attributeInfo ?? throw new ArgumentNullException(nameof(attributeInfo));

            // Initialize the wrapper with the provided attribute info
            AttributeInfo = attributeInfo ?? throw new ArgumentNullException(nameof(attributeInfo));
        }

        /// <summary>
        /// Resolves a named argument of type T from the attribute information.
        /// </summary>
        public T Resolve<T>(string name) where T : notnull
            => AttributeInfo.GetNamedArgument<T>(name);
    }
}
