using System.Collections.Generic;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Impl;
using Unity;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.ComponentRoots.ContainerBuilder.PkgContainerBuilder.ComponentRoots
{
    public class DiagnosticContainerBuilder
    {
        private IUnityContainer Container { get; } = new UnityContainer();
        private bool UseDefaultObserver { get; set; } = true;
        private List<IShadowDiagnosticObserver> CustomObservers { get; } = new();
        private bool UseMockNotifier { get; set; } = false;
        private ShadowDiagnosticLevel LogLevel { get; set; } = ShadowDiagnosticLevel.Info;

        public DiagnosticContainerBuilder WithoutDefaultDiagnosticsObserver()
        {
            UseDefaultObserver = false;
            return this;
        }

        public DiagnosticContainerBuilder AddDiagnosticsObserver(IShadowDiagnosticObserver observer)
        {
            CustomObservers.Add(observer);
            return this;
        }

        public DiagnosticContainerBuilder WithMockNotifier()
        {
            UseMockNotifier = true;
            return this;
        }

        public DiagnosticContainerBuilder WithLogLevel(ShadowDiagnosticLevel level)
        {
            LogLevel = level;
            return this;
        }

        public IUnityContainer Build()
        {
            if (CustomObservers != null)
            {
                foreach (var obs in CustomObservers)
                {
                    Container.RegisterInstance<IShadowDiagnosticObserver>(obs);
                }
            }
            else if (UseDefaultObserver)
            {
                Container.RegisterType<IShadowDiagnosticObserver, DefaultShadowDiagnosticObserver>();
            }

            if (UseMockNotifier)
            {
                // Container.RegisterType<IShadowDiagnosticNotifier, MockDiagnosticNotifier>();
            }
            else
            {
                Container.RegisterType<IShadowDiagnosticNotifierProvider, ShadowDiagnosticNotifierProvider>();
            }

            Container.RegisterInstance(LogLevel);
            return Container;
        }
    }
}