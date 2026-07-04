using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常履历
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("异常履历")]
	public partial class AbnormalResume : DataEntity
	{
		#region 异常任务 AbnormalTask
		/// <summary>
		/// 异常任务
		/// </summary>
		[Label("异常任务")]
		[MaxLength(1000)]
		public static readonly Property<string> AbnormalTaskProperty = P<AbnormalResume>.Register(e => e.AbnormalTask);

		/// <summary>
		/// 异常任务
		/// </summary>
		public string AbnormalTask
		{
			get { return GetProperty(AbnormalTaskProperty); }
			set { SetProperty(AbnormalTaskProperty, value); }
		}
		#endregion

		#region 异常定义 AbnormalDefine
		/// <summary>
		/// 异常定义Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalDefineIdProperty = P<AbnormalResume>.RegisterRefId(e => e.AbnormalDefineId, ReferenceType.Parent);

		/// <summary>
		/// 异常定义Id
		/// </summary>
		public double AbnormalDefineId
		{
			get { return (double)GetRefId(AbnormalDefineIdProperty); }
			set { SetRefId(AbnormalDefineIdProperty, value); }
		}

		/// <summary>
		/// 异常定义
		/// </summary>
		public static readonly RefEntityProperty<AbnormalDefine> AbnormalDefineProperty = P<AbnormalResume>.RegisterRef(e => e.AbnormalDefine, AbnormalDefineIdProperty);

		/// <summary>
		/// 异常定义
		/// </summary>
		public AbnormalDefine AbnormalDefine
		{
			get { return GetRefEntity(AbnormalDefineProperty); }
			set { SetRefEntity(AbnormalDefineProperty, value); }
		}
		#endregion

	}

	/// <summary>
	/// 异常履历 实体配置
	/// </summary>
	internal class AbnormalResumeConfig : EntityConfig<AbnormalResume>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNORMAL_RESUME").MapAllProperties();
			Meta.Property(AbnormalResume.AbnormalTaskProperty).ColumnMeta.HasLength(3000);
			Meta.EnablePhantoms();
		}
	}
}