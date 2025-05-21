using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.NotificationFlow
{
    /// <summary>
    /// Provides a multicast notifier that connects a single sending flow with multiple receiving flows.
    /// Allows dynamic attachment and detachment of receivers, and adapts source to target types.
    /// </summary>
    public class MulticastNotifier<TMulticastTarget, TMulticastSource> : IMulticastNotifier<TMulticastTarget, TMulticastSource>
        where TMulticastTarget : class
        where TMulticastSource : IAdaptTo<TMulticastTarget>
    {
        /// <summary>
        /// Lock object for synchronizing access to the receivers collection.
        /// </summary>
        private object ReceiversLock { get; } = new object();

        /// <summary>
        /// Thread-safe dictionary holding the current set of receivers.
        /// </summary>
        private ConcurrentDictionary<IUnicastNotifier<TMulticastTarget>, byte> Receivers { get; } = new();

        /// <summary>
        /// Adapts the source to the multicast target type.
        /// Override this method to provide custom adaptation logic.
        /// </summary>
        /// <param name="source">The source to adapt.</param>
        /// <returns>The adapted multicast target.</returns>
        protected virtual TMulticastTarget AdaptTo(TMulticastSource source)
        {
            // Default adaptation logic; override as needed.
            return source.Adapt();
        }

        /// <summary>
        /// Determines whether a receiver should be notified with the given output.
        /// Override to implement custom filtering logic.
        /// </summary>
        /// <param name="receiver">The receiver to check.</param>
        /// <param name="output">The output to be notified.</param>
        /// <returns>True if the receiver should be notified; otherwise, false.</returns>
        protected virtual bool IsNotifiable(IUnicastNotifier<TMulticastTarget> receiver, TMulticastTarget output)
        {
            return true;
        }

        /// <summary>
        /// Handles exceptions that occur during notification.
        /// Override this method to provide custom exception handling logic.
        /// </summary>
        /// <param name="receiver">The receiver that caused the exception during notification.</param>
        /// <param name="output">The output that was being notified when the exception occurred.</param>
        /// <param name="ex">The exception that was thrown during notification.</param>
        protected virtual void OnExceptionInNotification(IUnicastNotifier<TMulticastTarget> receiver, TMulticastTarget output, Exception ex)
        {
            // Handle exceptions that occur during notification
            // This can be overridden to provide custom exception handling logic
            System.Diagnostics.Debug.WriteLine($"[Warning] Exception during notification: {ex.Message}");
        }

        /// <summary>
        /// Returns a thread-safe copy of the current receivers, removing any that indicate they should be removed.
        /// </summary>
        /// <returns>A copy of the current receivers.</returns>
        protected internal IEnumerable<IUnicastNotifier<TMulticastTarget>> GetLatestReceiversSnapshot()
        {
            List<IUnicastNotifier<TMulticastTarget>> snapshot = new();

            // Lock to ensure thread-safe access to the receiver list
            lock (ReceiversLock)
            {
                foreach (var receiver in Receivers.Keys)
                {
                    // Remove receivers that indicate they should be removed
                    if (receiver is ISelfRemovable selfRemovable && selfRemovable.CanRemove())
                    {
                        Receivers.TryRemove(receiver, out _);
                        continue;
                    }
                    snapshot.Add(receiver);
                }
            }

            return snapshot;
        }

        /// <summary>
        /// Registers the sending flow using the specified notification handler.
        /// Only one sending flow may be registered per instance.
        /// Optionally, a runner can be provided to control the execution context.
        /// </summary>
        /// <param name="flow">The notification handler that defines the sending flow.</param>
        /// <param name="runner">An optional action to control the execution of the handler.</param>
        public void RegisterSendingFlow(INotificationHandler<TMulticastSource> flow, Action<Action>? runner = null)
        {
            if (flow == null)
                throw new ArgumentNullException(nameof(flow));

            // Use synchronous invocation if no runner is provided
            runner ??= a => a();

            // Assign the handler to be called when a source is received
            flow.Handle = source =>
            {
                // Adapt the source to the target type
                var output = AdaptTo(source);

                // Get a thread-safe copy of the current receivers
                var receiversCppy = GetLatestReceiversSnapshot();

                // Notify all eligible receivers via the injected execution context.
                // This allows receivers to be notified synchronously, asynchronously, or with custom logic.
                runner(() =>
                {
                    // Notify all receivers of the adapted output if they accept it
                    foreach (var receiver in receiversCppy)
                    {
                        // Check whether the receiver accepts the output
                        if (!IsNotifiable(receiver, output))
                        {
                            continue;
                        }

                        // Notify the receiver with the adapted output
                        try { receiver.Notify(output); }
                        catch (Exception ex)
                        {
                            // handle exceptions that occur during notification
                            OnExceptionInNotification(receiver, output, ex);
                        }
                    }
                });
            };
        }

        /// <summary>
        /// Attaches one or more receiving flows (i.e., unicast notifiers) to this multicast notifier.
        /// </summary>
        /// <param name="flow">One or more unicast notifiers representing receiving flows.</param>
        public void AttachReceivingFlow(params IUnicastNotifier<TMulticastTarget>[] flow)
        {
            lock (ReceiversLock)
            {
                foreach (var receiver in flow)
                {
                    if (receiver == null)
                        throw new ArgumentNullException(nameof(flow), "Flow contains a null receiver.");
                    // Add the receiver to the dictionary
                    Receivers.TryAdd(receiver, byte.MinValue);
                }
            }
        }

        /// <summary>
        /// Detaches one or more receiving flows (i.e., unicast notifiers) from this multicast notifier.
        /// </summary>
        /// <param name="flow">One or more unicast notifiers to remove.</param>
        public void DetachReceivingFlow(params IUnicastNotifier<TMulticastTarget>[] flow)
        {
            lock (ReceiversLock)
            {
                foreach (var receiver in flow)
                {
                    if (receiver == null)
                        continue;
                    // Remove the receiver from the dictionary
                    Receivers.TryRemove(receiver, out _);
                }
            }
        }
    }
}
