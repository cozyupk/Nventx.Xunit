using System.Collections.Generic;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;
using Xunit;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.UnitTests.NotificationFlow.IAdaptToTests
{
    /// <summary>
    /// Dummy implementation of <see cref="IPayload{TPayloadMeta, TPayloadBody}"/> for testing purposes.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DummyPayload"/> class.
    /// </remarks>
    /// <param name="meta">The payload metadata.</param>
    /// <param name="bodies">The payload bodies.</param>
    public class DummyPayload(string meta, IEnumerable<string> bodies) : IPayload<string, string>
    {
        /// <summary>
        /// Gets the metadata associated with the payload.
        /// </summary>
        public string Meta { get; } = meta;

        /// <summary>
        /// Gets the payload bodies.
        /// </summary>
        public IEnumerable<string> Bodies { get; } = bodies;
    }

    /// <summary>
    /// Concrete implementation of <see cref="PayloadArgs{TPayloadMeta, TPayloadBody}"/> for testing.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DummyPayloadArgs"/> class.
    /// </remarks>
    /// <param name="meta">The payload metadata.</param>
    /// <param name="bodies">The payload bodies.</param>
    public class DummyPayloadArgs(string meta, params string[] bodies) : IAdaptTo<IPayload<string, string>>
    {
        /// <summary>
        /// The payload metadata.
        /// </summary>
        private readonly string _meta = meta;

        /// <summary>
        /// The payload bodies.
        /// </summary>
        private readonly List<string> _bodies = [.. bodies];

        /// <summary>
        /// Creates a new <see cref="DummyPayload"/> instance from the current arguments.
        /// </summary>
        /// <returns>A new <see cref="DummyPayload"/> instance.</returns>
        public IPayload<string, string> Adapt()
        {
            return new DummyPayload(_meta, _bodies);
        }
    }

    /// <summary>
    /// Unit tests for <see cref="IAdaptToTests{TPayloadMeta, TPayloadBody}"/> implementations.
    /// </summary>
    public class IAdaptToTests
    {
        /// <summary>
        /// Tests that <see cref="DummyPayloadArgs.Adapt"/> returns a payload with expected values.
        /// </summary>
        [Fact]
        public void NewFromSelf_ReturnsExpectedPayload()
        {
            // Arrange: Create a DummyPayloadArgs with test data
            var args = new DummyPayloadArgs("meta", "a", "b", "c");

            // Act: Generate a payload from the arguments
            var payload = args.Adapt();

            // Assert: Check that the payload has the expected metadata and bodies
            Assert.Equal("meta", payload.Meta);
            Assert.Collection(payload.Bodies,
                b => Assert.Equal("a", b),
                b => Assert.Equal("b", b),
                b => Assert.Equal("c", b)
            );
        }
    }
}
