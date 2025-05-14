using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Impl
{
    /// <summary>
    /// Provides functionality to create diagnostic notifiers that can notify observers
    /// about diagnostic messages with a specific prefix.
    /// </summary>
    /// <remarks>
    /// Items for future consideration:
    ///  * Logging level filter.
    ///  * Make Categories composite(hierarchical and adding ContextIds for tracing).
    ///  * Should the loop in Observer.OnDiagnostic(...) be made asynchronous? (Use Task.WhenAll(...)).
    /// </remarks>
    public class ShadowDiagnosticNotifierProvider : IShadowDiagnosticNotifierProvider
    {
        /// <summary>
        /// Represents a diagnostic notifier that sends messages to registered observers.
        /// </summary>
        /// <remarks>
        /// The internal modifier is used to allow access for unit testing purposes.
        /// </remarks>
        protected internal class ShadowDiagnosticNotifier : IShadowDiagnosticNotifier
        {
            /// <summary>
            /// Factory for creating diagnostic messages.
            /// </summary>
            private IShadowDiagnosticMessageFactory MessageFactory { get; }

            /// <summary>
            /// Gets the collection of diagnostic observers.
            /// </summary>
            private IEnumerable<IShadowDiagnosticObserver> Observers { get; }

            /// <summary>
            /// Holds the sender object associated with the diagnostic notifier.
            /// This object represents the source of diagnostic messages and can be null.
            /// </summary>
            private object? Sender { get; }

            /// <summary>
            /// Represents the category of the diagnostic messages.
            /// This property is used to group or classify diagnostic messages
            /// for easier filtering or analysis by observers.
            /// </summary>
            private string Category { get; }

            /// <param name="messageFactory">The factory used to create diagnostic message instances.</param>
            /// <param name="observers">The collection of observers to be notified of diagnostic messages.</param>
            /// <param name="sender">The source object that is sending the diagnostic messages. Can be null.</param>
            /// <param name="category">The category of the diagnostic messages. Cannot be null or empty.</param>
            public ShadowDiagnosticNotifier(
                IShadowDiagnosticMessageFactory messageFactory,
                IEnumerable<IShadowDiagnosticObserver> observers,
                object? sender,
                string category
            )
            {
                // Validate the category to ensure it is not null or empty.
                if (string.IsNullOrEmpty(category))
                {
                    throw new ArgumentException("Category cannot be null or empty.", nameof(category));
                }

                // Set properties with provided values or defaults.
                MessageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory), "Message factory cannot be null.");
                Observers = observers ?? throw new ArgumentNullException(nameof(observers), "Observers cannot be null.");
                Sender = sender;
                Category = category;
            }

            /// <summary>
            /// Notifies all observers with the provided messages and severity level.
            /// </summary>
            /// <param name="sender">The source object that is sending the diagnostic messages.</param>
            /// <param name="level">The severity level of the diagnostic messages.</param>
            /// <param name="messages">The collection of diagnostic messages to send to the observers.</param>
            protected internal void Notify(
                object? sender,
                ShadowDiagnosticLevel level,
                IEnumerable<string> messages
            )
            {
                var diagnosticMessages = new List<IShadowDiagnosticMessage>();


                // Create diagnostic messages with the provided content and severity level.
                // In the future, we may introduce the Factory pattern for ShadowDiagnosticMessage.
                foreach (var msg in messages)
                {
                    diagnosticMessages.Add(MessageFactory.Create(sender, Category, msg, level));
                }

                // Collect exceptions that occur during observer notification.
                List<Exception> failures = new();

                foreach (var observer in Observers)
                {
                    foreach (var diagnosticMessage in diagnosticMessages)
                    {
                        try
                        {
                            observer.OnDiagnostic(diagnosticMessage);
                        }
                        catch (Exception ex)
                        {
                            failures.Add(ex);
                            Debug.WriteLine($"Observer failed: {ex.GetType().Name} - {ex.Message}");
                        }
                    }
                }

                // Throw an aggregate exception if any observers failed.
                if (failures.Count > 0)
                {
                    throw new AggregateException($"One or more diagnostic observers failed.", failures);
                }
            }

            /// <summary>
            /// Notifies observers with a single diagnostic message.
            /// </summary>
            /// <param name="message">The diagnostic message to send.</param>
            /// <param name="level">The severity level of the message.</param>
            public void Notify(string message, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info)
            {
                Notify(Sender, level, new List<string>() { message });
            }

            /// <summary>
            /// Notifies observers with messages generated by a factory, if any observers are registered.
            /// </summary>
            /// <param name="messageFactory">
            /// The factory function to generate diagnostic messages.
            /// If <c>null</c> is returned from the factory, it is treated as an empty array.
            /// </param>
            /// <param name="level">The severity level of the messages.</param>
            public void NotifyIfObserved(Func<List<string>> messageFactory, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info)
            {
                if (Observers.Any())
                {
                    var messages = messageFactory() ?? Enumerable.Empty<string>();
                    Notify(Sender, level, messages);
                }
            }
        }

        /// <summary>
        /// Gets the factory that has been injected by the constructor, used to create diagnostic message instances.
        /// </summary>
        private IShadowDiagnosticMessageFactory MessageFactory { get; }

        /// <summary>
        /// Gets and sets the collection of diagnostic observers.
        /// </summary>
        private IEnumerable<IShadowDiagnosticObserver>? Observers { get;  set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShadowDiagnosticNotifierProvider"/> class.
        /// </summary>
        /// <param name="factory">The factory used to create diagnostic message instances.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is null.</exception>
        public ShadowDiagnosticNotifierProvider(IShadowDiagnosticMessageFactory factory)
        {
            MessageFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Sets the collection of diagnostic observers to be notified.
        /// This observers is intended to be set only once (set-once semantics).
        /// Attempting to set it more than once will result in an exception.
        /// </summary>
        /// <param name="observers">The observers to register for diagnostic notifications.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="observers"/> is null.</exception>
        public void SetObservers(IEnumerable<IShadowDiagnosticObserver> observers)
        {
            if (Observers != null)
                throw new InvalidOperationException("Observers already set.");
            Observers = observers ?? throw new ArgumentNullException(nameof(observers));
        }

        /// <summary>
        /// Creates a diagnostic notifier with the specified category.
        /// </summary>
        /// <param name="category">The category of diagnostic messages to be used by the notifier.</param>
        /// <returns>An instance of <see cref="IShadowDiagnosticNotifier"/> configured with the specified category.</returns>
        public IShadowDiagnosticNotifier CreateDiagnosticNotifier(object? sender, string category)
        {
            if (Observers == null)
                throw new InvalidOperationException("Observers must be set before creating a notifier.");

            return new ShadowDiagnosticNotifier(MessageFactory, Observers, sender, category);
        }
    }
}
