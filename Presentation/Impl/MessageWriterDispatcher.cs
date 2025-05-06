using Cozyupk.HelloShadowDI.ComponentMeta.Attributes;
using Cozyupk.HelloShadowDI.Models.Contracts;
using Cozyupk.HelloShadowDI.Presentation.Contracts;

namespace Cozyupk.HelloShadowDI.Presentation.Impl
{
    [ShadowInjectable(typeof(IMessageWriterDispatcher))]
    public class MessageWriterDispatcher : IMessageWriterDispatcher
    {
        private readonly IMessageModel _messageModel;

        public Action<string>[] Writers { get; protected set; } = [
            Console.WriteLine,
            s => System.Diagnostics.Debug.WriteLine(s)
        ];

        public MessageWriterDispatcher(IMessageModel messageModel)
        {
            _messageModel = messageModel;
        }

        public void DispatchMessage()
        {
            foreach (var writer in Writers)
            {
                writer($"Message: {_messageModel.Message}");
            }
        }
    }
}
