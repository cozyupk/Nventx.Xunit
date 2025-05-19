using System;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.NotificationFlow
{
    /// <summary>
    /// Defines a contract for a notification handler that accepts a single delegate for handling notifications.
    /// </summary>
    /// <typeparam name="TTarget">The type of the notification target.</typeparam>
    public interface INotificationHandler<out TTarget>
    {
        /// <summary>
        /// Sets the delegate to handle notifications for the specified target.
        /// Only a single handler can be assigned; setting this property more than once should throw an exception.
        /// </summary>
        /// <remarks>
        /// This interface enforces unicast semantics by exposing a setter-only delegate property instead of a C# `event`.
        /// While `event` allows multiple subscribers (multicast), this contract only permits a single handler.
        /// Any attempt to set the handler more than once is expected to throw an exception in the implementation.
        /// </remarks>
        Action<TTarget> Handle { set; }
    }
}