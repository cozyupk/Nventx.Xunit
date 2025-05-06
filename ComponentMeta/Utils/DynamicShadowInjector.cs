using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Cozyupk.HelloShadowDI.ComponentMeta.Attributes;
using Unity;
using Unity.Lifetime;

namespace Cozyupk.HelloShadowDI.ComponentMeta.Utils
{
    /// <summary>
    /// A utility class for dynamically injecting dependencies into a Unity container
    /// by scanning assemblies for types marked with the ShadowInjectableAttribute.
    /// </summary>
    public class DynamicShadowInjector
    {
        /// <summary>
        /// Gets the default injection scope to be used when no specific scope is defined.
        /// </summary>
        public InjectionScope DefaultScope { get; }

        /// <summary>
        /// Gets the root path of the assemblies to be scanned for dependency injection.
        /// </summary>
        public string RootAssemblyPath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicShadowInjector"/> class.
        /// </summary>
        /// <param name="rootAssemblyPath">The root directory path containing the assemblies to scan.</param>
        /// <param name="defaultScope">The default injection scope to use if not specified in the attribute.</param>
        public DynamicShadowInjector(string rootAssemblyPath, InjectionScope defaultScope = InjectionScope.Transient)
        {
            DefaultScope = defaultScope;
            RootAssemblyPath = rootAssemblyPath;
        }

        /// <summary>
        /// Scans the specified assemblies for types marked with ShadowInjectableAttribute
        /// and registers them into the provided Unity container.
        /// </summary>
        /// <param name="container">The Unity container to register the dependencies into.</param>
        public void Inject(IUnityContainer container)
        {
            var assemblies = Directory.GetFiles(RootAssemblyPath, "*.dll", SearchOption.AllDirectories)
                                      .Select(path =>
                                      {
                                          try
                                          {
                                              return Assembly.LoadFrom(path);
                                          }
                                          catch (Exception ex)
                                          {
                                              Console.WriteLine($"[WARN] Failed to load assembly: {path} - {ex.Message}");
                                              return null;
                                          }
                                      })
                                      .Where(a => a != null)!;

            foreach (var assembly in assemblies)
            {
                Type[] types;

                if (assembly == null) continue;

                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray()!;
                }

                foreach (var type in types)
                {
                    var attr = type.GetCustomAttribute<ShadowInjectableAttribute>();
                    if (attr == null) continue;

                    var serviceType = attr.ServiceType;
                    var scope = attr.Scope != InjectionScope.Unspecified ? attr.Scope : DefaultScope;

                    switch (scope)
                    {
                        case InjectionScope.Singleton:
                            container.RegisterSingleton(serviceType, type);
                            break;

                        case InjectionScope.Scoped:
                            // NOTE: Unity's Scoped depends on HierarchicalLifetimeManager.
                            // Users need to use Container.CreateChildContainer().
                            container.RegisterType(serviceType, type, new HierarchicalLifetimeManager());
                            break;

                        case InjectionScope.Transient:
                            container.RegisterType(serviceType, type);
                            break;

                        default:
                            Console.WriteLine($"[WARN] Unsupported scope: {scope} in {type.FullName}");
                            continue;
                    }

                    System.Diagnostics.Debug.WriteLine($"[ShadowDI] Registered: {serviceType.Name} → {type.FullName} ({scope})");
                }
            }
        }
    }
}