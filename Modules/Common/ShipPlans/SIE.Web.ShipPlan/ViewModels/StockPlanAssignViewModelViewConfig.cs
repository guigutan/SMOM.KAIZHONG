using SIE.ShipPlan.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.ShipPlan.ViewModels
{
	/// <summary>
	/// 备料计划/发货计划齐套分析-预分配明细
	/// </summary>
	public class StockPlanAssignViewModelViewConfig: WebViewConfig<StockPlanAssignViewModel>
	{
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			base.ConfigView();
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.ClearCommands();
			View.DisableEditing();
			using (View.OrderProperties())
			{
				View.Property(p => p.WarehouseCode);
				View.Property(p => p.WarehouseName);
				View.Property(p => p.LotCode);
				View.Property(p => p.Qty);
			}
		}
	}
}
