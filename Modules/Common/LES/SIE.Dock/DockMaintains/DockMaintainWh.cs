using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Dock.DockMaintains
{
    /// <summary>
    /// 月台维护适用仓库
    /// </summary>
    [ChildEntity, Serializable]
	[Label("月台维护适用仓库")]
	public partial class DockMaintainWh : DataEntity
	{
		#region 仓库 Warehouse
		/// <summary>
		/// 仓库Id
		/// </summary>
		public static readonly IRefIdProperty WarehouseIdProperty = P<DockMaintainWh>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 仓库Id
		/// </summary>
		public double WarehouseId
		{
			get { return (double)GetRefId(WarehouseIdProperty); }
			set { SetRefId(WarehouseIdProperty, value); }
		}

		/// <summary>
		/// 仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<DockMaintainWh>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 仓库编码 WarehouseCode
		/// <summary>
		/// 仓库编码
		/// </summary>
		[Label("仓库编码")]
		public static readonly Property<string> WarehouseCodeProperty = P<DockMaintainWh>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

		/// <summary>
		/// 仓库编码
		/// </summary>
		public string WarehouseCode
		{
			get { return this.GetProperty(WarehouseCodeProperty); }
		}
		#endregion

		#region 仓库名称 WarehouseName
		/// <summary>
		/// 仓库名称
		/// </summary>
		[Label("仓库名称")]
		public static readonly Property<string> WarehouseNameProperty = P<DockMaintainWh>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

		/// <summary>
		/// 仓库名称
		/// </summary>
		public string WarehouseName
		{
			get { return this.GetProperty(WarehouseNameProperty); }
		}
		#endregion

		#region 状态 WarehouseState
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<State> WarehouseStateProperty = P<DockMaintainWh>.RegisterView(e => e.WarehouseState, p => p.Warehouse.State);

		/// <summary>
		/// 状态
		/// </summary>
		public State WarehouseState
		{
			get { return this.GetProperty(WarehouseStateProperty); }
		}
		#endregion

		#region 月台维护 DockMaintain
		/// <summary>
		/// 月台维护Id
		/// </summary>
		public static readonly IRefIdProperty DockMaintainIdProperty = P<DockMaintainWh>.RegisterRefId(e => e.DockMaintainId, ReferenceType.Parent);

		/// <summary>
		/// 月台维护Id
		/// </summary>
		public double DockMaintainId
		{
			get { return (double)GetRefId(DockMaintainIdProperty); }
			set { SetRefId(DockMaintainIdProperty, value); }
		}

		/// <summary>
		/// 月台维护
		/// </summary>
		public static readonly RefEntityProperty<DockMaintain> DockMaintainProperty = P<DockMaintainWh>.RegisterRef(e => e.DockMaintain, DockMaintainIdProperty);

		/// <summary>
		/// 月台维护
		/// </summary>
		public DockMaintain DockMaintain
		{
			get { return GetRefEntity(DockMaintainProperty); }
			set { SetRefEntity(DockMaintainProperty, value); }
		}
		#endregion
	}

	/// <summary>
	///  实体配置
	/// </summary>
	internal class DockMaintainWhConfig : EntityConfig<DockMaintainWh>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("DOCK_M_Wh").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}