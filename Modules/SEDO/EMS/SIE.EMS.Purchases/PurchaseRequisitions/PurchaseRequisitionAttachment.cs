using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购申请附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("采购申请附件")]
    public partial class PurchaseRequisitionAttachment : Attachment<PurchaseRequisition>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class PurchaseRequisitionAttachmentRepository : AttachmentRepository<PurchaseRequisitionAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class PurchaseRequisitionAttachmentConfig : AttachmentEntityConfig<PurchaseRequisitionAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_PR");
        }
    }
}
