using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 卡片来源
    /// </summary>
    public enum EquipmentCardSource
	{
		/// <summary>
		/// 采购接收
		/// </summary>
		[Label("采购接收")]
		EquipmentReceive = 10,
		/// <summary>
		/// 赠品接收
		/// </summary>
		[Label("赠品接收")]
		Giveaway = 20,
		/// <summary>
		/// 客供接收
		/// </summary>
		[Label("客供接收")]
		Customer = 30,
		/// <summary>
		/// 租赁接收
		/// </summary>
		[Label("租赁接收")]
		Lease = 40,
		/// <summary>
		/// 其他接收
		/// </summary>
		[Label("其他接收")]
		Other = 50,
		/// <summary>
		/// 盘盈
		/// </summary>
		[Label("盘盈")]
		Surplus = 60,
		/// <summary>
		/// 手动创建
		/// </summary>
		[Label("手动创建")]
		Manual = 70,
	}
}