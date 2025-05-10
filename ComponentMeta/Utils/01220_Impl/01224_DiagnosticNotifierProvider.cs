using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cozyupk.HelloShadowDI.ComponentMeta.Utils.Contracts;

namespace Cozyupk.HelloShadowDI.ComponentMeta.Utils.Impl
{
    public class DiagnosticNotifierProvider : IShadowDiagnosticNotifierProvider
    {
        protected class DiagnosticNotifier : IShadowDiagnosticNotifier
        {
            private IEnumerable<IShadowDiagnosticObserver> Observers { get; }

            private string? MessagePrefix { get; }

            public DiagnosticNotifier(IEnumerable<IShadowDiagnosticObserver> observers, string? messagePrefix = null)
            {
                Observers = observers;
                MessagePrefix = messagePrefix;
            }

            private static void Notify(IEnumerable<IShadowDiagnosticObserver> observers, IEnumerable<string> messages, string? prefix, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info)
            {
                var prefixFormatted = $"{(prefix == null ? "" : $"{prefix}: ")}";
                var diagnosticMessages = new List<IShadowDiagnosticMessage>();

                foreach (var msg in messages)
                {
                    // Create a new diagnostic message with the provided content and severity level.
                    diagnosticMessages.Add(
                        new DiagnosticMessage($"{prefixFormatted}{msg}", level)
                    );
                }

                // Initialize a list to collect exceptions that occur during observer notification.
                List<Exception> failures = new List<Exception>();

                // Iterate through all registered diagnostic observers.
                foreach (var observer in observers)
                {
                    foreach (var diagnosticMessage in diagnosticMessages)
                    {
                        try
                        {
                            // Notify the observer with the diagnostic message.
                            observer.OnDiagnostic(diagnosticMessage);
                        }
                        catch (Exception ex)
                        {
                            // If an exception occurs, add it to the failures list and log the error.
                            failures.Add(ex);
                            Debug.WriteLine($"{prefixFormatted}Observer failed: {ex.GetType().Name} - {ex.Message}");
                        }
                    }
                }

                // If any observers failed, throw an aggregate exception containing all failures.
                if (failures.Count > 0)
                {
                    throw new AggregateException($"{prefixFormatted}One or more diagnostic observers failed.", failures);
                }
            }


            public void Notify(string message, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info)
            {
                Notify(Observers, new List<string>() { message }, MessagePrefix, level);
            }

            public void NotifyIfObserved(Func<List<string>> messageFactory, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info)
            {
                if (Observers.Any())
                {
                    var messages = messageFactory();
                    Notify(Observers, messages, MessagePrefix, level);
                }
            }
        }

        private IEnumerable<IShadowDiagnosticObserver> Observers { get; }

        protected internal DiagnosticNotifierProvider(IEnumerable<IShadowDiagnosticObserver> observers)
        {
            Observers = observers;
        }

        public IShadowDiagnosticNotifier CreateDiagnosticNotifier(string messagePrefix)
        {
            return new DiagnosticNotifier(Observers, messagePrefix);
        }
    }
}
