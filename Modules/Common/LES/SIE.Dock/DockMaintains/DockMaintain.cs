using SIE.Dock.YardZones;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockMaintains
{
	/// <summary>
	/// 月台维护
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(DockMaintainCriteria))]
    [Label("月台维护")]
    [DisplayMember(nameof(Code))]
    public partial class DockMaintain : DataEntity, IStateEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<DockMaintain>.Register(e => e.Code);

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
		public static readonly Property<string> NameProperty = P<DockMaintain>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 是否收货月台 IsReceive
		/// <summary>
		/// 是否收货月台
		/// </summary>
		[Label("是否收货月台")]
		public static readonly Property<bool> IsReceiveProperty = P<DockMaintain>.Register(e => e.IsReceive);

		/// <summary>
		/// 是否收货月台
		/// </summary>
		public bool IsReceive
		{
			get { return GetProperty(IsReceiveProperty); }
			set { SetProperty(IsReceiveProperty, value); }
		}
		#endregion

		#region 是否发货月台 IsShip
		/// <summary>
		/// 是否发货月台
		/// </summary>
		/// <remarks>提货</remarks>
		[Label("是否发货月台")]
		public static readonly Property<bool> IsShipProperty = P<DockMaintain>.Register(e => e.IsShip);

		/// <summary>
		/// 是否发货月台
		/// </summary>
		public bool IsShip
		{
			get { return GetProperty(IsShipProperty); }
			set { SetProperty(IsShipProperty, value); }
		}
		#endregion

		#region 收货优先级 RecPriority
		/// <summary>
		/// 收货优先级
		/// </summary>
		/// <remarks>送货</remarks>
		[MinValue(1)]
		[Label("收货优先级")]
		public static readonly Property<int> RecPriorityProperty = P<DockMaintain>.Register(e => e.RecPriority);

		/// <summary>
		/// 收货优先级
		/// </summary>
		public int RecPriority
		{
			get { return GetProperty(RecPriorityProperty); }
			set { SetProperty(RecPriorityProperty, value); }
		}
		#endregion

		#region 发货优先级 ShipPriority
		/// <summary>
		/// 发货优先级
		/// </summary>
		[MinValue(1)]
		[Label("发货优先级")]
		public static readonly Property<int> ShipPriorityProperty = P<DockMaintain>.Register(e => e.ShipPriority);

		/// <summary>
		/// 发货优先级
		/// </summary>
		public int ShipPriority
		{
			get { return GetProperty(ShipPriorityProperty); }
			set { SetProperty(ShipPriorityProperty, value); }
		}
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<DockMaintain>.Register(e => e.State);

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
		public static readonly Property<string> RemarkProperty = P<DockMaintain>.Register(e => e.Remark);

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
		[Label("园片区")]
		public static readonly IRefIdProperty YardZoneIdProperty =
			P<DockMaintain>.RegisterRefId(e => e.YardZoneId, ReferenceType.Normal);

		/// <summary>
		/// 园片区维护Id
		/// </summary>
		public double YardZoneId
		{
			get { return (double)this.GetRefId(YardZoneIdProperty); }
			set { this.SetRefId(YardZoneIdProperty, value); }
		}

		/// <summary>
		/// 园片区维护
		/// </summary>
		public static readonly RefEntityProperty<YardZone> YardZoneProperty =
			P<DockMaintain>.RegisterRef(e => e.YardZone, YardZoneIdProperty);

		/// <summary>
		/// 园片区维护
		/// </summary>
		public YardZone YardZone
		{
			get { return this.GetRefEntity(YardZoneProperty); }
			set { this.SetRefEntity(YardZoneProperty, value); }
		}
		#endregion

		#region 列表 DockMaintainWhList
		/// <summary>
		/// 列表
		/// </summary>
		public static readonly ListProperty<EntityList<DockMaintainWh>> DockMaintainWhListProperty = P<DockMaintain>.RegisterList(e => e.DockMaintainWhList);
		/// <summary>
		/// 列表
		/// </summary>
		public EntityList<DockMaintainWh> DockMaintainWhList
		{
			get { return this.GetLazyList(DockMaintainWhListProperty); }
		}
        #endregion

        #region 视图属性
        #region 园片区编码 YardZoneCode
        /// <summary>
        /// 园片区编码
        /// </summary>
        [Label("园片区编码")]
        public static readonly Property<string> YardZoneCodeProperty = P<DockMaintain>.RegisterView(e => e.YardZoneCode, p => p.YardZone.Code);

        /// <summary>
        /// 园片区编码
        /// </summary>
        public string YardZoneCode
        {
            get { return this.GetProperty(YardZoneCodeProperty); }
        }
        #endregion

        #region 园片区名称 YardZoneName
        /// <summary>
        /// 园片区名称
        /// </summary>
        [Label("园片区名称")]
        public static readonly Property<string> YardZoneNameProperty = P<DockMaintain>.RegisterView(e => e.YardZoneName, p => p.YardZone.Name);

        /// <summary>
        /// 园片区名称
        /// </summary>
        public string YardZoneName
        {
            get { return this.GetProperty(YardZoneNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 月台维护 实体配置
    /// </summary>
    internal class DockMaintainConfig : EntityConfig<DockMaintain>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("DOCK_MAINTAIN").MapAllProperties();
			Meta.Property(DockMaintain.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}