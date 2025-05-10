using System.Collections.Concurrent;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.ComponentMeta.Attributes;
using Cozyupk.HelloShadowDI.ComponentMeta.Utils.Contracts;

namespace Cozyupk.HelloShadowDI.ComponentMeta.Utils.Impl
{
    /// <summary>
    /// A builder class for configuring and creating instances of DynamicShadowInjector.
    /// </summary>
    public class DynamicShadowInjectorBuilder
    {
        /// <summary>
        /// A thread-safe collection of custom diagnostic observers to be added to the injector.
        /// </summary>
        private ConcurrentBag<IShadowDiagnosticObserver> DiagnosticObservers { get; set; } = new ConcurrentBag<IShadowDiagnosticObserver>();

        /// <summary>
        /// A lock object to ensure thread-safe operations during the build process.
        /// </summary>
        private object LockToBuild { get; } = new object();

        /// <summary>
        /// Indicates whether the default diagnostic observer should be used.
        /// Defaults to true.
        /// </summary>
        private bool UseDefaultObserver { get; set; } = true;

        /// <summary>
        /// Disables the default diagnostic observer.
        /// This method is thread-safe.
        /// </summary>
        /// <returns>The current instance of <see cref="DynamicShadowInjectorBuilder"/> for method chaining.</returns>
        public DynamicShadowInjectorBuilder WithoutDefaultDiagnostics()
        {
            lock (LockToBuild)
            {
                UseDefaultObserver = false;
                return this;
            }
        }

        /// <summary>
        /// Adds a custom diagnostic observer to the builder.
        /// This method is thread-safe.
        /// </summary>
        /// <param name="observer">The diagnostic observer to add.</param>
        /// <returns>The current instance of <see cref="DynamicShadowInjectorBuilder"/> for method chaining.</returns>
        public DynamicShadowInjectorBuilder AddDiagnosticObserver(IShadowDiagnosticObserver observer)
        {
            lock (LockToBuild)
            {
                DiagnosticObservers.Add(observer);
                return this;
            }
        }

        /// <summary>
        /// Builds and returns a configured instance of <see cref="DynamicShadowInjector"/>.
        /// This method is thread-safe.
        /// </summary>
        /// <param name="rootAssemblyPath">The root directory path containing the assemblies to scan.</param>
        /// <param name="defaultScope">The default injection scope to use if not specified in the attribute. Defaults to <see cref="InjectionScope.Transient"/>.</param>
        /// <returns>A configured instance of <see cref="DynamicShadowInjector"/>.</returns>
        public DynamicShadowInjector Build(string rootAssemblyPath, InjectionScope defaultScope = InjectionScope.Transient)
        {
            lock (LockToBuild)
            {
                // Create a list to hold the diagnostic observers.
                var diagnosticObservers = new List<IShadowDiagnosticObserver>();

                // Add the default diagnostic observer if enabled.
                if (UseDefaultObserver)
                    diagnosticObservers.Add(new DefaultDiagnosticObserver());

                // Add all custom diagnostic observers.
                foreach (var obs in DiagnosticObservers)
                    diagnosticObservers.Add(obs);

                // Create DiagnosticNotifierProvider instance to handle diagnostic notifications.
                var diagnosticNotifierProvider
                    = new DiagnosticNotifierProvider(diagnosticObservers);

                // Create a new instance of DynamicShadowInjector with the specified parameters.
                var injector = new DynamicShadowInjector(rootAssemblyPath, diagnosticNotifierProvider, defaultScope);

                // Notify that the build process is completed.
                injector.OnBuildCompleted();

                return injector;
            }
        }
    }
}
