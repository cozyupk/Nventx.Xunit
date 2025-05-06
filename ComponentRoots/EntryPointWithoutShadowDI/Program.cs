using Cozyupk.HelloShadowDI.Models.Contracts;
using Cozyupk.HelloShadowDI.Models.Impl;
using Cozyupk.HelloShadowDI.Presentation.Contracts;
using Cozyupk.HelloShadowDI.Presentation.Impl;
using Unity;

namespace Cozyupk.HelloShadowDI.Models.ComponentRoots.EntryPointWithoutShadowDI
{
    public class Program
    {
        static void Main(string[] _)
        {
            // Unity コンテナの作成
            IUnityContainer container = new UnityContainer();

            // 必要な実装を手動でバインド
            container.RegisterType<IMessageModel, MessageModel>();
            container.RegisterType<IMessageWriterDispatcher, MessageWriterDispatcher>();

            // DI によって IMessageWriteDispatcher のインスタンス取得
            var dispatcher = container.Resolve<IMessageWriterDispatcher>();

            // メッセージ表示
            dispatcher.DispatchMessage();
        }
    }
}
