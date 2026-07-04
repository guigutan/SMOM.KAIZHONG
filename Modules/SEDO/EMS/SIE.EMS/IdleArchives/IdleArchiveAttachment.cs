using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.IdleArchives
{

    /// <summary>
    /// 闲置与封存附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("闲置与封存附件")]
    public partial class IdleArchiveAttachment : Attachment<IdleArchive>
    {
    }
    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class IdleArchiveAttachmentRepository : AttachmentRepository<IdleArchiveAttachment>
    {
    }

    /// <summary>
    /// 资产调拨附件 实体配置
    /// </summary>
    internal class IdleArchiveAttachmentConfig : AttachmentEntityConfig<IdleArchiveAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_IDLE_ARCH");
        }
    }
}