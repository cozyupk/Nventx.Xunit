using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Adapters.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.ComponentRoots.Common.Impl.Models;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Impl;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.ComponentRoots.Playgrounds.DiagnosingBob
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Create a new instance of DiagnosticMessageFactory
            var messageFactory = new ShadowDiagnosticMessageFactory();

            // Create new instance of ShadowDiagnosticNotifierProvider
            var sdnp = new ShadowDiagnosticNotifierProvider(messageFactory);

            // Create a new instance of the DefaultShadowDiagnosticObserver
            var observer = new DefaultShadowDiagnosticObserver();

            // Register the observer with the notifier provider
            sdnp.SetObservers(new List<IShadowDiagnosticObserver>() { observer });

            // Create a list of diagnosable models
            var models = DiagnosableModelsFactory.Create(sdnp);

            // Start each model asynchronously
            var tasks = models.Select(model => model.StartAsync()).ToArray();
            // Wait for all tasks to complete
            Task.WaitAll(tasks);
        }
    }
}
