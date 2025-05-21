using System;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.PayloadFlow;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.UnitTests.NotificationFlow.IAdaptToTests;
using Xunit;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.UnitTests.PayloadFlow.PayloadUnicastNotifierTests
{
    /// <summary>
    /// Unit tests for the PayloadUnicastNotifier class.
    /// Verifies correct notification behavior and exception handling.
    /// </summary>
    public class PayloadUnicastNotifierTests
    {
        /// <summary>
        /// Verifies that the assigned handler is called with the correct payload when Notify is invoked.
        /// </summary>
        [Fact]
        public void Notify_CallsAssignedHandle_WithCorrectPayload()
        {
            // Arrange
            IPayloadUnicastNotificationFlow<string, string> notifier = new PayloadUnicastNotifier<string, string>();
            var args = new DummyPayloadArgs("meta", "body1");

            // Variable to capture the payload passed to the handle.
            IPayload<string, string>? captured = null;

            // Assign a handler to capture the notified payload.
            notifier.Handle = payload => captured = payload;

            // Act
            notifier.Notify(args);

            // Assert
            Assert.NotNull(captured);
            Assert.Equal("meta", captured!.Meta);
            Assert.Contains("body1", captured.Bodies);
        }

        /// <summary>
        /// Verifies that an InvalidOperationException is thrown if Notify is called when Handle is null.
        /// </summary>
        [Fact]
        public void Notify_WithNullHandle_ThrowsInvalidOperationException()
        {
            // Arrange
            IPayloadUnicastNotificationFlow<string, string> notifier = new PayloadUnicastNotifier<string, string>();
            var args = new DummyPayloadArgs("meta", "body1");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => notifier.Notify(args));
        }
    }
}
