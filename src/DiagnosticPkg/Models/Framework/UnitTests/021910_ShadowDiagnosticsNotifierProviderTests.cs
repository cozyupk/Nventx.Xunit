using System;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Impl;
using Xunit;
using Moq;
using System.Linq;

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
        /// <param name="SenderMeta">The sender meta information of the diagnostic message.</param>
        /// <param name="Category">The category of the diagnostic message.</param>
        /// <param name="Message">The content of the diagnostic message.</param>
        /// <param name="Level">The severity level of the diagnostic message.</param>
        private readonly record struct TestMessage(
            IShadowDiagnosableMeta SenderMeta,
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
        /// Provides a reusable sender meta instance for use in test cases.
        /// </summary>
        private IShadowDiagnosableMeta SenderMeta { get; } = new Mock<IShadowDiagnosableMeta>().Object;

        /// <summary>
        /// Creates a mock implementation of <see cref="IShadowDiagnosticMessageFactory"/> for testing.
        /// </summary>
        /// <returns>A mock factory that returns <see cref="TestMessage"/> instances.</returns>
        private static IShadowDiagnosticMessageFactory CreateMockFactory()
        {
            var mock = new Mock<IShadowDiagnosticMessageFactory>();
            mock.Setup(f => f.Create(
                It.IsAny<IShadowDiagnosableMeta>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ShadowDiagnosticLevel>()
            )).Returns((IShadowDiagnosableMeta senderMeta, string category, string message, ShadowDiagnosticLevel level) =>
                new TestMessage(senderMeta, category, message, level)
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

            // Act: Set observers
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

            // Assert: Second call should throw
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

            // Act: Create notifier
            var notifier = provider.CreateDiagnosticNotifier(SenderMeta, "Category");

            // Assert: Notifier is not null
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

            // Assert: Should throw if observers are not set
            Assert.Throws<InvalidOperationException>(() => provider.CreateDiagnosticNotifier(SenderMeta, "Category"));
        }

        /// <summary>
        /// Verifies that the notifier constructor throws when category is null or empty.
        /// </summary>
        /// <param name="category">The category to test.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ShouldThrow_WhenCategoryIsNullOrEmpty(string? category)
        {
            var factory = new Mock<IShadowDiagnosticMessageFactory>().Object;
            var observers = new List<IShadowDiagnosticObserver>();
            // Assert: Should throw ArgumentException for null or empty category
            Assert.Throws<ArgumentException>(() =>
                new ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(factory, observers, SenderMeta, category!));
        }

        /// <summary>
        /// Verifies that the notifier constructor throws when the message factory is null.
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrow_WhenMessageFactoryIsNull()
        {
            var observers = new List<IShadowDiagnosticObserver>();
            // Assert: Should throw ArgumentNullException for null factory
            Assert.Throws<ArgumentNullException>(() =>
                new ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(null!, observers, SenderMeta, "TestCategory"));
        }

        /// <summary>
        /// Verifies that the notifier constructor throws when observers is null.
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrow_WhenObserversIsNull()
        {
            var factory = new Mock<IShadowDiagnosticMessageFactory>().Object;
            // Assert: Should throw ArgumentNullException for null observers
            Assert.Throws<ArgumentNullException>(() =>
                new ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(factory, null!, SenderMeta, "TestCategory"));
        }

        /// <summary>
        /// Verifies that the provider constructor throws when the factory is null.
        /// </summary>
        [Fact]
        public void ProviderConstructor_ShouldThrow_WhenFactoryIsNull()
        {
            // Assert: Should throw ArgumentNullException for null factory
            Assert.Throws<ArgumentNullException>(() =>
                new ShadowDiagnosticNotifierProvider(null!));
        }

        /// <summary>
        /// Verifies that Notify calls observers with the correct message.
        /// </summary>
        [Fact]
        public void Notify_ShouldCallObserversWithCorrectMessage()
        {
            var factory = new Mock<IShadowDiagnosticMessageFactory>();
            var observer = new Mock<IShadowDiagnosticObserver>();
            var message = new Mock<IShadowDiagnosticMessage>();

            // Setup factory to return the mock message
            factory.Setup(f => f.Create(It.IsAny<IShadowDiagnosableMeta>(), "TestCategory", "Hello", ShadowDiagnosticLevel.Info))
                   .Returns(message.Object);

            var notifier = new ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(
                factory.Object,
                [observer.Object],
                SenderMeta,
                "TestCategory"
            );

            // Act: Notify
            notifier.Notify("Hello", ShadowDiagnosticLevel.Info);

            // Assert: Observer should be called with the correct message
            observer.Verify(o => o.OnDiagnostic(message.Object), Times.Once);
        }

        /// <summary>
        /// Verifies that Notify aggregates exceptions thrown by observers.
        /// </summary>
        [Fact]
        public void Notify_ShouldAggregateExceptions_WhenObserversFail()
        {
            var factory = new Mock<IShadowDiagnosticMessageFactory>();
            var observer = new Mock<IShadowDiagnosticObserver>();
            var message = new Mock<IShadowDiagnosticMessage>();

            // Setup factory to return the mock message
            factory.Setup(f => f.Create(It.IsAny<IShadowDiagnosableMeta>(), "TestCategory", "Hello", ShadowDiagnosticLevel.Info))
                   .Returns(message.Object);

            // Setup observer to throw
            observer.Setup(o => o.OnDiagnostic(It.IsAny<IShadowDiagnosticMessage>()))
                    .Throws(new InvalidOperationException("Oops"));

            var notifier = new ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(
                factory.Object,
                [observer.Object],
                SenderMeta,
                "TestCategory"
            );

            // Assert: Should throw AggregateException
            var ex = Assert.Throws<AggregateException>(() =>
                notifier.Notify("Hello", ShadowDiagnosticLevel.Info));

            Assert.Single(ex.InnerExceptions);
            Assert.IsType<InvalidOperationException>(ex.InnerExceptions.First());
        }

        /// <summary>
        /// Verifies that NotifyIfObserved does not invoke the factory when there are no observers.
        /// </summary>
        [Fact]
        public void NotifyIfObserved_ShouldNotInvokeFactory_WhenNoObservers()
        {
            var factory = new Mock<IShadowDiagnosticMessageFactory>();

            var notifier = new ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(
                factory.Object,
                [], // empty
                SenderMeta,
                "TestCategory"
            );

            bool factoryInvoked = false;

            // Act: NotifyIfObserved with no observers
            notifier.NotifyIfObserved(() =>
            {
                factoryInvoked = true;
                return ["Msg"];
            });

            // Assert: Factory should not be invoked
            Assert.False(factoryInvoked);
        }

        /// <summary>
        /// Verifies that NotifyIfObserved invokes the factory and notifies observers when observers exist.
        /// </summary>
        [Fact]
        public void NotifyIfObserved_ShouldInvokeFactoryAndNotify_WhenObserversExist()
        {
            var factory = new Mock<IShadowDiagnosticMessageFactory>();
            var observer = new Mock<IShadowDiagnosticObserver>();
            var message = new Mock<IShadowDiagnosticMessage>();

            // Setup factory to return the mock message
            factory.Setup(f => f.Create(It.IsAny<IShadowDiagnosableMeta>(), "TestCategory", "Msg", ShadowDiagnosticLevel.Info))
                   .Returns(message.Object);

            var notifier = new ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(
                factory.Object,
                [observer.Object],
                SenderMeta,
                "TestCategory"
            );

            // Act: NotifyIfObserved
            notifier.NotifyIfObserved(() => ["Msg"]);

            // Assert: Observer should be called
            observer.Verify(o => o.OnDiagnostic(message.Object), Times.Once);
        }

        /// <summary>
        /// Verifies that NotifyIfObserved handles a null factory result as empty messages.
        /// </summary>
        [Fact]
        public void NotifyIfObserved_ShouldHandleNullFactoryResult_AsEmptyMessages()
        {
            var factory = new Mock<IShadowDiagnosticMessageFactory>();
            var observer = new Mock<IShadowDiagnosticObserver>();

            var notifier = new ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(
                factory.Object,
                [observer.Object],
                SenderMeta,
                "TestCategory"
            );

            // Act: NotifyIfObserved with null result
            notifier.NotifyIfObserved(() => null!);

            // Assert: Observer should not be called
            observer.Verify(o => o.OnDiagnostic(It.IsAny<IShadowDiagnosticMessage>()), Times.Never);
        }

        /// <summary>
        /// Verifies that SetObservers throws an ArgumentNullException when the observers argument is null.
        /// </summary>
        [Fact]
        public void SetObservers_ShouldThrow_WhenObserversIsNull()
        {
            var factory = new Mock<IShadowDiagnosticMessageFactory>().Object;
            var provider = new ShadowDiagnosticNotifierProvider(factory);

            Assert.Throws<ArgumentNullException>(() => provider.SetObservers(null!));
        }
    }
}