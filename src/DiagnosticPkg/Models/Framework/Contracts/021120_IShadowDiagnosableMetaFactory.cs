namespace Cozyupk.Shadow.Flow.DiagnosticPkg.Models.Framework.Contracts
{
    /// <summary>
    /// Factory interface for creating <see cref="IShadowDiagnosableMeta"/> instances from metadata information.
    /// </summary>
    /// <typeparam name="TMetaInfo">The type of metadata information used to create the diagnosable meta instance.</typeparam>
    public interface IShadowDiagnosableMetaFactory<in TMetaInfo>
    {
        /// <summary>
        /// Creates an <see cref="IShadowDiagnosableMeta"/> instance from the specified metadata information.
        /// </summary>
        /// <param name="metaInfo">The metadata information used to create the diagnosable meta instance.</param>
        /// <returns>An instance of <see cref="IShadowDiagnosableMeta"/>.</returns>
        IShadowDiagnosableMeta Create(TMetaInfo metaInfo);
    }
}
