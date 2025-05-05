using System.Runtime.Loader;
using Contracts;

namespace EntryPoint
{
    public class Program
    {
        static void Main(string[] _)
        {
            var implPath = Path.Combine(AppContext.BaseDirectory, "01200_Impl.dll");
            var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(implPath);

            System.Diagnostics.Debug.WriteLine($"Loaded assembly: {asm.FullName}");

            var targetType = typeof(IMessageModel);
            var implTypes = asm.GetTypes()
                               .Where(t => targetType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
            ;

            foreach (var implType in implTypes)
            {
                var instance = (IMessageModel)Activator.CreateInstance(implType)!;
                System.Diagnostics.Debug.WriteLine($"Created: {implType.FullName} → {instance}");
                System.Diagnostics.Debug.WriteLine($"{instance.Message}");
            }
        }
    }
}
