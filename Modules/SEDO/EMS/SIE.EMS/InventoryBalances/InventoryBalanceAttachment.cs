using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.InventoryBalances
{
    /// <summary>
    /// 盘点平账附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("盘点平账附件")]
    public partial class InventoryBalanceAttachment : Attachment<InventoryBalance>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class InventoryBalanceAttachmentRepository : AttachmentRepository<InventoryBalanceAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class InventoryBalanceAttachmentConfig : AttachmentEntityConfig<InventoryBalanceAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_INV_TASK");
        }
    }
}
