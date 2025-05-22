using System.Threading.Tasks;
using System;
using Xunit;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.Traits;
using System.Collections.Generic;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.UnitTests.Traits.NotificationHandlerExtensionsTests
{
    /// <summary>
    /// Unit tests for NotificationHandler extension methods such as WithLock, WithTryCatch, WithDelay, and WithDisposeOnCompletion.
    /// </summary>
    public class NotificationHandlerExtensionsTests
    {
        /// <summary>
        /// Verifies that WithLock ensures mutual exclusion when executing the action in parallel.
        /// </summary>
        [Fact]
        public void WithLock_EnsuresMutualExclusion()
        {
            int counter = 0;
            object lockObj = new();
            Action<int> action = i => { counter += i; };
            var lockedAction = action.WithLock(lockObj);

            // Ensure mutual exclusion by running the action in parallel
            Parallel.For(0, 1000, i => lockedAction(1));
            Assert.Equal(1000, counter);
        }

        /// <summary>
        /// Verifies that WithTryCatch catches exceptions and calls the provided error handler.
        /// </summary>
        [Fact]
        public void WithTryCatch_CatchesExceptionAndCallsOnError()
        {
            bool errorHandled = false;
            Action<int> action = i => throw new InvalidOperationException();
            void onError(Exception ex) => errorHandled = ex is InvalidOperationException;

            var safeAction = action.WithTryCatch(onError);
            safeAction(0);

            Assert.True(errorHandled);
        }

        /// <summary>
        /// Verifies that WithDelay delays the execution of the action by the specified milliseconds.
        /// </summary>
        [Fact]
        public void WithDelay_DelaysExecution()
        {
            Action<int> action = i => { };
            var delayedAction = action.WithDelay(100);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            delayedAction(0);
            sw.Stop();

            // Ensure the delay is at least 100 milliseconds
            Assert.True(sw.ElapsedMilliseconds >= 100);
        }

        /// <summary>
        /// Test implementation of IDisposable for verifying disposal behavior.
        /// </summary>
        private class TestDisposable : IDisposable
        {
            public bool Disposed { get; private set; }
            public void Dispose() => Disposed = true;
        }

        /// <summary>
        /// Verifies that WithDisposeOnCompletion disposes the provided IDisposable after the action is executed.
        /// </summary>
        [Fact]
        public void WithDisposeOnCompletion_DisposesAfterAction()
        {
            var disposable = new TestDisposable();
            bool actionCalled = false;
            Action<int> action = i => actionCalled = true;

            var wrapped = action.WithDisposeOnCompletion(disposable);
            wrapped(0);

            Assert.True(actionCalled);
            Assert.True(disposable.Disposed);
        }
    }
}
