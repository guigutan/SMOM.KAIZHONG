using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 设备润滑附件
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("设备润滑附件")]
	public partial class LubricationAttachment : Attachment<Lubrication>
	{
		#region 润滑项目 LubricationDetail
		/// <summary>
		/// 润滑项目Id
		/// </summary>
		public static readonly IRefIdProperty LubricationDetailIdProperty = P<LubricationAttachment>.RegisterRefId(e => e.LubricationDetailId, ReferenceType.Normal);

		/// <summary>
		/// 润滑项目Id
		/// </summary>
		public double? LubricationDetailId
		{
			get { return (double?)GetRefNullableId(LubricationDetailIdProperty); }
			set { SetRefNullableId(LubricationDetailIdProperty, value); }
		}

		/// <summary>
		/// 润滑项目
		/// </summary>
		public static readonly RefEntityProperty<LubricationDetail> LubricationDetailProperty = P<LubricationAttachment>.RegisterRef(e => e.LubricationDetail, LubricationDetailIdProperty);

		/// <summary>
		/// 润滑项目
		/// </summary>
		public LubricationDetail LubricationDetail
		{
			get { return GetRefEntity(LubricationDetailProperty); }
			set { SetRefEntity(LubricationDetailProperty, value); }
		}
		#endregion
	}


	/// <summary>
	///  仓库
	/// </summary>
	[DataProvider(typeof(EmsEntityDataProvider))]
	public partial class LubricationAttachmentRepository : AttachmentRepository<LubricationAttachment>
	{
	}

	/// <summary>
	/// 设备润滑附件 实体配置
	/// </summary>
	internal class LubricationAttachmentConfig : EntityConfig<LubricationAttachment>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_LUBR_ATT").MapAllPropertiesExcept(Attachment.ContentProperty);
			Meta.EnablePhantoms();
		}
	}
}