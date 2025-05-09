using Cozyupk.HelloShadowDI.ComponentMeta.Attributes;
using Cozyupk.HelloShadowDI.Models.Contracts;
using Cozyupk.HelloShadowDI.Presentation.Contracts;

namespace Cozyupk.HelloShadowDI.Presentation.Impl
{
    /// <summary>
    /// Implements the IMessageWriterDispatcher interface to dispatch messages
    /// using a collection of writers. Writers can output messages to various
    /// destinations such as the console or debug output.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the MessageWriterDispatcher class
    /// with the specified message model.
    /// </remarks>
    /// <param name="messageModel">The message model containing the message to dispatch.</param>
    [ShadowInjectable(typeof(IMessageWriterDispatcher))]
    public class MessageWriterDispatcher(IMessageModel messageModel) : IMessageWriterDispatcher
    {
        private readonly IMessageModel _messageModel = messageModel;

        /// <summary>
        /// A collection of actions that define how messages are written.
        /// By default, messages are written to the console and debug output.
        /// </summary>
        public Action<string>[] Writers { get; protected set; } = [
            Console.WriteLine,
            s => System.Diagnostics.Debug.WriteLine(s)
        ];

        /// <summary>
        /// Dispatches the message from the message model to all configured writers.
        /// </summary>
        public void DispatchMessage()
        {
            foreach (var writer in Writers)
            {
                writer($"Message: {_messageModel.Message}");
            }
        }
    }
}
