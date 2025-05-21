namespace Cozyupk.Shadow.Flow.DiagnosticPkg.ComponentRoots.Playgrounds.DiagnosingBob
{
    internal class Program
    {
        public static void Main(string[] _)
        {
        }
    }
}

/*
using System.Linq;
using System.Threading.Tasks;
using Cozyupk.Shadow.Flow.DiagnosticPkg.Adapters.Framework.Impl;
using Cozyupk.Shadow.Flow.DiagnosticPkg.ComponentRoots.Common.Impl.Models;
using Cozyupk.Shadow.Flow.DiagnosticPkg.Details.Framework.Impl;
using Cozyupk.Shadow.Flow.DiagnosticPkg.Models.Framework.Impl;

namespace Cozyupk.Shadow.Flow.DiagnosticPkg.ComponentRoots.Playgrounds.DiagnosingBob
{
    internal class Program
    {
        public static void Main(string[] _)
        {
            // Create a new instance of DiagnosticMessageFactory
            var messageFactory = new ShadowDiagnosticMessageFactory();

            // Create new instance of ShadowDiagnosticNotifierProvider
            var sdnp = new ShadowDiagnosticNotifierProvider(messageFactory);

            // Create new instance of ShadowDiagnosticFormatter
            var formatter = new DefaultShadowDiagnosticFormatter();

            // Create a new instance of the DefaultShadowDiagnosticObserver
            var observer = new DefaultShadowDiagnosticObserver(formatter);

            // Register the observer with the notifier provider
            sdnp.SetObservers([observer]);

            // Create a list of diagnosable models
            var models = DiagnosableModelsFactory.Create(sdnp);

            // Start each model asynchronously
            var tasks = models.Select(model => model.StartAsync()).ToArray();
            // Wait for all tasks to complete
            Task.WaitAll(tasks);
        }
    }
}
*/