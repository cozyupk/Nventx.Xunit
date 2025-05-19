using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Impl.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.UnitTests.PayloadFlow.IAdaptToTests;
using Xunit;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.UnitTests.PayloadFlow.PayloadUnicastNotifierTests
{
    /// <summary>
    /// Contains unit tests for the <see cref="PayloadUnicastNotifier{TMeta, TBody}"/> class.
    /// </summary>
    public class PayloadUnicastNotifierTests
    {
        /// <summary>
        /// Verifies that the <see cref="PayloadUnicastNotifier{TMeta, TBody}.OnObjectCreated"/> event is triggered
        /// and receives the correct payload when <see cref="PayloadUnicastNotifier{TMeta, TBody}.Notify"/> is called.
        /// </summary>
        [Fact]
        public void Notify_TriggersOnObjectCreated()
        {
            // Arrange: Create a factory and dummy arguments for payload creation.
            var factory = new PayloadUnicastNotifier<string, string>();
            var args = new DummyPayloadArgs("meta", "body1");

            // Variable to capture the payload passed to the OnObjectCreated event.
            IPayload<string, string>? captured = null;

            // Assign a handler to capture the created payload.
            factory.Handle = payload => captured = payload;

            // Act: Create the payload and trigger the event.
            factory.Notify(args);

            // Assert: Ensure the event was triggered and the payload has expected values.
            Assert.NotNull(captured);
            Assert.Equal("meta", captured!.Meta);
            Assert.Contains("body1", captured.Bodies);
        }
    }
}
