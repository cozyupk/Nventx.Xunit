using System;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.PayloadFlow
{
    /// <summary>
    /// Wraps a payload consumer in a WeakReference and acts as a proxy that forwards notifications.
    /// Automatically becomes removable when the target is collected.
    /// </summary>
    public class PayloadConsumerUsingWeakReferenceBase<TSenderMeta, TPayloadMeta, TPayloadBody>
        : ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>, ISelfRemovable
        where TSenderMeta : class
        where TPayloadMeta : class
    {
        /// <summary>
        /// Holds a weak reference to a payload consumer and acts as a proxy to forward notifications.
        /// Automatically becomes eligible for removal when the target consumer is garbage collected.
        /// </summary>
        private WeakReference<ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>> WeakConsumer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayloadConsumerUsingWeakReferenceBase{T}"/> class.
        /// </summary>
        /// <param name="target">The actual payload consumer to wrap.</param>
        public PayloadConsumerUsingWeakReferenceBase(ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            WeakConsumer = new WeakReference<ISenderPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>>(target);
        }

        /// <summary>
        /// Forwards notifications to the underlying consumer if still alive.
        /// </summary>
        public IUnicastNotifier<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>> SenderPayloadArrivalNotifier =>
            new LambdaNotifier(payload =>
            {
                if (WeakConsumer.TryGetTarget(out var target))
                {
                    target.SenderPayloadArrivalNotifier.Notify(payload);
                }
                // If target is dead, do nothing
            });

        /// <summary>
        /// Determines whether the underlying consumer has been garbage collected.
        /// </summary>
        public bool CanRemove() => !WeakConsumer.TryGetTarget(out _);

        /// <summary>
        /// Simple lambda-based notifier.
        /// </summary>
        private class LambdaNotifier
            : IUnicastNotifier<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>>
        {
            /// <summary>
            /// The handler delegate that processes the payload notification.
            /// </summary>
            private Action<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>> Handler { get; }

            /// <summary>
            /// Notifies the handler with the provided payload argument.
            /// </summary>
            /// <param name="arg">The payload argument to notify.</param>
            public void Notify(ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody> arg) => Handler(arg);

            /// <summary>
            /// Initializes a new instance of the <see cref="LambdaNotifier"/> class with the specified handler.
            /// </summary>
            /// <param name="handler">The delegate to handle payload notifications.</param>
            public LambdaNotifier(Action<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>> handler)
            {
                Handler = handler;
            }
        }
    }
}