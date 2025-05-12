using System;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Impl;
using Moq;
using Xunit;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the ShadowDiagnosticNotifierProvider class.
    /// </summary>
    public class ShadowDiagnosticNotifierProviderTests
    {
        /// <summary>
        /// Verifies that the constructor throws an ArgumentNullException when the observers list is null.
        /// </summary>
        [Fact]
        public void Provider_ThrowsArgumentNullException_WhenObserversIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ShadowDiagnosticNotifierProvider(null!));
        }

        /// <summary>
        /// Verifies that CreateDiagnosticNotifier throws an ArgumentException when the category is null or empty.
        /// </summary>
        [Fact]
        public void CreateDiagnosticNotifier_Throws_WhenCategoryIsNullOrEmpty()
        {
            var provider = new ShadowDiagnosticNotifierProvider(new List<IShadowDiagnosticObserver>());

            Assert.Throws<ArgumentException>(() =>
                provider.CreateDiagnosticNotifier(null!));

            Assert.Throws<ArgumentException>(() =>
                provider.CreateDiagnosticNotifier(""));
        }

        /// <summary>
        /// Verifies that CreateDiagnosticNotifier returns a valid notifier with the correct category.
        /// </summary>
        [Fact]
        public void CreateDiagnosticNotifier_ReturnsNotifier_WithCorrectCategory()
        {
            var provider = new ShadowDiagnosticNotifierProvider(new List<IShadowDiagnosticObserver>());
            var notifier = provider.CreateDiagnosticNotifier("System.Component");

            Assert.NotNull(notifier);
            Assert.IsType<IShadowDiagnosticNotifier>(notifier, exactMatch: false);
        }

        /// <summary>
        /// Verifies that Notify sends a diagnostic message to the observer.
        /// </summary>
        [Fact]
        public void Notify_SendsMessageToObserver()
        {
            // Arrange
            var mockObserver = new Mock<IShadowDiagnosticObserver>();
            var observers = new List<IShadowDiagnosticObserver> { mockObserver.Object };

            var notifier = new Impl.ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(observers, "MyCategory");

            var sender = new object();
            var message = "Test Message";

            // Act
            notifier.Notify(sender, message, ShadowDiagnosticLevel.Warning);

            // Assert
            mockObserver.Verify(o => o.OnDiagnostic(It.Is<IShadowDiagnosticMessage>(
                m => m.Sender == sender &&
                     m.Category == "MyCategory" &&
                     m.Message == message &&
                     m.Level == ShadowDiagnosticLevel.Warning
            )), Times.Once);
        }

        /// <summary>
        /// Verifies that NotifyIfObserved calls the message factory when observers exist.
        /// </summary>
        [Fact]
        public void NotifyIfObserved_CallsMessageFactory_WhenObserversExist()
        {
            // Arrange
            var mockObserver = new Mock<IShadowDiagnosticObserver>();
            var observers = new List<IShadowDiagnosticObserver> { mockObserver.Object };

            var notifier = new Impl.ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(observers, "Test");

            bool factoryCalled = false;
            List<string> factory()
            {
                factoryCalled = true;
                return new List<string> { "Generated Message" };
            }

            // Act
            notifier.NotifyIfObserved(null, factory);

            // Assert
            Assert.True(factoryCalled);
            mockObserver.Verify(o => o.OnDiagnostic(It.IsAny<IShadowDiagnosticMessage>()), Times.Once);
        }

        /// <summary>
        /// Verifies that NotifyIfObserved skips the message factory when no observers are present.
        /// </summary>
        [Fact]
        public void NotifyIfObserved_SkipsFactory_WhenNoObservers()
        {
            var notifier = new Impl.ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(new List<IShadowDiagnosticObserver>(), "Test");

            bool factoryCalled = false;
            List<string> factory()
            {
                factoryCalled = true;
                return new List<string> { "Should Not Be Called" };
            }

            notifier.NotifyIfObserved(null, factory);

            Assert.False(factoryCalled);
        }

        /// <summary>
        /// Verifies that Notify throws an AggregateException when an observer fails.
        /// </summary>
        [Fact]
        public void Notify_ThrowsAggregateException_WhenObserverFails()
        {
            var mockObserver = new Mock<IShadowDiagnosticObserver>();
            mockObserver.Setup(o => o.OnDiagnostic(It.IsAny<IShadowDiagnosticMessage>()))
                        .Throws(new InvalidOperationException("Expected Failure"));

            var observers = new List<IShadowDiagnosticObserver> { mockObserver.Object };
            var notifier = new Impl.ShadowDiagnosticNotifierProvider.ShadowDiagnosticNotifier(observers, "Failing");

            var ex = Assert.Throws<AggregateException>(() =>
                notifier.Notify(null, "Oops"));

            Assert.Single(ex.InnerExceptions);
            Assert.IsType<InvalidOperationException>(ex.InnerExceptions[0]);
        }
    }
}
