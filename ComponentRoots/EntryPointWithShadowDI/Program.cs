using Cozyupk.HelloShadowDI.ComponentMeta.Utils;
using Cozyupk.HelloShadowDI.Models.Contracts;
using Unity;

namespace Cozyupk.HelloShadowDI.Models.ComponentRoots.EntryPointWithShadowDI
{
    public class Program
    {
        static void Main(string[] _)
        {
            // Unity コンテナの作成
            IUnityContainer container = new UnityContainer();

            // Shadow DI でバインド
            var injector = new DynamicShadowInjector(AppContext.BaseDirectory);
            injector.Inject(container);

            // DI によって IMessageModel のインスタンス取得
            var model = container.Resolve<IMessageModel>();

            // メッセージ表示
            var writers = new Action<string>[]
            {
                s => Console.WriteLine(s),
                s => System.Diagnostics.Debug.WriteLine(s)
            };
            foreach (var method in writers)
            {
                method($"Message: {model.Message}");
            }
        }
    }
}
