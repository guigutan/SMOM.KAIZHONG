using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("项目附件")]
    public partial class ProjectAttachment : Attachment<Project>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class ProjectAttachmentRepository : AttachmentRepository<ProjectAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class ProjectAttachmentConfig : AttachmentEntityConfig<ProjectAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_EARL_PROJ");
        }
    }
}
