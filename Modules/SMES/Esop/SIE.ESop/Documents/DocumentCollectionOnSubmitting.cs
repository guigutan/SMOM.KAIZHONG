using DocumentFormat.OpenXml.Office.CustomXsn;
using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.ComponentModel;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 添加文档集时，保存文件
    /// </summary>
    [DisplayName("添加文档集")]
    [Description("添加文档集时，保存文件")]
    public class AddDocumentCollectionToContentFile : OnSubmitting<DocumentCollection>
    {
        /// <summary>
        /// 保存文档集前触发，
        /// </summary>
        /// <param name="entity">文档集实体对象</param>
        /// <param name="e">提交事件参数</param>
        protected override void Invoke(DocumentCollection entity, EntitySubmittingEventArgs e)
        {
            if (entity != null && entity.Content != null && entity.Content.Length > 0)
            {
                var esopPath = RT.Service.Resolve<DocumentCollectionController>().GetESopDir();
                var path = $"{esopPath}/{Guid.NewGuid().ToString("N")}";
                RT.Service.Resolve<AttachmentController>().FileStorage(entity.FileName, entity.Content, path);
                entity.FilePath = $"{path}/{entity.FileName}";
                entity.IsProcessed = true;
            }
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                foreach (var item in entity.DocumentList)
                {
                    if (item.PdfPlayBeginPage.HasValue && item.PdfPlayEndPage.HasValue && item.PdfPlayBeginPage > item.PdfPlayEndPage)
                    {
                        throw new ValidationException("PDF播放结束页必须大于等于PDF播放开始页!".L10N());
                    }
                }
            }
        }
    }
}