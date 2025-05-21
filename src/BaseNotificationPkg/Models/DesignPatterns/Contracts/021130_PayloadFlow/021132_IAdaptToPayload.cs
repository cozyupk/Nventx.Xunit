using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.Traits;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow
{
    /// <summary>
    /// Defines a contract for adapting an object to a payload type that contains metadata and body elements.
    /// </summary>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    internal interface IAdaptToPayload<out TPayloadMeta, out TPayloadBody> : IAdaptTo<IPayload<TPayloadMeta, TPayloadBody>>
    {
    }
}
