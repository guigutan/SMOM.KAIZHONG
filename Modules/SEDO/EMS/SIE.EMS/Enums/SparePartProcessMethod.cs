using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 备件盘点平账方式
	/// </summary>
	public enum SparePartProcessMethod
	{
		/// <summary>
		/// 不调整
		/// </summary>
		[Label("不调整")]
		Unchanged = 10,
		/// <summary>
		/// 库存调整
		/// </summary>
		[Label("库存调整")]
		Adjust = 20,
		/// <summary>
		/// 报废
		/// </summary>
		[Label("报废")]
		Scrap = 30,
		/// <summary>
		/// 盘盈入库
		/// </summary>
		[Label("盘盈入库")]
		ProfitStockIn = 40,
	}
}