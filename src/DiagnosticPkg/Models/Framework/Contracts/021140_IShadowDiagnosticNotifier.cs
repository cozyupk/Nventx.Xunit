using System;
using System.Collections.Generic;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts
{
    /// <summary>
    /// Interface for notifying diagnostic messages with varying severity levels.
    /// </summary>
    public interface IShadowDiagnosticNotifier
    {
        /// <summary>
        /// Sends a diagnostic notification with a specified message and severity level.
        /// </summary>
        /// <param name="sender">The source of the diagnostic notification. Can be null if the sender is not applicable.</param>
        /// <param name="message">The diagnostic message to notify. This should provide a clear and concise description of the issue or event being reported.</param>
        /// <param name="level">The severity level of the diagnostic message. Defaults to <see cref="ShadowDiagnosticLevel.Info"/> if not specified.</param>
        void Notify(object? sender, string message, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info);

        /// <summary>
        /// Sends a diagnostic notification if observers are present, using a message factory.
        /// </summary>
        /// <param name="sender">The source of the diagnostic notification. Can be null if the sender is not applicable.</param>
        /// <param name="messageFactory">A factory function that generates a list of diagnostic messages. 
        /// This function is only invoked if there are observers present to receive the messages.</param>
        /// <param name="level">The severity level of the diagnostic message. Defaults to <see cref="ShadowDiagnosticLevel.Info"/> if not specified.</param>
        void NotifyIfObserved(object? sender, Func<List<string>> messageFactory, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info);
    }
}
