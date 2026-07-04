using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.AssetRequisitions
{
    /// <summary>
    /// 资产领用附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("资产领用附件")]
    public partial class AssetRequisitionAttachment : Attachment<AssetRequisition>
    {

    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class AssetRequisitionAttachmentRepository : AttachmentRepository<AssetRequisitionAttachment>
    {
    }

    /// <summary>
    /// 资产领用附件 实体配置
    /// </summary>
    internal class AssetRequisitionAttachmentConfig : AttachmentEntityConfig<AssetRequisitionAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_ASET_REQ");
        }
    }
}