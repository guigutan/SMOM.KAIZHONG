using System.IO;

namespace SIE.Wpf.ESOP.Common
{
    /// <summary>
    /// 下载帮助类
    /// </summary>
    public static class DownloadHelper
    {
        /// <summary>
        /// 下载文件到临时路径
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="savePath">保存路径</param>
        public static void FileDownload(string filePath, string savePath)
        {
            var fileName = Path.GetFileName(filePath);
            var fileBytes = RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileDownload(filePath, fileName);
            var tempFile = Path.Combine(savePath, fileName);
            FileStream pFileStream = new FileStream(tempFile, FileMode.OpenOrCreate);
            pFileStream.Write(fileBytes, 0, fileBytes.Length);
            pFileStream.Close();
        }

    }
}