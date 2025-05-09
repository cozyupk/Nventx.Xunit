using System;

namespace Cozyupk.HelloShadowDI.ComponentMeta.Attributes
{
    public sealed class DebuggableApplicationParametersAttribute : Attribute
    {
        // 外からはMSBuildによって文字列が注入される
        public string IsDebugOutputTarget
        {
            get => _isDebugOutputTargetString;
            set
            {
                _isDebugOutputTargetString = value;
                _isDebugOutputTarget = !bool.TryParse(value, out var result) || result;
            }
        }

        // 内部で保持するstringとboolの状態
        private string _isDebugOutputTargetString = "true";
        private bool _isDebugOutputTarget = true;

        // アプリ側から使うときはこっち！（型安全）
        public bool IsDebugOutputTargetBool => _isDebugOutputTarget;
    }
}
