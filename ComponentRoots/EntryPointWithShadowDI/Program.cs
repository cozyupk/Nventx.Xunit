using Cozyupk.HelloShadowDI.ComponentMeta.Utils.Impl;
using Cozyupk.HelloShadowDI.Presentation.Contracts;
using Unity;

namespace Cozyupk.HelloShadowDI.Models.ComponentRoots.EntryPointWithShadowDI
{
    public class Program
    {
        /// <summary>
        /// Entry point of the application.
        /// Initializes the Unity container, performs dependency injection using Shadow DI,
        /// resolves the IMessageWriterDispatcher instance, and dispatches a message.
        /// </summary>
        static void Main(string[] _)
        {
            // for testing
            var analyzer = new AssemblyDependencyAnalyzer();

            // Create Unity container
            IUnityContainer container = new UnityContainer();

            // Bind using Shadow DI
            var injector = new DynamicShadowInjectorBuilder()

                                // Uncomment to disable the default diagnostic observer
                                // .WithoutDefaultDiagnostics()

                                // Uncomment to add a custom diagnostic observer
                                // .AddDiagnosticObserver(new CustomDiagnosticObserver())

                                .Build(AppContext.BaseDirectory);

            injector.Inject(container);

            // Retrieve an instance of IMessageWriteDispatcher via DI
            var dispatcher = container.Resolve<IMessageWriterDispatcher>();

            // Display the message
            dispatcher.DispatchMessage();
        }
    }
}
