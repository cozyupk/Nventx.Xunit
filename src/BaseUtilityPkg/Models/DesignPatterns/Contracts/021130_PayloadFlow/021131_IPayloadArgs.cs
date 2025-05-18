using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow;
using Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.Traits;

namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Defines a contract for argument types that can create a new payload instance with specified metadata and body types.
    /// </summary>
    public interface IPayloadArgs<TPayloadMeta, TPayloadBody> : ISelfNewable<IPayload<TPayloadMeta, TPayloadBody>>
    {
    }
}
