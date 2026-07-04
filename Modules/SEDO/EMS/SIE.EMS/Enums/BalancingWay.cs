using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 平账方式
	/// </summary>
	public enum BalancingWay
	{
		/// <summary>
		/// 不变化
		/// </summary>
		[Label("不调整")]
		Unchange = 10,


		/// <summary>
		/// 库存调整
		/// </summary>
		[Label("库存调整")]
		StockChange = 20,

		/// <summary>
		/// 报废
		/// </summary>
		[Label("报废")]
		Scrap =30,

		/// <summary>
		/// 盘盈入库
		/// </summary>
		[Label("盘盈入库")]
		ProfitInStock =40
	}
}
