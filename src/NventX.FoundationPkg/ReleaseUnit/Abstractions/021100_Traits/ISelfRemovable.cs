namespace NventX.FoundationPkg.Abstractions.Traits
{
    /// <summary>
    /// Represents an interface for objects that can determine if they are eligible for removal.
    /// </summary>
    public interface ISelfRemovable
    {
        /// Note: Removability is a suggestion, not a guarantee of disuse.

        /// <summary>
        /// Determines whether the object can be removed.
        /// </summary>
        /// <remarks>
        /// Even if this method returns true, the object may still be accessed due to deferred task execution.
        /// This access might happen milliseconds, seconds, or even days later depending on the notification pipeline.
        /// Implementations must ensure they remain safe and consistent even after being marked removable.
        /// </remarks>
        /// <returns>True if the object can be removed; otherwise, false.</returns>
        bool CanRemove();
    }
}
