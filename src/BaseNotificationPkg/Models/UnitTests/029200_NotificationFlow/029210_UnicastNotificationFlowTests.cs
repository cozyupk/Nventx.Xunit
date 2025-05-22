using System;
using Xunit;
using FluentAssertions;
using Moq;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;
using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Impl.NotificationFlow;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.UnitTests.NotificationFlow
{
    public class UnicastNotificationFlowTests
    {
        [Fact]
        public void UnicastFlow_Should_Invoke_Handler_With_Exact_Instance()
        {
            // Arrange
            var flow = new UnicastFlow<string>();
            string? received = null;
            flow.RegisterReceivingHandler(s => received = s);

            // Act
            flow.Notify("hello");

            // Assert
            received.Should().Be("hello");
        }

        [Fact]
        public void UnicastAdaptationFlow_Should_Invoke_Handler_With_Adapted_Result()
        {
            // Arrange
            var mockAdaptable = new Mock<IAdaptTo<string>>();
            mockAdaptable.Setup(m => m.Adapt()).Returns("adapted!");

            var flow = new UnicastAdaptationFlow<string, IAdaptTo<string>>();
            string? received = null;
            flow.RegisterReceivingHandler(s => received = s);

            // Act
            flow.Notify(mockAdaptable.Object);

            // Assert
            received.Should().Be("adapted!");
        }

        [Fact]
        public void Constructor_Should_Throw_If_Lambda_Is_Null()
        {
            // Act
            Action act = () => new UnicastProjectionFlow<string, string>(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("*Adaptation function cannot be null.*");
        }

        [Fact]
        public void RegisterHandler_Should_Throw_If_Null()
        {
            // Arrange
            var flow = new UnicastFlow<string>();

            // Act
            Action act = () => flow.RegisterReceivingHandler(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("*Handler cannot be null*");
        }

        [Fact]
        public void RegisterHandler_Should_Throw_If_Registered_Twice()
        {
            // Arrange
            var flow = new UnicastFlow<string>();
            flow.RegisterReceivingHandler(_ => { });

            // Act
            Action act = () => flow.RegisterReceivingHandler(_ => { });

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("*already been assigned*");
        }

        [Fact]
        public void Notify_Should_Throw_If_No_Handler_Registered()
        {
            // Arrange
            var flow = new UnicastFlow<string>();

            // Act
            Action act = () => flow.Notify("hello");

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("*No handler has been registered*");
        }

        [Fact]
        public void Notify_Should_Throw_If_Source_Is_Null()
        {
            // Arrange
            var flow = new UnicastFlow<string>();
            flow.RegisterReceivingHandler(_ => { });

            // Act
            Action act = () => flow.Notify(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("*source cannot be null*");
        }
    }
}