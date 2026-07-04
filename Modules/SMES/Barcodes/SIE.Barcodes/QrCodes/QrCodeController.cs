using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace SIE.Barcodes.QrCodes
{
    /// <summary>
    /// 二维码控制器
    /// </summary>
    public class QrCodeController : DomainController
    {
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pixel"></param>
        /// <returns></returns>
        [IgnoreProxy]
        public virtual Bitmap GetQrCode(string content, int pixel = 10)
        {
            try
            {
                QRCodeGenerator generator = new QRCodeGenerator();
                QRCodeData codeData = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M, true);
                QRCoder.QRCode qRCode = new QRCode(codeData);
                var bitmap = qRCode.GetGraphic(pixel);
                return bitmap;
            }
            catch (Exception ex)
            {
                Logging.LogManager.Logger.Error("获取二维码失败。".L10N(), ex);
                return null;
            }
        }

        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="pixel">显示倍数</param>
        /// <param name="isImage">是否图像，如果是，补全base64格式前缀</param>
        /// <returns></returns>
        [IgnoreProxy]
        public virtual string GetQrCodeBase64(string content, bool isImage = true, int pixel = 10)
        {
            try
            {
                var bitmap = GetQrCode(content, pixel);
                if (bitmap != null)
                {
                    var strBase64 = ToBase64(bitmap);
                    if (isImage)
                        return "data:image/png;base64," + strBase64;
                    else
                        return strBase64;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Logging.LogManager.Logger.Error("获取二维码失败。".L10N(), ex);
                return null;
            }
        }

        /// <summary>
        /// 转换成64位
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        [IgnoreProxy]
        private string ToBase64(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = ms.ToArray();
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return strbaser64;
            }
        }
    }
}
