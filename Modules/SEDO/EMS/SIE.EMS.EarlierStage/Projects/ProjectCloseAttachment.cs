using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目结项附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("项目结项附件")]
    public partial class ProjectCloseAttachment : Attachment<ProjectClose>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class ProjectCloseAttachmentRepository : AttachmentRepository<ProjectCloseAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class ProjectCloseAttachmentConfig : AttachmentEntityConfig<ProjectCloseAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_EARL_PRO_CLOSE");
        }
    }
}
