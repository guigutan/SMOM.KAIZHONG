using SIE.Barcodes.QrCodes;
using SIE.xUnit.Core;
using Xunit;

namespace SIE.xUnit.Barcodes.QrCodes
{
    public class QrCodeControllerTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 查询条码测试
        /// </summary>
        [Fact]
        public void GetQrCodeTest()
        {
            QrCodeController controller = RT.Service.Resolve<QrCodeController>();
            var url = "http://www.baidu.com";
            var bitmap = controller.GetQrCode(url);
            Assert.NotNull(bitmap);
        }

        /// <summary>
        /// 查询条码测试,Base64
        /// </summary>
        [Fact]
        public void GetQrCodeTest1()
        {
            QrCodeController controller = RT.Service.Resolve<QrCodeController>();
            var url = "http://www.baidu.com";
            var base64Str = controller.GetQrCodeBase64(url);
            Assert.NotNull(base64Str);
            if (base64Str != null)
            {
                var result = base64Str;
                Assert.NotNull(result);
                Assert.Contains("data:image/png;base64,", result);
            }
        }
    }
}
