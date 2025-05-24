using System;
using System.Threading;

namespace NventX.FoundationPkg.Impl.Traits
{
    /// <summary>
    /// Provides decorators for <see cref="Action{T}"/> to enable opt-in behavior composition
    /// such as synchronization, exception handling, and delay.
    /// </summary>
    /// <remarks>
    /// These extensions are designed to be explicitly applied by consumers who need them,
    /// in accordance with the Shadow DI principle of non-intrusive design.
    /// </remarks>
    public static class NotificationHandlerExtensions
    {
        /// <summary>
        /// Adds synchronization to an <see cref="Action{T}"/> using a provided lock object.
        /// </summary>
        /// <remarks>
        /// Use this decorator if the target handler modifies shared state and needs to be synchronized.
        /// In Shadow DI fashion, this extension does not impose locking—it merely enables it.
        /// </remarks>
        public static Action<T> WithLock<T>(this Action<T> action, object lockObj)
        {
            return arg =>
            {
                lock (lockObj)
                {
                    action(arg);
                }
            };
        }

        /// <summary>
        /// Wraps the handler in a try-catch block with a custom error handler.
        /// </summary>
        /// <remarks>
        /// Enables safe notification handling without enforcing global error handling policies.
        /// </remarks>
        public static Action<T> WithTryCatch<T>(this Action<T> action, Action<Exception> onError)
        {
            return arg =>
            {
                try { action(arg); }
                catch (Exception ex) { onError(ex); }
            };
        }

        /// <summary>
        /// Adds an artificial delay before invoking the action.
        /// </summary>
        /// <remarks>
        /// Useful in testing, throttling, or scenarios where slight pacing is desired.
        /// </remarks>
        public static Action<T> WithDelay<T>(this Action<T> action, int milliseconds)
        {
            return arg =>
            {
                Thread.Sleep(milliseconds);
                action(arg);
            };
        }

        /// <summary>
        /// Ensures that an <see cref="IDisposable"/> is disposed after the action completes.
        /// </summary>
        /// <remarks>
        /// Useful for scoped resources tied to notification lifetimes (e.g., telemetry spans).
        /// </remarks>
        public static Action<T> WithDisposeOnCompletion<T>(this Action<T> action, IDisposable disposable)
        {
            return arg =>
            {
                try { action(arg); }
                finally { disposable.Dispose(); }
            };
        }
    }
}
