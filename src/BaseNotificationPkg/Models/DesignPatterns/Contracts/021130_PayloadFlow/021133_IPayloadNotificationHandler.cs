using Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.NotificationFlow;

namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.PayloadFlow
{
    /// <summary>
    /// Defines a contract for handling notifications that carry a payload consisting of metadata and body data.
    /// </summary>
    /// <typeparam name="TPayloadMeta">The type of the payload metadata.</typeparam>
    /// <typeparam name="TPayloadBody">The type of the payload body.</typeparam>
    internal interface IPayloadNotificationHandler<out TPayloadMeta, out TPayloadBody> : INotificationHandler<IPayload<TPayloadMeta, TPayloadBody>>
    {
    }
}
