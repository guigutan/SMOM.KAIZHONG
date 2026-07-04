using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
	/// 图片上传
	/// </summary>
	[ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("图片上传")]
    public partial class MaintainPlanAttachment : Attachment<MaintainPlan>
    {

    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class MaintainPlanAttachmentRepository : AttachmentRepository<MaintainPlanAttachment>
    {
    }

    /// <summary>
    /// 图片上传 实体配置
    /// </summary>
    internal class MaintainPlanAttachmentConfig : AttachmentEntityConfig<MaintainPlanAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.MapTable("EMS_EQUIP_MAINTAIN_ATT").MapAllProperties();
            Meta.Property(MaintainPlanAttachment.ContentProperty).DontMapColumn();
            Meta.EnableDiscriminator("EMS_EQUIP_MAINTAIN");
        }
    }
}
