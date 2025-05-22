using System;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow
{
    /// <summary>
    /// Represents a unicast notification handler that accepts a single delegate 
    /// to process incoming notifications of type <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TTarget">The type of the notification payload to be handled.</typeparam>

    public interface INotificationHandler<out TTarget>
    {
        /// <summary>
        /// Assigns the delegate that will handle incoming notifications.
        /// Only a single delegate may be assigned; assigning this property more than once is expected to throw an exception.
        /// </summary>
        /// <remarks>
        /// This interface enforces unicast semantics by exposing a setter-only delegate instead of a C# <c>event</c>.
        /// Unlike multicast events, only one delegate may be assigned at a time.
        /// Implementations should throw an exception if an attempt is made to assign the handler multiple times.
        /// </remarks>
        public void RegisterReceivingHandler(Action<TTarget> handle);
    }
}