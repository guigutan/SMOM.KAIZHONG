using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.AssetDisposals
{
    /// <summary>
    /// 资产处置附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("附件")]
    public partial class AssetDisposalAttachment : Attachment<AssetDisposal>
    { }

    /// <summary>
    /// 资产处置附件的数据仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class AssetDisposalAttachmentRepository : AttachmentRepository<AssetDisposalAttachment>
    { }

    /// <summary>
    /// 资产处置附件 实体配置
    /// </summary>
    internal class AssetDisposalAttachmentConfig : AttachmentEntityConfig<AssetDisposalAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_ASET_DSPO");
        }
    }
}