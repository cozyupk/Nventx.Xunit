namespace Xproof.Abstractions.TestProofForTestMethods
{
    /// <summary>
    /// A marker interface representing the outcome of a probe operation,
    /// encapsulating either a return value of type <typeparamref name="T"/> or an exception thrown during probing.
    /// </summary>
    public interface IProbeOutcome<out T>
    {
        // This interface is intended for tagging result types of Probe<T> methods.
        // It does not declare any members by design.
    }
}