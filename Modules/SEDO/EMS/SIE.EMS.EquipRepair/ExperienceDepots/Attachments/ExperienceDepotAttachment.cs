using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ExperienceDepots.Attachments
{
    /// <summary>
    /// 图片附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("图片附件")]
    public class ExperienceDepotAttachment : Attachment<ExperienceDepot>
    {
    }

    /// <summary>
    /// 仓库
    /// </summary>
    [DataProvider(typeof(EquipRepairEntityDataProvider))]
    public partial class ExperienceDepotAttachmentRepository : AttachmentRepository<ExperienceDepotAttachment>
    {

    }
    /// <summary>
    /// 班级附件配置
    /// </summary>
    public class ExperienceDepotAttachmentConfig : AttachmentEntityConfig<ExperienceDepotAttachment>
    {
        /// <summary>  
        /// 配置  
        /// </summary>  
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            // 鉴别器，对应bd_attachment附件管理表中的DISCRIMINATOR  
            Meta.EnableDiscriminator("ExperienceDepotAttachment");
        }
    }
}
