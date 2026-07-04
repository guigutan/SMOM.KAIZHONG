using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 采购单附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("采购单附件")]
    public partial class PurchaseAttachment : Attachment<PurchaseOrder>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class PurchaseAttachmentRepository : AttachmentRepository<PurchaseAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class PurchaseAttachmentConfig : AttachmentEntityConfig<PurchaseAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_PO");
        }
    }
}
