using Cozyupk.HelloShadowDI.Models.Contracts;
using Cozyupk.HelloShadowDI.Models.Impl;
using Unity;

namespace Cozyupk.HelloShadowDI.Models.ComponentRoots.EntryPointWithoutShadowDI
{
    public class Program
    {
        static void Main(string[] _)
        {
            // Unity コンテナの作成
            IUnityContainer container = new UnityContainer();

            // IMessageModel に MessageModel をバインド
            container.RegisterType<IMessageModel, MessageModel>();

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
