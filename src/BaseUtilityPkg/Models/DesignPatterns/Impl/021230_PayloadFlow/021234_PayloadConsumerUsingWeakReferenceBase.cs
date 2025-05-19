using System;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow
{
    /// <summary>
    /// Wraps a payload consumer in a WeakReference and acts as a proxy that forwards notifications.
    /// Automatically becomes removable when the target is collected.
    /// </summary>
    public class PayloadConsumerUsingWeakReferenceBase<TSenderMeta, TPayloadMeta, TPayloadBody>
        : IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>, ISelfRemovable
        where TSenderMeta : class
        where TPayloadMeta : class
    {
        /// <summary>
        /// Holds a weak reference to a payload consumer and acts as a proxy to forward notifications.
        /// Automatically becomes eligible for removal when the target consumer is garbage collected.
        /// </summary>
        private WeakReference<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>> WeakConsumer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayloadConsumerUsingWeakReferenceBase{T}"/> class.
        /// </summary>
        /// <param name="target">The actual payload consumer to wrap.</param>
        public PayloadConsumerUsingWeakReferenceBase(IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody> target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            WeakConsumer = new WeakReference<IPayloadConsumer<TSenderMeta, TPayloadMeta, TPayloadBody>>(target);
        }

        /// <summary>
        /// Forwards notifications to the underlying consumer if still alive.
        /// </summary>
        public INotifyAdapted<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>> PayloadArrivalNotifier =>
            new LambdaNotifier(payload =>
            {
                if (WeakConsumer.TryGetTarget(out var target))
                {
                    target.PayloadArrivalNotifier.Notify(payload);
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
            : INotifyAdapted<ISenderPayload<TSenderMeta, TPayloadMeta, TPayloadBody>>
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