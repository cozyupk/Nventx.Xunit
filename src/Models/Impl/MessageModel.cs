using Cozyupk.HelloShadowDI.ComponentMeta.Attributes;
using Cozyupk.HelloShadowDI.Models.Contracts;

namespace Cozyupk.HelloShadowDI.Models.Impl
{
    /// <summary>
    /// Implementation of the IMessageModel interface.
    /// Provides a default message "Hello world.".
    /// This class is marked as ShadowInjectable for dependency injection.
    /// </summary>
    [ShadowInjectable(typeof(IMessageModel))]
    public class MessageModel : IMessageModel
    {
        public string Message => "Hello world.";
    }
}
