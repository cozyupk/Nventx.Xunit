using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NventX.xProof.Abstractions.TestProofForTestRunner;

namespace NventX.xProof.BaseProofLibrary.BaseImpl
{
    public sealed class ProbeScope : IDisposable, IAsyncDisposable
    {
        private IInvokableProof InvokableProof { get; }
        private Stopwatch Sw { get; } = Stopwatch.StartNew();
        private bool Disposed { get; set; } = false;
        private bool Completed { get; set; } = false;
        private bool AsyncDisposed { get; set; } = false;

        private string? Label { get; }
        private Delegate Act { get; }
        private string? CallerFilePath { get; }
        private int? CallerLineNumber { get; }
        private string? CallerMemberName { get; }
        private int? Index { get; }
        private int? TotalCount { get; }

        public ProbeScope(
            IInvokableProof invokableProof, Delegate act, string? label, 
            string? callerFilePath, int? callerLineNumber, string? callerMemberName,
            int? index, int? totalCount
        ) {
            InvokableProof = invokableProof ?? throw new ArgumentNullException(nameof(invokableProof), "InvokableProof cannot be null.");
            Act = act ?? throw new ArgumentNullException(nameof(act), "Action cannot be null.");
            Label = label;
            Index = index;
            TotalCount = totalCount;
            CallerFilePath = callerFilePath;
            CallerLineNumber = callerLineNumber;
            CallerMemberName = callerMemberName;
        }

        public void Success()
        {
            if (!Completed)
            {
                Completed = true;
                Sw.Stop();
                InvokableProof.RecordProobingSuccess(Sw.Elapsed);
            }
        }

        public void Failure(Exception ex)
        {
            if (!Completed)
            {
                Sw.Stop();
                Completed = true;
                InvokableProof.RecordProbingFailure(
                    ex, Sw.Elapsed, Act, Label, CallerFilePath, CallerLineNumber, CallerMemberName,
                    Index, TotalCount
                );
            }
        }

        public void Dispose()
        {
            if (Disposed) return;

            Sw.Stop();

            if (!Completed)
            {
                InvokableProof.RecordProbingFailure(
                    new InvalidOperationException("ProbeScope.Dispose() called without Success() or Failure()."),
                    Sw.Elapsed, Act, Label,
                    CallerFilePath, CallerLineNumber, CallerMemberName,
                    Index, TotalCount
                );
            }

            Disposed = true;
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (Disposed) return;

            Sw.Stop();

            if (!Completed)
            {
                InvokableProof.RecordProbingFailure(
                    new InvalidOperationException("ProbeScope.DisposeAsync() called without Success() or Failure()."),
                    Sw.Elapsed, Act, Label, CallerFilePath, CallerLineNumber, CallerMemberName,
                    Index, TotalCount
                );
            }

            Disposed = true;
            AsyncDisposed = true;
            GC.SuppressFinalize(this);

            await Task.Yield(); // force async path if awaited
        }

        ~ProbeScope()
        {
            if (!Disposed)
            {
                Sw.Stop();

                InvokableProof.RecordProbingFailure(
                    new InvalidOperationException("ProbeScope was finalized without Dispose()/DisposeAsync()."),
                    Sw.Elapsed, Act, Label, CallerFilePath, CallerLineNumber, CallerMemberName,
                    Index, TotalCount
                );
            }
        }
    }
}
