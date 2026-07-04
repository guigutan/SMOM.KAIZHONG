using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.MetaModel.View;
using SIE.Web.Core.Common.Commands;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances
{
	/// <summary>
	/// 工治验收项目视图配置
	/// </summary>
	internal class FixtureAcceptanceItemViewConfig : WebViewConfig<FixtureAcceptanceItem>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.UseDefaultCommands();
			View.RemoveCommands(WebCommandNames.Save, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXls);
			View.ReplaceCommands(WebCommandNames.Delete, typeof(ImmediateDeleteCommand).FullName);
			View.Property(p => p.ItemName);
			View.Property(p => p.AcceptanceValue);
			View.Property(p => p.Remark);
			View.Property(p => p.InspectionResult);
		}
	}
}