using SIE.Packages.Boxs;
using SIE.Web.Common;

namespace SIE.WPF.Packages.Boxs
{
	/// <summary>
	/// 周转工具型号视图配置
	/// </summary>
	internal class TurnoverBoxModelViewConfig : WebViewConfig<TurnoverBoxModel>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.UseDefaultCommands(); 
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.ToolType).UseCatalogEditor(e =>
			{
				e.CatalogType = TurnoverBox.BoxTypeCatalog;
				e.CatalogReloadData = true;
			}).UseListSetting(e => { e.HelpInfo = "周转工具型号快码“BOX_TYPE”"; });
			View.Property(p => p.Length);
			View.Property(p => p.Width);
			View.Property(p => p.Height);
		}

		/// <summary>
		/// 查询视图
		/// </summary>
        protected override void ConfigQueryView()
        {
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.ToolType).UseCatalogEditor(e => { e.CatalogType = TurnoverBox.BoxTypeCatalog;e.CatalogReloadData = true; });
		}

		/// <summary>
		/// 选择视图
		/// </summary>
        protected override void ConfigSelectionView()
        {
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.ToolType);
			View.Property(p => p.Length);
			View.Property(p => p.Width);
			View.Property(p => p.Height).UseCatalogEditor(e => { e.CatalogType = TurnoverBox.BoxTypeCatalog;e.CatalogReloadData = true; });
		}
    }
}