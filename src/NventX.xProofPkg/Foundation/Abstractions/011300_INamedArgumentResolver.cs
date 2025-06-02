namespace NventX.xProof.Abstractions
{
    public interface INamedArgumentResolver
    {
        T Resolve<T>(string name);
    }
}
