namespace Cozyupk.Shadow.Flow.BaseNotificationPkg.Models.Contracts.SenderPayloadFlow
{
    public interface IPayloadNotifyCondition<in TPayloadMeta>
    {
        bool ShouldNotify(TPayloadMeta payloadMeta);
    }

    public interface ISenderAwareNotifyCondition<in TSenderMeta, in TPayloadMeta>
    {
        bool ShouldNotify(TSenderMeta senderMeta, TPayloadMeta payloadMeta);
    }
}