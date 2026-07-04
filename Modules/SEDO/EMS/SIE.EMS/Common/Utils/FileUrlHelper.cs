using SIE.Common.Attachments;
using SIE.Common.Discriminator;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Maintains.Confirmations;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Common.Utils
{
    /// <summary>
    /// 文件路径帮助类
    /// </summary>
    public class FileUrlHelper
    {
        /// <summary>
        /// 获取附件Base64格式数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetAttachmentBase64StringData(string filePath, string fileName)
        {
            string data = string.Empty;
            //文件名或路径为空，退出取值(考虑是否抛异常)
            if (filePath.IsNullOrEmpty() || fileName.IsNullOrEmpty())
            {
                return data;
            }

            var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(filePath, fileName);

            if (fileBytes != null)
                data = Convert.ToBase64String(fileBytes);

            return data;
        }

        /// <summary>
        /// 生成附件Base64格式上下文
        /// </summary>
        /// <param name="attachment">实体</param>
        /// <param name="content">上下文</param>
        /// <param name="fileName">文件名</param>
        /// <param name="persistenceStatus">持久化状态</param>
        /// <returns></returns>
        public Attachment GenerateAttachmentBase64StringContent(Attachment attachment, string content, string fileName, PersistenceStatus persistenceStatus = SIE.Domain.PersistenceStatus.New)
        {
            
            if (string.IsNullOrWhiteSpace(content))
            {
                //没有文件内容则直接返回
                return attachment;
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                //空文件名则生成Guid作为文件名
                fileName = Guid.NewGuid().ToString("N");
            }

            var exts = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp", ".psd", ".svg", ".tiff", ".jfif" };
            if (!exts.Contains(System.IO.Path.GetExtension(fileName)))
            {
                throw new ValidationException("只能上传图片格式的文件".L10N());
            }

            // 解析出base64的字符，格式形如：data:application/zip;base64,UEsDBBQAAAAIAKJUU02Zjai********
            content = content.Substring(content.IndexOf(",") + 1);

            attachment.Content = Convert.FromBase64String(content);
            attachment.FileName = fileName;
            attachment.FileExtesion = System.IO.Path.GetExtension(attachment.FileName);
            attachment.FileSize = FormatFileSize(attachment.Content.Length);
            attachment.FilePath = GenerateFilePath(attachment);
            attachment.PersistenceStatus = persistenceStatus;

            return attachment;
        }

        /// <summary>
        /// 生成文件上传路径
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public string GenerateFilePath(Attachment attachment)
        {
            var discriminator = RF.Find(attachment.GetType()).EntityMeta?.GetPropertyOrDefault<string>(DiscriminatorExtension.DiscriminatorProperty.Name);
            var path = discriminator + "/" + Guid.NewGuid().ToString("N");
            return path + "/" + attachment.FileName;
        }

        /// <summary>
        /// 计算附件大小
        /// </summary>
        /// <param name="fileSize">文件大小</param>
        /// <returns>计算后文件大小</returns>
        public static string FormatFileSize(long fileSize)
        {
            const long num = 1024L; //byte

            if (fileSize < num) return fileSize + "B"; //B
            if (fileSize < Math.Pow(num, 2)) return (fileSize / num).ToString("f2") + "KB"; //KB
            if (fileSize < Math.Pow(num, 3)) return (fileSize / Math.Pow(num, 2)).ToString("f2") + "MB"; //KB
            if (fileSize < Math.Pow(num, 4)) return (fileSize / Math.Pow(num, 3)).ToString("f2") + "GB"; //KB

            return (fileSize / Math.Pow(num, 4)).ToString("f2") + "TB"; //KB
        }

        /// <summary>
        /// 生成附件Base64格式上下文
        /// </summary>
        /// <param name="maintainConfirmation">实体</param>
        /// <param name="content">上下文</param>
        /// <param name="fileName">文件名</param>
        /// <param name="persistenceStatus">持久化状态</param>
        /// <returns></returns>
        public static void GenerateMaintainConfirmationBase64StringContent(MaintainConfirmation maintainConfirmation, string content, string fileName, PersistenceStatus persistenceStatus = SIE.Domain.PersistenceStatus.New)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                //没有文件内容则直接返回
                return;
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                //空文件名则生成Guid作为文件名
                fileName = Guid.NewGuid().ToString("N");
            }

            // 解析出base64的字符，格式形如：data:application/zip;base64,UEsDBBQAAAAIAKJUU02Zjai********
            content = content.Substring(content.IndexOf(",") + 1);

            maintainConfirmation.Content = Convert.FromBase64String(content);
            maintainConfirmation.FileName = fileName;
            maintainConfirmation.FileExtesion = System.IO.Path.GetExtension(maintainConfirmation.FileName);
            ValidationFileExtesionIsImage(maintainConfirmation.FileExtesion, fileName);

            maintainConfirmation.FileSize = FormatFileSize(maintainConfirmation.Content.Length);

            maintainConfirmation.PersistenceStatus = persistenceStatus;

            var path = "MaintainConfirmation/" + Guid.NewGuid().ToString("N");

            maintainConfirmation.FilePath = path + "/" + fileName;
        }

        /// <summary>
        /// 验证图片格式
        /// </summary>
        /// <param name="fileExtension">文件扩展名</param>
        /// <param name="fileName">文件名</param>
        /// <exception cref="ValidationException"></exception>
        public static void ValidationFileExtesionIsImage(string fileExtension, string fileName)
        {
            var extStrList = new List<string>() { ".png" , ".jpg", ".bmp", ".gif", ".webp"
                , ".avif" , ".apng" , ".jfif" , ".jpeg" , ".tif", ".pcx", ".tga", ".exif", ".fpx"
                , ".svg" , ".psd" , ".cdr" , ".pcd" , ".dxf" , ".ufo" , ".eps" , ".ai" , ".raw" , ".wmf" };

            if (!extStrList.Contains(fileExtension.ToLower()))
            {
                throw new ValidationException("{0}非常规的图片格式文件，请转换后再上传！".L10nFormat(fileName));
            }
        }
    }
}
