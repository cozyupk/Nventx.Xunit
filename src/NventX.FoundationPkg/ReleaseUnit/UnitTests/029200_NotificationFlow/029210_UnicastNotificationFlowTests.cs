namespace NventX.Tests
{
    /*
    public class ObservableXTests
    {
        [Fact]
        public void EnableObservationBy_RegistersHandler_AndNotifiesSuccessfully()
        {
            // Arrange
            var messages = new List<string>();
            var handler = new Consumer<string>(msg => messages.Add(msg));
            var notifier = new Notifier<string>();

            // Act
            notifier.EnableObservationBy(handler);
            notifier.Notify("Hello NventX");

            // Assert
            Assert.Single(messages);
            Assert.Equal("Hello NventX", messages[0]);
        }

        [Fact]
        public void DisableObservationBy_RemovesHandler_AndNoNotificationOccurs()
        {
            // Arrange
            var messages = new List<string>();
            var handler = new Consumer<string>(msg => messages.Add(msg));
            var notifier = new Notifier<string>();
            notifier.EnableObservationBy(handler);
            notifier.DisableObservationBy(handler);

            // Act
            notifier.Notify("Message should not be received");

            // Assert
            Assert.Empty(messages);
        }

        private class DummySelfRemovableConsumer<T>(Action<T> action) : Consumer<T>(val =>
                {
                    action(val);
                }), ISelfRemovable
        {
            private bool _hasHandled = false;

            public override void Handle(T value)
            {
                base.Handle(value);
                _hasHandled = true;
            }

            public bool CanRemove() => _hasHandled;
        }

        [Fact]
        public void DisableObservationBy_AfterSelfRemoving_AndNoNotificationOccurs()
        {
            // Arrange
            var messages = new List<string>();
            var handler = new DummySelfRemovableConsumer<string>(msg => messages.Add(msg));
            var notifier = new Notifier<string>();
            notifier.EnableObservationBy(handler);

            // Act
            notifier.Notify("Message should be received");
            notifier.Notify("Message should not be received");

            // Assert
            Assert.Single(messages);
            Assert.Equal("Message should be received", messages[0]);
        }

        [Fact]
        public void ProjectionNotifier_TransformsInputBeforeNotify()
        {
            // Arrange
            var messages = new List<string>();
            var handler = new Consumer<string>(msg => messages.Add(msg));
            var notifier = new ProjectionNotifier<string, Exception>(ex => ex.Message);
            notifier.EnableObservationBy(handler);

            // Act
            notifier.Notify(new Exception("Disk full"));

            // Assert
            Assert.Single(messages);
            Assert.Equal("Disk full", messages[0]);
        }


        private class DummyAdaptable(string value) : IAdaptTo<string>
        {
            private readonly string _value = value;

            public string Adapt() => _value;
        }

        [Fact]
        public void ProjectionSwitchBoard_HandlesAndRelays_Notification()
        {
            // Arrange
            var results = new List<int>();
            var consumer = new Consumer<int>(n => results.Add(n));
            var switchBoard = new ProjectionSwitchBoard<int, string>(s => s.Length);
            switchBoard.EnableObservationBy(consumer);

            // Act
            switchBoard.Handle("TestString");

            // Assert
            Assert.Single(results);
            Assert.Equal("TestString".Length, results[0]);
        }

        public static IEnumerable<object[]> NullCtorCases =>
        [
            ["Consumer", (Action)(() => _ = new Consumer<string>(null!))],
            ["ProjectionSwitchBoard", (Action)(() => _ = new ProjectionSwitchBoard<int, string>(null!))],
            [ "SwitchBoard.EnableObservationBy", (Action)(() =>
            {
                var sb = new SwitchBoard<string>();
                sb.EnableObservationBy(null!);
            })
            ]
        ];

        [Theory]
        [MemberData(nameof(NullCtorCases))]
        public void Throws_ArgumentNull(string _, Action ctor)
        {
            Assert.Throws<ArgumentNullException>(ctor);
        }

        [Fact]
        public void AdaptionNotifier_CallsAdaptMethod_WhenNotified()
        {
            // Arrange
            var messages = new List<string>();
            var handler = new Consumer<string>(msg => messages.Add(msg));

            var adaptee = new DummyAdaptable("Adapted!");
            var notifier = new AdaptionNotifier<string, DummyAdaptable>();
            notifier.EnableObservationBy(handler);

            // Act
            notifier.Notify(adaptee);

            // Assert
            Assert.Single(messages);
            Assert.Equal("Adapted!", messages[0]);
        }

        [Fact]
        public void AdaptionSwitchBoard_AdaptsAndNotifiesHandlers()
        {
            // Arrange
            var results = new List<string>();
            var consumer = new Consumer<string>(x => results.Add(x.AddDoubleQuote()));

            var switchBoard = new AdaptionSwitchBoard<DummyAdaptable, string>();
            switchBoard.EnableObservationBy(consumer);

            var input = new DummyAdaptable("test");

            // Act
            switchBoard.Handle(input);

            // Assert
            Assert.Single(results);
            Assert.Equal("\"test\"", results[0]);
        }

        [Fact]
        public void SwitchBoard_NotifiesHandlers()
        {
            // Arrange
            var results = new List<string>();
            var consumer = new Consumer<string>(x => results.Add(x));

            var switchBoard = new SwitchBoard<string>();
            switchBoard.EnableObservationBy(consumer);

            var input = "aaa";

            // Act
            switchBoard.Handle(input);

            // Assert
            Assert.Single(results);
            Assert.Equal("aaa", results[0]);
        }

        [Fact]
        public void InvokeHandlers_ThrowsArgumentNullException_WhenBeingNotifiedIsNull()
        {
            var notifier = new Notifier<string>();
            Assert.Throws<ArgumentNullException>(() => notifier.Notify(null!));
        }

        private class ExceptionThrowingHandler : IHandlerOf<string>
        {
            public void Handle(string value)
            {
                throw new InvalidOperationException("Handler failed intentionally.");
            }
        }

        private class TestableNotifier<T>(Action onException) : Notifier<T>
        {
            private readonly Action _onException = onException;

            protected internal override void OnExceptionInNotification(IHandlerOf<T> handler, T beingNotified, Exception ex)
            {
                _onException();
            }
        }

        [Fact]
        public void OnExceptionInNotification_IsCalled_WhenHandlerThrows()
        {
            var errorLogged = false;
            var handler = new ExceptionThrowingHandler();

            var notifier = new TestableNotifier<string>(() => errorLogged = true);
            notifier.EnableObservationBy(handler);
            notifier.Notify("test");

            Assert.True(errorLogged);
        }

        private class FalseNotifiableNotifier : Notifier<string>
        {
            protected internal override bool IsNotifiable(IHandlerOf<string> handler, string beingNotified) => false;
        }

        [Fact]
        public void Notify_SkipsHandler_WhenIsNotifiableReturnsFalse()
        {
            var messages = new List<string>();
            var handler = new Consumer<string>(msg => messages.Add(msg));
            var notifier = new FalseNotifiableNotifier();
            notifier.EnableObservationBy(handler);
            notifier.Notify("Invisible message");
            Assert.Empty(messages);
        }

        [Fact]
        public void EnableObservationBy_Throws_WhenHandlerArrayContainsNull()
        {
            var notifier = new Notifier<string>();
            Assert.Throws<ArgumentNullException>(() =>
                notifier.EnableObservationBy([null!]));
        }

        [Fact]
        public void Notify_Continues_WhenHandlerThrowsException()
        {
            // Arrange: Create a notifier and a handler that throws an exception
            var messages = new List<string>();
            var ExceptionHandler = new Consumer<string>(msg => throw new Exception());
            var NormalHandler = new Consumer<string>(msg => messages.Add(msg));
            var notifier = new Notifier<string>();
            notifier.EnableObservationBy(NormalHandler);
            notifier.EnableObservationBy(ExceptionHandler);

            // Act & Assert: No exception should be thrown
            var ex = Record.Exception(() => notifier.Notify("Visible message"));
            Assert.Null(ex);
            Assert.Single(messages);
            Assert.Equal("Visible message", messages[0]);
        }

        [Fact]
        public void DisableObservationBy_IgnoresNullHandlers()
        {
            // Arrange: Create a notifier and a null handler
            var notifier = new Notifier<string>();

            // Act & Assert: No exception should be thrown
            var ex = Record.Exception(() => notifier.DisableObservationBy([null!]));
            Assert.Null(ex);
        }

    }
    */
}
