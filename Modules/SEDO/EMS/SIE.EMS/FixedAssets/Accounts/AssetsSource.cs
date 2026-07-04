using SIE.ObjectModel;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 资产来源
    /// </summary>
    public enum AssetsSource
	{
		/// <summary>
		/// 投资采购
		/// </summary>
		[Label("投资采购")]
		Investment = 5,
		/// <summary>
		/// 捐赠
		/// </summary>
		[Label("捐赠")]
		donation = 10,
		/// <summary>
		/// 盘盈
		/// </summary>
		[Label("盘盈")]
		InventoryProfit = 15,
		/// <summary>
		/// 自制转入
		/// </summary>
		[Label("自制转入")]
		SelfMadeTransferIn = 20,
		/// <summary>
		/// 调拨转入
		/// </summary>
		[Label("调拨转入")]
		TransferIn = 25
	}
}