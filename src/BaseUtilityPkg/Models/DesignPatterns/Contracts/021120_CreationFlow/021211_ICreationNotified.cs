using System;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.CreationFlow
{
    /// <summary>
    /// Defines a contract for notifying when a single object of type <typeparamref name="TTarget"/> is created.
    /// This contract enforces unicast semantics.
    /// </summary>
    public interface ICreationNotified<out TTarget>
    {
        /// <summary>
        /// Assigns the handler to be invoked when an object is created.
        /// Only one handler must be set; repeated assignment is invalid.
        /// </summary>
        /// <remarks>
        /// This interface enforces unicast semantics by exposing a setter-only delegate property instead of a C# `event`.
        /// While `event` allows multiple subscribers (multicast), this contract only permits a single handler.
        /// Any attempt to set the handler more than once is expected to throw an exception in the implementation.
        /// This ensures strict ownership of object creation notification and avoids implicit fan-out behavior.
        /// </remarks>
        Action<TTarget> OnObjectCreated { set; }
    }
}