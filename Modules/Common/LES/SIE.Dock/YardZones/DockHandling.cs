using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.YardZones
{
	/// <summary>
	/// 月台装卸能力
	/// </summary>
	[ChildEntity, Serializable]
	[Label("月台装卸能力")]
	public partial class DockHandling : DataEntity
	{
		#region 开始时间 BeginTime
		/// <summary>
		/// 开始时间
		/// </summary>
		[Required]
		[Label("开始时间")]
		public static readonly Property<string> BeginTimeProperty = P<DockHandling>.Register(e => e.BeginTime);

		/// <summary>
		/// 开始时间
		/// </summary>
		public string BeginTime
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
		public static readonly Property<string> EndTimeProperty = P<DockHandling>.Register(e => e.EndTime);

		/// <summary>
		/// 结束时间
		/// </summary>
		public string EndTime
		{
			get { return GetProperty(EndTimeProperty); }
			set { SetProperty(EndTimeProperty, value); }
		}
		#endregion

		#region 送货可预约数 ShipAppoNum
		/// <summary>
		/// 送货可预约数
		/// </summary>
		[Required]
		[MinValue(0)]
		[Label("送货可预约数")]
		public static readonly Property<int> ShipAppoNumProperty = P<DockHandling>.Register(e => e.ShipAppoNum);

		/// <summary>
		/// 送货可预约数
		/// </summary>
		public int ShipAppoNum
		{
			get { return GetProperty(ShipAppoNumProperty); }
			set { SetProperty(ShipAppoNumProperty, value); }
		}
		#endregion

		#region 提货可预约数 ReceiveAppoNum
		/// <summary>
		/// 提货可预约数
		/// </summary>
		[Required]
		[MinValue(0)]
		[Label("提货可预约数")]
		public static readonly Property<int> ReceiveAppoNumProperty = P<DockHandling>.Register(e => e.ReceiveAppoNum);

		/// <summary>
		/// 提货可预约数
		/// </summary>
		public int ReceiveAppoNum
		{
			get { return GetProperty(ReceiveAppoNumProperty); }
			set { SetProperty(ReceiveAppoNumProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(2000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<DockHandling>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
        #endregion

        #region 园片区维护 YardZone
        /// <summary>
        /// 园片区维护Id
        /// </summary>
        public static readonly IRefIdProperty YardZoneIdProperty = P<DockHandling>.RegisterRefId(e => e.YardZoneId, ReferenceType.Parent);

		/// <summary>
		/// 园片区维护Id
		/// </summary>
		public double YardZoneId
        {
			get { return (double)GetRefId(YardZoneIdProperty); }
			set { SetRefId(YardZoneIdProperty, value); }
		}

		/// <summary>
		/// 园片区维护
		/// </summary>
		public static readonly RefEntityProperty<YardZone> YardZoneProperty = P<DockHandling>.RegisterRef(e => e.YardZone, YardZoneIdProperty);

		/// <summary>
		/// 园片区维护
		/// </summary>
		public YardZone YardZone
        {
			get { return GetRefEntity(YardZoneProperty); }
			set { SetRefEntity(YardZoneProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 月台装卸能力 实体配置
	/// </summary>
	internal class DockHandlingConfig : EntityConfig<DockHandling>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("DOCK_HANDLING").MapAllProperties();
			Meta.Property(DockHandling.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}