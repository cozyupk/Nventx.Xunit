using Cozyupk.HelloShadowDI.ComponentMeta.Attributes;
using Cozyupk.HelloShadowDI.Models.Contracts;

namespace Cozyupk.HelloShadowDI.Models.Impl
{
    [ShadowInjectable(typeof(IMessageModel))]
    public class MessageModel : IMessageModel
    {
        public string Message => "Hello world.";
    }
}
