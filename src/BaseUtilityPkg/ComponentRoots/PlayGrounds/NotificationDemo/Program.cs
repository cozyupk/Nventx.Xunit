using System;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.ComponentRoots.PlayGrounds.NotificationDemo
{
    /// <summary>
    /// Console demo for PayloadMulticastNotifierBuilder and related notification flow.
    /// </summary>
    public class Program
    {
        public static void Main(string[] _)
        {
            Console.WriteLine("== Starting notification demo ==");

            // Create a multicast notifier builder
            var builder = new PayloadMulticastNotifierBuilder<string, string, string>();

            // Add two consumers that print notification to the console
            builder.AddConsumer(new ConsoleConsumer("ConsumerA"));
            builder.AddConsumer(new ConsoleConsumer("ConsumerB"));

            // Build the notifier with a sender meta
            var notifier = builder.Build("ConsoleSender");

            // Register a handler and keep reference for simulation
            var handler = new ConsolePayloadHandler();
            notifier.RegisterHandler(handler);

            // Simulate a payload notification
            var payload = new DemoPayload("MetaInfo", ["Body1", "Body2"]);
            handler.Simulate(payload);

            Console.WriteLine("== Demo complete. Press any key to exit ==");
            Console.ReadKey();
        }

        /// <summary>
        /// Consumer that prints notification to the console.
        /// </summary>
        private class ConsoleConsumer : IPayloadConsumer<string, string, string>
        {
            private readonly string _name;
            public INotifyAdapted<ISenderPayload<string, string, string>> PayloadArrivalNotifier { get; }

            public ConsoleConsumer(string name)
            {
                _name = name;
                PayloadArrivalNotifier = new ConsoleNotifier(_name);
            }

            private class ConsoleNotifier(string consumerName) : INotifyAdapted<ISenderPayload<string, string, string>>
            {
                private readonly string _consumerName = consumerName;

                public void Notify(ISenderPayload<string, string, string> payload)
                {
                    Console.WriteLine($"[{_consumerName}] Notified: Sender={payload.SenderMeta}, Meta={payload.Payload.Meta}, Bodies=[{string.Join(", ", payload.Payload.Bodies)}]");
                }
            }
        }

        /// <summary>
        /// Simple payload implementation.
        /// </summary>
        private class DemoPayload(string meta, IEnumerable<string> bodies) : IPayload<string, string>
        {
            public string Meta { get; } = meta;
            public IEnumerable<string> Bodies { get; } = bodies;
        }

        /// <summary>
        /// Handler that simulates notification and prints to the console.
        /// </summary>
        private class ConsolePayloadHandler : INotificationHandler<IPayload<string, string>>
        {
            private Action<IPayload<string, string>>? _handle;

            public Action<IPayload<string, string>> Handle
            {
                set => _handle = value;
            }

            public void Simulate(IPayload<string, string> payload)
            {
                _handle?.Invoke(payload);
            }
        }
    }
}
