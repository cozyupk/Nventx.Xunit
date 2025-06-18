#pragma warning disable IDE0130 // The namespace does not match the folder structure
namespace System.Diagnostics.CodeAnalysis
#pragma warning restore IDE0130 // The namespace does not match the folder structure
{
    /// <summary>
    /// Local stub for AllowNullAttribute to support compatibility with .NET Standard 2.0.
    /// 
    /// This attribute is normally defined in System.Diagnostics.CodeAnalysis 
    /// in .NET Core 3.0 / .NET Standard 2.1 and later.
    /// 
    /// At runtime, if the environment provides a public definition, this internal version will be ignored.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    internal sealed class AllowNullAttribute : Attribute { }
}