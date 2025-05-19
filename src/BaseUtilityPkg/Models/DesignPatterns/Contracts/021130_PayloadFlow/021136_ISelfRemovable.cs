namespace Cozyupk.HelloShadowDI.BaseUtilityPkg.Models.DesignPatterns.Contracts.PayloadFlow
{
    /// <summary>
    /// Represents an interface for objects that can determine if they are eligible for removal.
    /// </summary>
    public interface ISelfRemovable
    {
        /// <summary>
        /// Determines whether the object can be removed.
        /// </summary>
        /// <returns>True if the object can be removed; otherwise, false.</returns>
        bool CanRemove();
    }
}