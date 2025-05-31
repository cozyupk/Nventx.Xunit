namespace NventX.Xunit.Generic
{
    public interface INamedArgumentResolver
    {
        T Resolve<T>(string name);
    }
}
