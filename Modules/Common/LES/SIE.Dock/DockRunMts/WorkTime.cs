using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockRunMts
{
	/// <summary>
	/// 工作时段
	/// </summary>
	[ChildEntity, Serializable]
	[Label("工作时段")]
	public partial class WorkTime : DataEntity
	{
		#region 开始时间 BeginTime
		/// <summary>
		/// 开始时间
		/// </summary>
		[Required]
		[Label("开始时间")]
		public static readonly Property<DateTime> BeginTimeProperty = P<WorkTime>.Register(e => e.BeginTime);

		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime BeginTime
		{
			get { return GetProperty(BeginTimeProperty); }
			set { SetProperty(BeginTimeProperty, value); }
		}
		#endregion

		#region 结束时间 EndTime
		/// <summary>
		/// 结束时间
		/// </summary>
		[Required]
		[Label("结束时间")]
		public static readonly Property<DateTime> EndTimeProperty = P<WorkTime>.Register(e => e.EndTime);

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime
		{
			get { return GetProperty(EndTimeProperty); }
			set { SetProperty(EndTimeProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(2000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<WorkTime>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 月台运行维护 DockRunMt
		/// <summary>
		/// 月台运行维护Id
		/// </summary>
		public static readonly IRefIdProperty DockRunMtIdProperty = P<WorkTime>.RegisterRefId(e => e.DockRunMtId, ReferenceType.Parent);

		/// <summary>
		/// 月台运行维护Id
		/// </summary>
		public double DockRunMtId
		{
			get { return (double)GetRefId(DockRunMtIdProperty); }
			set { SetRefId(DockRunMtIdProperty, value); }
		}

		/// <summary>
		/// 月台运行维护
		/// </summary>
		public static readonly RefEntityProperty<DockRunMt> DockRunMtProperty = P<WorkTime>.RegisterRef(e => e.DockRunMt, DockRunMtIdProperty);

		/// <summary>
		/// 月台运行维护
		/// </summary>
		public DockRunMt DockRunMt
		{
			get { return GetRefEntity(DockRunMtProperty); }
			set { SetRefEntity(DockRunMtProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 工作时段 实体配置
	/// </summary>
	internal class WorkTimeConfig : EntityConfig<WorkTime>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("WORK_TIME").MapAllProperties();
			Meta.Property(WorkTime.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}