using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Common
{
    /// <summary>
    /// 上传图片处理信息
    /// </summary>
    public class BasePictureController : DomainController
    {
        /// <summary>
        /// 验证图片格式
        /// </summary>
        /// <param name="fileExtension">文件扩展名</param>
        /// <param name="fileName">文件名</param>
        public virtual bool ValidationFileExtesionIsImage(string fileExtension, string fileName)
        {
            var extStrList = new List<string>() { ".png" , ".jpg", ".bmp", ".gif", ".webp"
                , ".avif" , ".apng" , ".jfif" , ".jpeg" , ".tif", ".pcx", ".tga", ".exif", ".fpx"
                , ".svg" , ".psd" , ".cdr" , ".pcd" , ".dxf" , ".ufo" , ".eps" , ".ai" , ".raw" , ".wmf" };

            return extStrList.Contains(fileExtension.ToLower());
        }

        /// <summary>
        /// 压缩图片分辨率
        /// </summary>
        /// <param name="imageBytes">原图片</param>
        /// <param name="minWidth">最小横向分辨率</param>
        /// <param name="minHight">最小纵向分辨率</param>
        /// <param name="ratio">压缩比例</param>
        /// <returns></returns>
        public virtual (byte[] picContent, string picSize) ToZipPictureBeforeSaving(byte[] imageBytes, int minWidth = 1980, int minHight = 1080, float ratio = 0.6f)
        {
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Image image = Image.FromStream(ms))
                {
                    int originalWidth = image.Width;
                    int originalHeight = image.Height;
                    if ((originalWidth * originalHeight <= minWidth * minHight))
                    {
                        // 分辨率不超过1k不进行压缩
                        return (imageBytes, imageBytes.Length.ToString());
                    }

                    int targetWidth = (int)(originalWidth * ratio);
                    int targetHeight = (int)(originalHeight * ratio);

                    using (Bitmap bitmap = new Bitmap(targetWidth, targetHeight))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                            graphics.DrawImage(image, 0, 0, targetWidth, targetHeight);

                            using (MemoryStream compressedMs = new MemoryStream())
                            {
                                bitmap.Save(compressedMs, ImageFormat.Jpeg);
                                byte[] compressedBytes = compressedMs.ToArray();
                                string compressedSize = compressedBytes.Length.ToString();

                                return (compressedBytes, compressedSize);
                            }
                        }
                    }
                }
            }
        }
    }
}
