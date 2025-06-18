using System;

namespace Xproof.SupportingXunit.SourceGeneratorForAssertBinding
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GenerateAssertProofAttribute : Attribute
    {
    }
}
