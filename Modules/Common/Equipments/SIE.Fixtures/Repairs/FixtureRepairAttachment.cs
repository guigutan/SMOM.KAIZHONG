using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Fixtures;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Repairs
{
    /// <summary>
	/// 工治具报修附件
	/// </summary>
	[ChildEntity, Serializable]
    [Label("工治具报修附件")]
    public partial class FixtureRepairAttachment : Attachment<FixtureRepairDetail>
    {
    }

    ///<summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(KitFixturesEntityDataProvider))]
    public partial class FixtureRepairAttachmentRepository : AttachmentRepository<FixtureRepairAttachment>
    {
    }

    /// <summary>
    /// 工治具报修附件 实体配置
    /// </summary>
    internal class FixtureRepairAttachmentConfig : AttachmentEntityConfig<FixtureRepairAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("ELEC_REPAIR_ATT");
        }
    }
}
