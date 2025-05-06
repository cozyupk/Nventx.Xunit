using Cozyupk.HelloShadowDI.Models.Contracts;
using Cozyupk.HelloShadowDI.Models.Impl;
using Cozyupk.HelloShadowDI.Presentation.Contracts;
using Cozyupk.HelloShadowDI.Presentation.Impl;
using Unity;

namespace Cozyupk.HelloShadowDI.Models.ComponentRoots.EntryPointWithoutShadowDI
{
    public class Program
    {
        /// <summary>
        /// Entry point of the application demonstrating manual dependency injection using Unity container.
        /// </summary>
        static void Main(string[] _)
        {
            // Create Unity container
            IUnityContainer container = new UnityContainer();

            // Manually bind required implementations
            container.RegisterType<IMessageModel, MessageModel>();
            container.RegisterType<IMessageWriterDispatcher, MessageWriterDispatcher>();

            // Resolve IMessageWriterDispatcher instance via DI
            var dispatcher = container.Resolve<IMessageWriterDispatcher>();

            // Display message
            dispatcher.DispatchMessage();
        }
    }
}
