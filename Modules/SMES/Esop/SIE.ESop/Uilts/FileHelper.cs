using SIE.ESop.Documents;
using System;
using System.IO;

namespace SIE.ESop.Uilts
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 格式文件大小
        /// </summary>
        /// <param name="fileSize">文件大小</param>
        /// <returns>格式后文件大小</returns>
        public static string FormatFileSize(long fileSize)
        {
            if (fileSize >= 1024 * 1024 * 1024)
            {
                return string.Format("{0:########0.00} GB", ((Double)fileSize) / (1024 * 1024 * 1024));
            }
            else if (fileSize >= 1024 * 1024)
            {
                return string.Format("{0:####0.00} MB", ((Double)fileSize) / (1024 * 1024));
            }
            else if (fileSize >= 1024)
            {
                return string.Format("{0:####0.00} KB", ((Double)fileSize) / 1024);
            }
            else
            {
                return string.Format("{0} bytes", fileSize);
            }
        }

        /// <summary>
        /// 计算哈希
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>哈希</returns>
        public static string ComputeHash(FileInfo file)
        {
            if (!file.Exists) return string.Empty;
            using (var stream = file.OpenRead())
            {
                var s = System.Security.Cryptography.MD5.Create().ComputeHash(stream);
                return BitConverter.ToString(s).Replace("-", "");
            }

        }

        /// <summary>
        /// 获取文档类型
        /// </summary>
        /// <param name="fileExtension">文档扩展名</param>
        /// <returns>文档类型</returns>
        public static DocumentType GetDocumentType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".xls":
                case ".xlsx":
                case ".pdf":
                    return DocumentType.Document;
                case ".png":
                case ".bmp":
                case ".jpg":
                case ".gif":
                case ".jpeg":
                    return DocumentType.Img;
                case ".avi":
                case ".rmvb":
                case ".rm":
                case ".mp4":
                case ".wmv":
                case ".mpg":
                case ".flv":
                case ".mov":
                case ".swf":
                case ".vob":
                case ".mkv":
                case ".mpeg":
                case ".asf":
                case ".divx":
                    return DocumentType.Video;
                default:
                    return DocumentType.Img;
            }
        }
    }
}
