using System;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Impl;
using Xunit;
using Moq;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ShadowDiagnosticNotifierProvider"/> class.
    /// Verifies correct observer management and notifier creation behavior.
    /// </summary>
    public class ShadowDiagnosticNotifierProviderTests
    {
        /// <summary>
        /// Test implementation of <see cref="IShadowDiagnosticMessage"/> for mocking purposes.
        /// </summary>
        /// <param name="Sender">The sender of the diagnostic message.</param>
        /// <param name="Category">The category of the diagnostic message.</param>
        /// <param name="Message">The content of the diagnostic message.</param>
        /// <param name="Level">The severity level of the diagnostic message.</param>
        private readonly record struct TestMessage(
            object? Sender,
            string Category,
            string Message,
            ShadowDiagnosticLevel Level
        ) : IShadowDiagnosticMessage
        {
            /// <summary>
            /// Gets the timestamp when the diagnostic message was created.
            /// </summary>
            public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;
        }

        /// <summary>
        /// Creates a mock implementation of <see cref="IShadowDiagnosticMessageFactory"/> for testing.
        /// </summary>
        /// <returns>A mock factory that returns <see cref="TestMessage"/> instances.</returns>
        private static IShadowDiagnosticMessageFactory CreateMockFactory()
        {
            var mock = new Mock<IShadowDiagnosticMessageFactory>();
            mock.Setup(f => f.Create(
                It.IsAny<object?>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ShadowDiagnosticLevel>()
            )).Returns((object? sender, string category, string message, ShadowDiagnosticLevel level) =>
                new TestMessage(sender, category, message, level)
            );
            return mock.Object;
        }

        /// <summary>
        /// Verifies that observers can be set successfully on the provider.
        /// </summary>
        [Fact]
        public void SetObservers_SetsSuccessfully()
        {
            var factory = CreateMockFactory();
            var provider = new ShadowDiagnosticNotifierProvider(factory);
            var observers = new List<IShadowDiagnosticObserver> { new Mock<IShadowDiagnosticObserver>().Object };

            provider.SetObservers(observers);
        }

        /// <summary>
        /// Verifies that setting observers twice throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        [Fact]
        public void SetObservers_Throws_WhenCalledTwice()
        {
            var factory = CreateMockFactory();
            var provider = new ShadowDiagnosticNotifierProvider(factory);
            var observers = new List<IShadowDiagnosticObserver> { new Mock<IShadowDiagnosticObserver>().Object };

            provider.SetObservers(observers);

            Assert.Throws<InvalidOperationException>(() => provider.SetObservers(observers));
        }

        /// <summary>
        /// Verifies that a diagnostic notifier can be created after observers are set.
        /// </summary>
        [Fact]
        public void CreateDiagnosticNotifier_ReturnsNotifier()
        {
            var factory = CreateMockFactory();
            var provider = new ShadowDiagnosticNotifierProvider(factory);
            var observers = new List<IShadowDiagnosticObserver> { new Mock<IShadowDiagnosticObserver>().Object };
            provider.SetObservers(observers);

            var notifier = provider.CreateDiagnosticNotifier("Category");

            Assert.NotNull(notifier);
        }

        /// <summary>
        /// Verifies that creating a diagnostic notifier before setting observers throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        [Fact]
        public void CreateDiagnosticNotifier_Throws_WhenObserversNotSet()
        {
            var factory = CreateMockFactory();
            var provider = new ShadowDiagnosticNotifierProvider(factory);

            Assert.Throws<InvalidOperationException>(() => provider.CreateDiagnosticNotifier("Category"));
        }
    }
}