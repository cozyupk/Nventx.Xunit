using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Cozyupk.Shadow.Flow.DiagnosticPkg.Models.Framework.Contracts;

/*
namespace Cozyupk.Shadow.Flow.DiagnosticPkg.ComponentRoots.Common.Impl.Models
{
    public static class DiagnosableModelsFactory
    {
        public static IEnumerable<DiagnosableModelBase> Create(IShadowDiagnosticNotifierProvider sdnp)
        {
            return new DiagnosableModelBase[] {
                new DailyLife(sdnp),
                new Coffee(sdnp),
                new Window(sdnp),
                new Wardrobe(sdnp),
                new Toaster(sdnp),
                new Calendar(sdnp),
                new Health(sdnp),
                new Mail(sdnp),
                new LivingRoom(sdnp),
                new Fridge(sdnp),
                new Gallery(sdnp),
                new Cat(sdnp),
                new Exit(sdnp),
                new Environment(sdnp),
                new Identity(sdnp),
                new Support(sdnp),
                new Weather(sdnp),
                new Status(sdnp)
            };
        }
        public abstract class DiagnosableModelBase
        {
            virtual public string Category => "Bob";
            abstract public string Message { get; }
            abstract public ShadowDiagnosticLevel Level { get; }
            private IShadowDiagnosticNotifier ShadowDiagnosticNotifier { get; }
            private static readonly Random SharedRandom = new();
            public DiagnosableModelBase(IShadowDiagnosticNotifierProvider sdnp)
            {
                ShadowDiagnosticNotifier = sdnp.CreateDiagnosticNotifier(
                                                this,
                                                Category
                                           );
            }
            public async Task StartAsync(CancellationToken cancellationToken = default)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Wait for a random time between 0.5 and 5 seconds
                    var delay = TimeSpan.FromSeconds(SharedRandom.NextDouble() * 4.5 + 0.5);
                    await Task.Delay(delay, cancellationToken);

                    ShadowDiagnosticNotifier.Notify(Message, Level);
                }
            }
        }

        private class DailyLife : DiagnosableModelBase
        {
            public DailyLife(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Trace;
            override public string Message => "Bob opened his eyes at 7:01 AM, just like yesterday.";
        }

        private class Coffee : DiagnosableModelBase
        {
            public Coffee(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            override public string Category => "Bob.Kitchen";
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Trace;
            override public string Message => "The coffee machine clicked once. Bob smiled.";
        }

        private class Window : DiagnosableModelBase
        {
            public Window(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Trace;
            override public string Message => "A pigeon landed on his windowsill. Bob nodded as if they were old friends.";
        }

        private class Wardrobe : DiagnosableModelBase
        {
            public Wardrobe(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Debug;
            override public string Message => "Bob noticed his socks were not the usual shade of navy.";
        }

        private class Toaster : DiagnosableModelBase
        {
            public Toaster(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            override public string Category => "Bob.Kitchen";
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Debug;
            override public string Message => "The toaster took 4 seconds longer than usual.";
        }

        private class Calendar : DiagnosableModelBase
        {
            public Calendar(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Debug;
            override public string Message => "Unexpected meeting detected: “Quarterly Summit with Entities”.";
        }

        private class Health : DiagnosableModelBase
        {
            public Health(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Info;
            override public string Message => "Heart rate increased by 7% compared to baseline.";
        }

        private class Mail : DiagnosableModelBase
        {
            public Mail(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Info;
            override public string Message => "Package received: “Do not open” - no sender.";
        }

        private class LivingRoom : DiagnosableModelBase
        {
            public LivingRoom(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Info;
            override public string Message => "Unknown cat detected sleeping on couch. No prior ownership records found.";
        }

        private class Fridge : DiagnosableModelBase
        {
            public Fridge(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Notice;
            override public string Message => "Fridge emitting Morse code pattern: “.--- ..- ... - / .-. ..- -.” (translation: “just run”).";
        }

        private class Gallery : DiagnosableModelBase
        {
            public Gallery(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override string Category => "Bob.Phone";
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Notice;
            override public string Message => "All photos replaced with images of a duck. No user action recorded.";
        }

        private class Cat : DiagnosableModelBase
        {
            public Cat(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Notice;
            override public string Message => "Cat is now wearing Bob’s tie. Sustained eye contact for 47 seconds.";
        }

        private class Exit : DiagnosableModelBase
        {
            public Exit(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Warning;
            override public string Message => "Exit door redirects to kitchen. Loop suspected.";
        }

        private class Environment : DiagnosableModelBase
        {
            public Environment(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Warning;
            override public string Message => "Sky color mismatch detected: #B452CD (Purple). User-only anomaly.";
        }

        private class Identity : DiagnosableModelBase
        {
            public Identity(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Warning;
            override public string Message => "Name tag updated: “Bob” → “Rob”. No approval workflow triggered.";
        }

        private class Support : DiagnosableModelBase
        {
            public Support(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Error;
            override public string Message => "Tech support call answered by user’s own voice. No external call trace found.";
        }

        private class Weather : DiagnosableModelBase
        {
            public Weather(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Error;
            override public string Message => "Sun blinked unexpectedly. Duration: 0.3s. No celestial event recorded.";
        }

        private class Status : DiagnosableModelBase
        {
            public Status(IShadowDiagnosticNotifierProvider sdnp) : base(sdnp)
            {
            }
            public override ShadowDiagnosticLevel Level => ShadowDiagnosticLevel.Error;
            override public string Message => "Bob is not feeling good.";
        }
    }
}
*/