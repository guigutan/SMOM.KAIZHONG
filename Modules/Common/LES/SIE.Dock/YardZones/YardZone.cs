using SIE;
using SIE.Core.Common;
using SIE.Dock.YardMaintains;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.YardZones
{
    /// <summary>
    /// 园片区维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(YardZoneCriteria))]
    [Label("园片区维护")]
    [DisplayMember(nameof(Name))]
    public partial class YardZone : BaseRegionalInfo, IStateEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<YardZone>.Register(e => e.Code);

		/// <summary>
		/// 编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 名称 Name
		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("名称")]
		public static readonly Property<string> NameProperty = P<YardZone>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 园区 YardMaintain
		/// <summary>
		/// 园区Id
		/// </summary>
		[Label("园区")]
		public static readonly IRefIdProperty YardMaintainIdProperty =
			P<YardZone>.RegisterRefId(e => e.YardMaintainId, ReferenceType.Normal);

		/// <summary>
		/// 园区Id
		/// </summary>
		public double YardMaintainId
		{
			get { return (double)this.GetRefId(YardMaintainIdProperty); }
			set { this.SetRefId(YardMaintainIdProperty, value); }
		}

		/// <summary>
		/// 园区
		/// </summary>
		public static readonly RefEntityProperty<YardMaintain> YardMaintainProperty =
			P<YardZone>.RegisterRef(e => e.YardMaintain, YardMaintainIdProperty);

		/// <summary>
		/// 园区
		/// </summary>
		public YardMaintain YardMaintain
		{
			get { return this.GetRefEntity(YardMaintainProperty); }
			set { this.SetRefEntity(YardMaintainProperty, value); }
		}
		#endregion

		#region 经度 Longitude
		/// <summary>
		/// 经度
		/// </summary>
		[Required]
		[MinValue(-180)]
		[MaxValue(180)]
		[Label("经度")]
		public static readonly Property<double> LongitudeProperty = P<YardZone>.Register(e => e.Longitude);

		/// <summary>
		/// 经度
		/// </summary>
		public double Longitude
		{
			get { return GetProperty(LongitudeProperty); }
			set { SetProperty(LongitudeProperty, value); }
		}
		#endregion

		#region 纬度 Latitude
		/// <summary>
		/// 纬度
		/// </summary>
		[Required]
        [MinValue(-90)]
        [MaxValue(90)]
        [Label("纬度")]
		public static readonly Property<double> LatitudeProperty = P<YardZone>.Register(e => e.Latitude);

		/// <summary>
		/// 纬度
		/// </summary>
		public double Latitude
		{
			get { return GetProperty(LatitudeProperty); }
			set { SetProperty(LatitudeProperty, value); }
		}
        #endregion

        #region 排队取号围栏距离(km) Distance
        /// <summary>
        /// 排队取号围栏距离(km)
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("排队取号围栏距离(km)")]
		public static readonly Property<double> DistanceProperty = P<YardZone>.Register(e => e.Distance);

        /// <summary>
        /// 排队取号围栏距离(km)
        /// </summary>
        public double Distance
        {
			get { return GetProperty(DistanceProperty); }
			set { SetProperty(DistanceProperty, value); }
		}
		#endregion

		#region 状态 State
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<State> StateProperty = P<YardZone>.Register(e => e.State);

		/// <summary>
		/// 状态
		/// </summary>
		public State State
		{
			get { return this.GetProperty(StateProperty); }
			set { this.SetProperty(StateProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(2000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<YardZone>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
        #endregion

        #region 月台装卸能力列表 DockHandlingList
        /// <summary>
        /// 月台装卸能力列表
        /// </summary>
        public static readonly ListProperty<EntityList<DockHandling>> DockHandlingListProperty = P<YardZone>.RegisterList(e => e.DockHandlingList);
        /// <summary>
        /// 月台装卸能力列表
        /// </summary>
        public EntityList<DockHandling> DockHandlingList
        {
            get { return this.GetLazyList(DockHandlingListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 园片区维护 实体配置
    /// </summary>
    internal class YardZoneConfig : EntityConfig<YardZone>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("YARD_ZONE").MapAllProperties();
			Meta.Property(YardZone.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(YardZone.AddressProperty).ColumnMeta.HasLength(4000);
            Meta.Property(YardZone.FullAddressProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
		}
	}
}