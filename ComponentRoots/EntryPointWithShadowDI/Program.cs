using Cozyupk.HelloShadowDI.ComponentMeta.Utils;
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
            // Create Unity container
            IUnityContainer container = new UnityContainer();

            // Bind using Shadow DI
            var injector = new DynamicShadowInjector(AppContext.BaseDirectory);
            injector.Inject(container);

            // Retrieve an instance of IMessageWriteDispatcher via DI
            var dispatcher = container.Resolve<IMessageWriterDispatcher>();

            // Display the message
            dispatcher.DispatchMessage();
        }
    }
}
