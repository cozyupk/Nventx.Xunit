namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface IProvable
    {
        void Probe(
            string? callerFilePath = null,
            int callerLineNumber = 0,
            string? callerMemberName = null
        );
    }
}
