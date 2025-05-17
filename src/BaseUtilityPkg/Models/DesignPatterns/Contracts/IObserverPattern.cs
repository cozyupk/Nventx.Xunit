/*
using System;
using System.Collections.Generic;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts
{
    /// <summary>
    /// Defines contracts for the Observer design pattern, including factories, formatters, observers, and notifiers.
    /// </summary>
    public interface IObserverPattern
    {
        /// <summary>
        /// Observer interface that receives notifications.
        /// </summary>
        public interface IObserver<in TNotification>
        {
            /// <summary>
            /// Called when a notification is received.
            /// </summary>
            /// <param name="notification">The notification.</param>
            void OnNotified(TNotification notification);
        }

        /// <summary>
        /// Represents a notifier subject with its metadata.
        /// </summary>
        public interface INotifierSubject<out TSubjectMeta, out TSubject>
        {
            /// <summary>
            /// Gets the subject metadata.
            /// </summary>
            TSubjectMeta Meta { get; }

            /// <summary>
            /// Gets the subject.
            /// </summary>
            TSubject Subject { get; }
        }

        /// <summary>
        /// Notifier interface for sending notifications to observers.
        /// </summary>
        public interface INotifier<in TPayloadMeta, in TPayload>
        {
            /// <summary>
            /// Notifies observers with the specified payload and metadata.
            /// </summary>
            /// <param name="payloadMeta">The payload metadata.</param>
            /// <param name="payload">The payload.</param>
            void Notify(TPayloadMeta payloadMeta, TPayload payload);

            /// <summary>
            /// Notifies observers if any are present, using a factory for payloads.
            /// </summary>
            /// <param name="payloadMeta">The payload metadata.</param>
            /// <param name="payloadsFactory">A factory function to create payloads.</param>
            void NotifyIfObserved(TPayloadMeta payloadMeta, Func<IEnumerable<TPayload>> payloadsFactory);
        }

        /// <summary>
        /// Factory interface for creating notifiers and associating observers.
        /// </summary>
        public interface INotifierBuilder<out TSubjectMeta, out TSubject, out TNotification, TPayloadMeta, TPayload>
            : IFactoryObjectPattern.IFactoryWithCreationArgs<INotifierSubject<TSubject, TSubjectMeta>, INotifier<TPayload, TPayloadMeta>>
        {
            /// <summary>
            /// Adds an observer that will receive notifications.
            /// </summary>
            void AddObserver(IObserver<TNotification> observer);
            /// <summary>
            /// Builds and returns a notifier instance.
            /// </summary>
            INotifier<TPayload, TPayloadMeta> Build();
        }
    }
}
*/