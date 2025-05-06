using System;

namespace Cozyupk.HelloShadowDI.ComponentMeta.Attributes
{
    public enum InjectionScope
    {
        Unspecified,
        Singleton,
        Scoped,
        Transient
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ShadowInjectableAttribute : Attribute
    {
        public Type ServiceType { get; }

        public InjectionScope Scope { get; }

        public ShadowInjectableAttribute(Type serviceType, InjectionScope scope = InjectionScope.Unspecified)
        {
            ServiceType = serviceType;
            Scope = scope;
        }
    }
}
