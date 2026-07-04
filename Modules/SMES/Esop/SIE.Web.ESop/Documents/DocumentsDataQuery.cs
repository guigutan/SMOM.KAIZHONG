using Microsoft.AspNetCore.StaticFiles;
using SIE.Common.Attachments;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop.Documents;
using SIE.Resources.Employees;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SIE.Web.ESop.Documents.DataQuerys
{
    /// <summary>
    /// dataquery
    /// </summary>
    public class DocumentsDataQuery : DataQueryer
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileNamme"></param>
        /// <param name="documentCollection"></param>
        /// <returns></returns>
        public Tuple<DocumentCollection, EntityList<Document>> UploadFile(string fileContent, string fileNamme, DocumentCollection documentCollection)
        {

            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }
            if (fileContent.Split(',').Length > 1)
            {
                var fileContentGB = Convert.FromBase64String(fileContent.Split(',')[1]);

                var esopPath = RT.Service.Resolve<DocumentCollectionController>().GetESopDir();
                var path = $"{esopPath}/{Guid.NewGuid().ToString("N")}";
                RT.Service.Resolve<AttachmentController>().FileStorage(fileNamme, fileContentGB, path);

                var filePath = $"{path}/{fileNamme}";
                documentCollection.FilePath = filePath;
                //限制只能上传表格文件
                var fileExtension = fileNamme.Substring(fileNamme.LastIndexOf(".") + 1);
                if (fileExtension != "xls" && fileExtension != "xlsx")
                {
                    throw new ValidationException("只能上传excel文件!".L10N());
                }

                var res = RT.Service.Resolve<DocumentCollectionController>().AnalysisAttachment(documentCollection);
                res.Md5 = GetMD5WithString(fileContentGB);
                res.IsProcessed = true;
                if (res.DocumentList.DeletedList.Any())
                {
                    res.DocumentList.DeletedList.Clear();
                }
                return new Tuple<DocumentCollection, EntityList<Document>>(res, res.DocumentList);
            }
            else
            {
                throw new ValidationException("文件内容异常，不能上传。".L10N());
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileNamme"></param>
        /// <returns></returns>
        public virtual UploadResult SaveListAttachment(string fileContent, string fileNamme)
        {
            if (fileContent.Split(',').Length > 1)
            {
                var fileContentGB = Convert.FromBase64String(fileContent.Split(',')[1]);
                var md5 = GetMD5WithString(fileContentGB);
                var esopPath = RT.Service.Resolve<DocumentCollectionController>().GetESopDir();
                var path = $"{esopPath}/{Guid.NewGuid().ToString("N")}";
                RT.Service.Resolve<AttachmentController>().FileStorage(fileNamme, fileContentGB, path);
                var size = fileContentGB.Length / 1024 + "kb";
                var filePath = $"{path}/{fileNamme}";
                var res = new UploadResult()
                {
                    FileName = fileNamme,
                    FilePath = filePath,
                    Md5 = md5,
                    Size = size,
                    FileExtension = fileNamme.Substring(fileNamme.LastIndexOf(".")),
                    IsProcessed = true

                };
                res.DocumentType = RT.Service.Resolve<DocumentCollectionController>().GetDocumentType(res.FileExtension);
                return res;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public object DownLoadPictureAttachment(string filePath, string fileName)
        {
            var pathFileName = System.IO.Path.GetFileName(filePath);
            var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(filePath, pathFileName);
            return new { FileName = fileName, FileContent = FileByteToBase64(fileBytes, pathFileName) };
        }

        /// <summary>
        /// 文件转Base64
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string FileByteToBase64(byte[] buffer, string fileName)
        {
            Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
            Check.NotNull(buffer, nameof(buffer));

            var fileExt = Path.GetExtension(fileName);
            if (fileExt.IsNullOrWhiteSpace())
            {
                throw new ValidationException("{0}文件没有扩展名，无法解析对应的文件类型".L10nFormat(fileName));
            }
            var provider = new FileExtensionContentTypeProvider();
            var contentType = provider.Mappings[fileExt];
            if (contentType.IsNullOrWhiteSpace())
            {
                throw new ValidationException("{0}扩展名无法解析对应的类型，请在应用服务的{1}节点配置中进行维护".L10nFormat(fileExt, "MimeMap"));
            }
            return "data:{0};base64,{1}".FormatArgs(contentType, Convert.ToBase64String(buffer));
        }


        /// <summary>
        ///获取字节流的MD5
        /// </summary>
        /// <param name="fileContentGB"></param>
        /// <returns></returns>
        private string GetMD5WithString(byte[] fileContentGB)
        {
            var str = new StringBuilder();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(fileContentGB);
            for (int i = 0; i < bytes.Length; i++)
            {
                str.Append(bytes[i].ToString("x2"));
            }
            return str.ToString();
        }
    }
    /// <summary>
    /// 上传结果类
    /// </summary>
    [Serializable]
    public class UploadResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Md5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsProcessed { get; set; }
    }
}
