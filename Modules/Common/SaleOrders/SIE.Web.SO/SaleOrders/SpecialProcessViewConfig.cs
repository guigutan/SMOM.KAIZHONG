using SIE.MetaModel.View;
using SIE.SO.SaleOrders;

namespace SIE.Web.SO.SaleOrders
{
    /// <summary>
    /// 特殊工艺视图配置
    /// </summary>
    internal class SpecialProcessViewConfig : WebViewConfig<SpecialProcess>
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
			View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete);
			//View.UseCommands("SIE.Web.SO.SaleOrders.Commands.SpecialProcessSaveCommand");
			using (View.OrderProperties())
			{
				View.Property(p => p.Process);
				View.Property(p => p.Value).UseSpinEditor(p =>
				{
					p.MinValue = 0;
					p.AllowDecimals = true;
				});
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
		}
	}
}