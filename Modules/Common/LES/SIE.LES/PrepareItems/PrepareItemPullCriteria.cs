using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.LES
{
    /// <summary>
    /// 备料模式拉式查询实体
    /// </summary>
    [QueryEntity, Serializable]
	[Label("备料模式拉式查询实体")]
	public partial class PrepareItemPullCriteria : BasePrepareItemCriteria
	{
		#region 最高存量 MaxStock
		/// <summary>
		/// 最高存量
		/// </summary>
		[Label("最高存量")]
		public static readonly Property<decimal?> MaxStockProperty = P<PrepareItemPullCriteria>.Register(e => e.MaxStock);

		/// <summary>
		/// 最高存量
		/// </summary>
		public decimal? MaxStock
		{
			get { return GetProperty(MaxStockProperty); }
			set { SetProperty(MaxStockProperty, value); }
		}
		#endregion

		#region 安全水位 LowestStage
		/// <summary>
		/// 安全水位
		/// </summary>
		[Label("安全水位")]
		[Required]
		public static readonly Property<decimal?> LowestStageProperty = P<PrepareItemPullCriteria>.Register(e => e.LowestStage);

		/// <summary>
		/// 安全水位
		/// </summary>
		public decimal? LowestStage
		{
			get { return GetProperty(LowestStageProperty); }
			set { SetProperty(LowestStageProperty, value); }
		}
		#endregion

		#region 仓库 Warehouse
		/// <summary>
		/// 仓库Id
		/// </summary>
		[Label("备料接收仓库")]
		[Required]
		public static readonly IRefIdProperty WarehouseIdProperty = P<PrepareItemPullCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 仓库Id
		/// </summary>
		public double? WarehouseId
		{
			get { return (double?)GetRefNullableId(WarehouseIdProperty); }
			set { SetRefNullableId(WarehouseIdProperty, value); }
		}

		/// <summary>
		/// 仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<PrepareItemPullCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		/// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<PrepareItemController>().GetPrepareItemPulls(this);
		}
	}
}