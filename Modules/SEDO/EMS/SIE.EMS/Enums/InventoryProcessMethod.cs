using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 盘点处理方式
    /// </summary>
    public enum InventoryProcessMethod
	{
		/// <summary>
		/// 不变化
		/// </summary>
		[Label("不变化")]
		Unchange = 10,
		/// <summary>
		/// 手动处理
		/// </summary>
		[Label("手动处理")]
		Manually = 20,
		/// <summary>
		/// 待报废
		/// </summary>
		[Label("待报废")]
		ToBeScrap = 30,
		/// <summary>
		/// 待闲置
		/// </summary>
		[Label("待闲置")]
		Idle = 40,
		/// <summary>
		/// 待维修
		/// </summary>
		[Label("待维修")]
		Repair = 50,
		/// <summary>
		/// 待归还
		/// </summary>
		[Label("待归还")]
		Return = 60,
		/// <summary>
		/// 待调拨
		/// </summary>
		[Label("待调拨")]
		Transfer = 70,
		/// <summary>
		/// 新增卡片
		/// </summary>
		[Label("新增卡片")]
		NewCard = 80,
		/// <summary>
		/// 报废
		/// </summary>
		[Label("报废")]
		Scrap = 90,
	}
}