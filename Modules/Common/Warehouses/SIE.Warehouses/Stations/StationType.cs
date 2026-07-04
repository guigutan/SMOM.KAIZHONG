using SIE.ObjectModel;

namespace SIE.Warehouses.Stations
{
	/// <summary>
	/// 站台类型
	/// </summary>
	public enum StationType
	{
		/// <summary>
		/// 入库站台
		/// </summary>
		[Label("入库站台")]
		InStation = 1,
		/// <summary>
		/// 出库站台
		/// </summary>
		[Label("出库站台")]
		OutStation = 2,
		/// <summary>
		/// 双向出入站台
		/// </summary>
		[Label("双向出入站台")]
		InAndOutStation = 3,
		/// <summary>
		/// 楼层站台
		/// </summary>
		[Label("楼层站台")]
		FloorStation = 4,
		/// <summary>
		/// 拣选站台
		/// </summary>
		[Label("拣选站台")]
		PickingStation = 5,
		/// <summary>
		/// 盘点站台
		/// </summary>
		[Label("盘点站台")]
		InventoryStation = 6,
		/// <summary>
		/// 关键节点站台
		/// </summary>
		[Label("关键节点站台")]
		KeyNodeStation = 7,
		/// <summary>
		/// 货架站台
		/// </summary>
		[Label("货架站台")]
		ShelfStation = 8,
		/// <summary>
		/// 缓存站台
		/// </summary>
		[Label("缓存站台")]
		CacheStation = 9,
		/// <summary>
		/// 叠盘机站台
		/// </summary>
		[Label("叠盘机站台")]
		PalletizerStation = 10,
	}
}