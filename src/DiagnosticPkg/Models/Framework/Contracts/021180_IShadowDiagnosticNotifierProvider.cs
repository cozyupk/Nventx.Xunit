namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts
{
    /// </summary>
    public interface IShadowDiagnosticNotifierProvider
    {
        /// <summary>
        /// Creates a diagnostic notifier with the specified prefix.
        /// </summary>
        /// <param name="senderMeta">診断通知の送信元となるメタデータ情報。</param>
        /// <param name="category">診断通知のカテゴリ名。</param>
        /// <returns>An instance of <see cref="IShadowDiagnosticNotifier"/>.</returns>
        IShadowDiagnosticNotifier CreateDiagnosticNotifier(IShadowDiagnosableMeta senderMeta, string category);
    }
}
