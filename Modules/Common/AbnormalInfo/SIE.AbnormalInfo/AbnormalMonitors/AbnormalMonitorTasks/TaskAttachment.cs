using SIE;
using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{


    /// <summary>
    /// 异常任务附件
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(FileName))]
    [Label("附件")]
    public class TaskAttachment : Attachment<AbnormalMonitorTask>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(AbnormalInfoDataProvider))]
    public partial class TaskAttachmentRepository : AttachmentRepository<TaskAttachment>
    {
    }

    /// <summary>
    /// 附件 实体配置
    /// </summary>
    internal class TaskAttachmentConfig : AttachmentEntityConfig<TaskAttachment>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("AbnormalMonitorTask");
        }
    }


}