using SIE.Common.Utils;
using SIE.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 添加文档时，保存文件
    /// </summary>
    [System.ComponentModel.DisplayName("添加文档")]
    [System.ComponentModel.Description("添加文档时，保存文件")]
    public class AddDocumentToContentFile : OnSubmitting<Document>
    {
        /// <summary>
        /// 文档提交前执行逻辑(保存文档的数据到指定位置)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(Document entity, EntitySubmittingEventArgs e)
        {
            if (entity != null && entity.Content != null && entity.Content.Length > 0)
            {
                var esopPath = RT.Service.Resolve<DocumentCollectionController>().GetESopDir();
                var path = $"{esopPath}/{Guid.NewGuid().ToString("N")}";
                RT.Service.Resolve<Common.Attachments.AttachmentController>().FileStorage(entity.FileName, entity.Content, path);
                var filePath = $"{path}/{entity.FileName}";
                entity.FilePath = filePath;

                if (entity.FileExtension.IsNullOrEmpty() && entity.FileSize.IsNullOrEmpty())
                {
                    var info = new FileInfo(filePath);
                    entity.Md5 = FileHelper.ComputeHash(info);
                    entity.FileExtension = info.Extension;
                    entity.FileName = info.Name;
                    entity.FileSize = FileHelper.FormatFileSize(info.Length);
                    entity.DocumentType = RT.Service.Resolve<DocumentCollectionController>().GetDocumentType(info.Extension);
                    entity.IsProcessed = true;
                }
                if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
                {
                    if (entity.PdfPlayBeginPage.HasValue && entity.PdfPlayEndPage.HasValue&& entity.PdfPlayBeginPage>entity.PdfPlayEndPage)
                    {
                        throw new ValidationException("PDF播放结束页必须大于等于PDF播放开始页!".L10N());
                    }
                }
            }

        }
    }
}
