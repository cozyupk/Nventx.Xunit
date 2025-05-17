using System;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl;
using Xunit;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.UnitTests
{
    public class DefaultShadowDiagnosableMetaFactoryTests
    {
        private readonly DefaultShadowDiagnosableMetaFactory _factory = new();

        [Fact]
        public void Create_WithTypeAndHashCode_ReturnsLabelWithTypeAndHash()
        {
            var args = new DefaultShadowDiagnosableCreateArgs
            {
                SenderType = typeof(DefaultShadowDiagnosableMetaFactoryTests),
                SenderHashCode = 123456
            };

            var meta = _factory.Create(args);
            Assert.Equal($"({nameof(DefaultShadowDiagnosableMetaFactoryTests)}/1e240)", meta.Label);
        }

        [Fact]
        public void Create_WithTypeOnly_ReturnsLabelWithTypeOnly()
        {
            var args = new DefaultShadowDiagnosableCreateArgs
            {
                SenderType = typeof(string)
            };

            var meta = _factory.Create(args);
            Assert.Equal("(String)", meta.Label);
        }

        [Fact]
        public void Create_WithHashOnly_ReturnsLabelWithUnknownAndHash()
        {
            var args = new DefaultShadowDiagnosableCreateArgs
            {
                SenderHashCode = 255
            };

            var meta = _factory.Create(args);
            Assert.Equal("([Unknown]/ff)", meta.Label);
        }

        [Fact]
        public void Create_WithNulls_ReturnsLabelWithUnknown()
        {
            var args = new DefaultShadowDiagnosableCreateArgs();

            var meta = _factory.Create(args);
            Assert.Equal("([Unknown])", meta.Label);
        }

        [Fact]
        public void Create_WithNullArgs_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _factory.Create(null!));
        }
    }
}
