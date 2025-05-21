using System;

namespace Cozyupk.Shadow.Flow.ShadowPkg.ComponentMeta.Attributes
{
    /// <summary>
    /// Specifies the injection metadata for a class to be used with dependency injection.
    /// </summary>
    public enum InjectionScope
    {
        /// <summary>
        /// The injection scope is not specified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// A single instance is created and shared across the application.
        /// </summary>
        Singleton,

        /// <summary>
        /// A new instance is created for each scope.
        /// </summary>
        Scoped,

        /// <summary>
        /// A new instance is created every time it is requested.
        /// </summary>
        Transient
    }

    /// <summary>
    /// An attribute to mark a class as injectable with a specified service type and scope.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ShadowInjectableAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the service to be injected.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Gets the scope of the injection.
        /// </summary>
        public InjectionScope Scope { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShadowInjectableAttribute"/> class.
        /// </summary>
        /// <param name="serviceType">The type of the service to be injected.</param>
        /// <param name="scope">The scope of the injection. Defaults to <see cref="InjectionScope.Unspecified"/>.</param>
        public ShadowInjectableAttribute(Type serviceType, InjectionScope scope = InjectionScope.Unspecified)
        {

            // Set the properties
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType), "Service type cannot be null.");
            Scope = scope;
        }
    }
}
