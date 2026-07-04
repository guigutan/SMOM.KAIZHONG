using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
	/// 图片上传
	/// </summary>
	[ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("图片上传")]
    public partial class CheckPlanAttachment : Attachment<CheckPlan>
    {

    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class CheckPlanAttachmentRepository : AttachmentRepository<CheckPlanAttachment>
    {
    }

    /// <summary>
    /// 图片上传 实体配置
    /// </summary>
    internal class CheckPlanAttachmentConfig : AttachmentEntityConfig<CheckPlanAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.MapTable("EMS_EQUIP_CHECK_ATT").MapAllProperties();
            Meta.Property(CheckPlanAttachment.ContentProperty).DontMapColumn();
            Meta.EnableDiscriminator("EMS_EQUIP_CHECK");
        }
    }
}
