namespace NventX.Xunit.Generic
{
    public interface ITestProof
    {
        void OnTestMethodStarting();

        void OnTestMethodCompleted();
    }
}
