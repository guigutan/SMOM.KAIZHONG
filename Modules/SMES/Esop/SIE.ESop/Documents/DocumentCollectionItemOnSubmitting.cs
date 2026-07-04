using SIE.Domain;
using System.ComponentModel;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档集关联物料处理规则
    /// </summary>
    [DisplayName("文档集关联物料处理规则")]
    [Description("文档集关联物料,把物料从以前关联的文档集移除")]
    public class DocumentCollectionItemRemoveOldItemRule : OnSubmitting<DocumentCollectionItem>
    {
        /// <summary>
        /// 文档集提交时,把物料从以前关联的文档集移除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="e">提交参数对象</param>
        protected override void Invoke(DocumentCollectionItem entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
                RT.Service.Resolve<DocumentCollectionController>().DeleteOldRefCollectionItem(entity.DocumentCollectionId, entity.ItemId);
        }
    }
}