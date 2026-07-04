using SIE.SO.SaleOrders;

namespace SIE.Web.SO.SaleOrders
{
	/// <summary>
	/// 销售订单日志视图
	/// </summary>
    internal class SaleOrderLogViewConfig : WebViewConfig<SaleOrderLog>
    {
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{

		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			using (View.OrderProperties())
			{
				View.Property(p => p.LineNo);
				View.Property(p => p.UpdateItem);
				View.Property(p => p.ModifyBefore);
				View.Property(p => p.ModifyAfter);
				View.Property(p => p.ItemCode);
				View.Property(p => p.ItemName);
			}
		}

		///<summary>
		/// 配置明细视图
		/// </summary>
		protected override void ConfigDetailsView()
		{
		}

		///<summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView()
		{
		}
	}
}
