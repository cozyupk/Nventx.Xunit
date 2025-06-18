using Xproof.BaseProofLibrary.Proofs;
using Xproof.SupportingXunit.SourceGeneratorForAssertBinding;

namespace Xproof.SupportingXunit.AssertBinding
{
    /// <summary>
    /// A proof class that wraps <c>Xunit.Assert</c>, enabling deferred (late) failure collection during test execution.
    /// </summary>
    [GenerateAssertProof]
    public partial class AssertProof<TLabelAxes> : DefaultProof<TLabelAxes>
    {
    }

    /// <summary>
    /// A non-generic wrapper for <see cref="AssertProof{TLabelAxes}"/> that defaults to <c>string</c> as label axes.
    /// Useful for scenarios where custom axes are not required.
    /// </summary>
    public class AssertProof : AssertProof<string>
    {
        // Including this empty class allows the use of Xproof without specifying axes.
    }
}
