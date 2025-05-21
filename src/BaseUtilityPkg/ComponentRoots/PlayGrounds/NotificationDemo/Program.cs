using System;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.ComponentRoots.PlayGrounds.NotificationDemo
{
    /// <summary>
    /// Console demo for PayloadMulticastNotifier including:
    /// - Multicast delivery
    /// - Payload metadata and sender info propagation
    /// - Consumer registration via builder
    /// - Handler simulation
    /// - Console-based notification tracing
    /// </summary>
    public class Program
    {
        public static void Main(string[] _)
        {
            Console.WriteLine("== Starting Shadow.NotifyConsume demo ==");
            Console.WriteLine("Features demonstrated:");
            Console.WriteLine("- Multicast notifier");
            Console.WriteLine("- Builder-based consumer registration");
            Console.WriteLine("- Payload metadata and sender binding");
            Console.WriteLine("- Manual handler triggering");
            Console.WriteLine("- Console output for notification events");
            Console.WriteLine();

            // Create a multicast notifier builder
            var builder = new PayloadMulticastNotifierBuilder<string, string, string>();

            // Add two consumers that print notifications
            builder.AddConsumer(new ConsoleConsumer("ConsumerA"));
            builder.AddConsumer(new ConsoleConsumer("ConsumerB"));

            // Build the notifier with a sender meta
            var notifier = builder.Build("ConsoleSender");

            // Register a handler and keep reference for simulation
            var handler = new ConsolePayloadHandler();
            notifier.RegisterHandler(handler);

            // Simulate payload notification
            var payload = new DemoPayload("MetaInfo", ["Body1", "Body2"]);
            handler.Simulate(payload);

            Console.WriteLine();
            Console.WriteLine("== Demo complete. Press any key to exit ==");
            Console.ReadKey();
        }

        /// <summary>
        /// Console output consumer (simple observer).
        /// </summary>
        private class ConsoleConsumer : IPayloadConsumer<string, string, string>
        {
            private readonly string _name;
            public IUnicastNotifier<ISenderPayload<string, string, string>> PayloadArrivalNotifier { get; }

            public ConsoleConsumer(string name)
            {
                _name = name;
                PayloadArrivalNotifier = new ConsoleNotifier(_name);
            }

            private class ConsoleNotifier(string consumerName) : IUnicastNotifier<ISenderPayload<string, string, string>>
            {
                private readonly string _consumerName = consumerName;

                public void Notify(ISenderPayload<string, string, string> payload)
                {
                    Console.WriteLine($"[{_consumerName}] Notified:");
                    Console.WriteLine($"  SenderMeta: {payload.SenderMeta}");
                    Console.WriteLine($"  PayloadMeta: {payload.Payload.Meta}");
                    Console.WriteLine($"  Bodies: {string.Join(", ", payload.Payload.Bodies)}");
                }
            }
        }

        /// <summary>
        /// Simple IPayload implementation.
        /// </summary>
        private class DemoPayload(string meta, IEnumerable<string> bodies) : IPayload<string, string>
        {
            public string Meta { get; } = meta;
            public IEnumerable<string> Bodies { get; } = bodies;
        }

        /// <summary>
        /// Simulates external payload emission.
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
