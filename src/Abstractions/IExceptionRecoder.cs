using System.Threading.Tasks;
using System;

namespace NventX.Xunit.Abstractions
{
    /// <summary>
    /// Defines a contract for recording exceptions thrown during the execution of test logic.
    /// Implementations are responsible for capturing the exception and making it available for later inspection.
    /// </summary>
    public interface IExceptionRecorder
    {
        /// <summary>
        /// Executes the specified synchronous action and records any exception that is thrown.
        /// </summary>
        /// <param name="action">
        /// The action to execute. Any exception thrown during execution will be captured and stored.
        /// </param>
        void Record(Action action);

        /// <summary>
        /// Executes the specified asynchronous action and records any exception that is thrown.
        /// </summary>
        /// <param name="action">
        /// A function representing the asynchronous operation to execute. Any exception thrown will be captured.
        /// </param>
        /// <returns>A task that completes when the asynchronous operation has finished.</returns>
        Task RecordAsync(Func<Task> action);

        /// <summary>
        /// Gets a value indicating whether the <see cref="Record"/> or <see cref="RecordAsync"/> method
        /// was called during the execution of the test method.
        /// This is used to ensure that exception recording was explicitly triggered by the test.
        /// </summary>
        bool RecordCalled { get; }

        /// <summary>
        /// Gets the exception instance that was captured during the execution of the recorded action, if any.
        /// If no exception was thrown, this value will be <c>null</c>.
        /// </summary>
        Exception? Captured { get; }
    }
}