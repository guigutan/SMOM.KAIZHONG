using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpecialEquipment.RegularInspections
{
	/// <summary>
	/// 设备定检附件
	/// </summary>
	[ChildEntity, Serializable]
	[Label("设备定检附件")]
	public partial class RegularInspectionAttachment : Attachment<RegularInspection>
	{

	}

	/// <summary>
	///  仓库
	/// </summary>
	[DataProvider(typeof(EntityDataProvider))]
	public partial class RegularInspectionAttachmentRepository : AttachmentRepository<RegularInspectionAttachment>
	{
	}
	/// <summary>
	/// 附件资料 实体配置
	/// </summary>
	internal class RegularInspectionAttachmentConfig : AttachmentEntityConfig<RegularInspectionAttachment>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			base.ConfigMeta();
			Meta.EnableDiscriminator("EMS_REG_INS");
		}
	}
}