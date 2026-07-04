using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.AssetTransfers
{
    /// <summary>
    /// 资产调拨附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("资产调拨附件")]
    public partial class AssetTransferAttachment : Attachment<AssetTransfer>
    {
    }
    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class AssetTransferAttachmentRepository : AttachmentRepository<AssetTransferAttachment>
    {
    }

    /// <summary>
    /// 资产调拨附件 实体配置
    /// </summary>
    internal class AssetTransferAttachmentConfig : AttachmentEntityConfig<AssetTransferAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_ASET_TRAN");
        }
    }
}