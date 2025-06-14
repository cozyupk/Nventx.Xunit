namespace Xproof.Abstractions.TestProofForTestRunner
{
    /// <summary>
    /// Represents the kind of proof invocation used in test proofs.
    /// </summary>
    public enum ProofInvocationKind
    {
        SingleCase,    // Single test case invocation like a Fact in xUnit
        Parameterized, // Parameterized test case invocation like a Theory in xUnit
        Unknown        // Unknown invocation kind, used when the type of test case is not determined
    }
}
