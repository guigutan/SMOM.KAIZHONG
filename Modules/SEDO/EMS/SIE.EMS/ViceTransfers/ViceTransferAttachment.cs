using SIE;
using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.ViceTransfers
{
    /// <summary>
    /// 副资产调拨附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("副资产调拨附件")]
    public partial class ViceTransferAttachment : Attachment<ViceTransfer>
    {

    }

    ///<summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class ViceTransferAttachmentRepository : AttachmentRepository<ViceTransferAttachment>
    {
    }

    /// <summary>
    /// 副资产调拨附件 实体配置
    /// </summary>
    internal class ViceTransferAttachmentConfig : AttachmentEntityConfig<ViceTransferAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_VICE_TRAN");
        }
    }
}