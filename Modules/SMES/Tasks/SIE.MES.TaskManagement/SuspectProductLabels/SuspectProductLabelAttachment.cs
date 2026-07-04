using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 可疑品标签附件
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(FileName))]
    [Label("可疑品标签附件")]
    public class SuspectProductLabelAttachment : Attachment<SuspectProductLabel>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(TaskManagementEntityDataProvider))]
    public partial class SuspectProductLabelAttachmentRepository : AttachmentRepository<SuspectProductLabelAttachment>
    {
    }

    /// <summary>
    /// 附件 实体配置
    /// </summary>
    internal class SuspectProductLabelAttachmentConfig : AttachmentEntityConfig<SuspectProductLabelAttachment>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("SuspectProductLabel");
        }
    }
}
