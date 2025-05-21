using System;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow
{
    /// <summary>
    /// Defines a contract for a multicast notifier that connects a single sending flow with multiple receiving flows.
    /// Only one sending flow may be registered per instance. Receivers can be attached and detached dynamically.
    /// </summary>
    /// <typeparam name="TFanOut">The type of the data sent to each receiver (fan-out).</typeparam>
    /// <typeparam name="TIn">The type of the data accepted by the sending flow.</typeparam>
    public interface IMulticastNotifier<out TFanOut, in TIn>
    {
        /// <summary>
        /// Registers the sending flow using the specified notification handler.
        /// This method can only be called once; subsequent calls will result in an exception.
        /// Optionally, a runner can be provided to control the execution context.
        /// </summary>
        /// <param name="flow">The notification handler that defines the sending flow.</param>
        /// <param name="runner">An optional action to control the execution of the handler.</param>
        void RegisterSendingFlow(INotificationHandler<TIn> flow, Action<Action>? runner = null);

        /// <summary>
        /// Attaches one or more receiving flows (i.e., unicast notifiers) to this multicast notifier.
        /// </summary>
        /// <param name="flow">One or more unicast notifiers representing receiving flows.</param>
        void AttachReceivingFlow(params IUnicastNotifier<TFanOut>[] flow);

        /// <summary>
        /// Detaches one or more receiving flows (i.e., unicast notifiers) from this multicast notifier.
        /// </summary>
        /// <param name="flow">One or more unicast notifiers to remove.</param>
        void DetachReceivingFlow(params IUnicastNotifier<TFanOut>[] flow);
    }
}