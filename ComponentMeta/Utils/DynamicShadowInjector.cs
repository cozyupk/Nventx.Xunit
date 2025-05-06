using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Cozyupk.HelloShadowDI.ComponentMeta.Attributes;
using Unity;
using Unity.Lifetime;

namespace Cozyupk.HelloShadowDI.ComponentMeta.Utils
{
    public class DynamicShadowInjector
    {
        public InjectionScope DefaultScope { get; }
        public string RootAssemblyPath { get; }

        public DynamicShadowInjector(string rootAssemblyPath, InjectionScope defaultScope = InjectionScope.Transient)
        {
            DefaultScope = defaultScope;
            RootAssemblyPath = rootAssemblyPath;
        }

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
                            // NOTE: Unity の Scoped は HierarchicalLifetimeManager に依存。
                            // 利用者が Container.CreateChildContainer() を使う必要あり。
                            container.RegisterType(serviceType, type, new HierarchicalLifetimeManager());
                            break;

                        case InjectionScope.Transient:
                            container.RegisterType(serviceType, type);
                            break;

                        default:
                            Console.WriteLine($"[WARN] Unsupported scope: {scope} in {type.FullName}");
                            continue;
                    }

                    Console.WriteLine($"[ShadowDI] Registered: {serviceType.Name} → {type.FullName} ({scope})");
                }
            }
        }
    }
}