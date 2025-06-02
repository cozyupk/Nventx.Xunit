using NventX.Xunit.Generic;

namespace NventX.Xunit.E2ETests.ForXunitV2.Mock
{
    public class NonExceptionProof : ITestProof
    {
        public void OnTestMethodStarting()
        {
            // Logic for when the test method starts
        }

        public IEnumerable<Exception> ValidateAfterProbing()
        {
            return
            [
                new Exception("(1) This is a non-exception proof test case. It should not throw an exception, but it does for testing purposes."),
                new Exception("(2) This is a non-exception proof test case. It should not throw an exception, but it does for testing purposes."),
                new Exception("(3) This is a non-exception proof test case. It should not throw an exception, but it does for testing purposes.")
            ];
        }
    }
}
