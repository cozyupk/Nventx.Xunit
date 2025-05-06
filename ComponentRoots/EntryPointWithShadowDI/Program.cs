using Cozyupk.HelloShadowDI.ComponentMeta.Utils;
using Cozyupk.HelloShadowDI.Presentation.Contracts;
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

            // DI によって IMessageWriteDispatcher のインスタンス取得
            var dispatcher = container.Resolve<IMessageWriterDispatcher>();

            // メッセージ表示
            dispatcher.DispatchMessage();
        }
    }
}
