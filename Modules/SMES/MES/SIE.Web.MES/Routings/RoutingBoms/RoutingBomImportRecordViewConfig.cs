using SIE.Items;
using SIE.Domain;
using System.Collections.Generic;
using SIE.Web.MES.Routings.RoutingBoms.Commands;
using SIE.MES.Routings.RoutingBoms.ImportBoms;

namespace SIE.Web.Tech.Routings
{
    class RoutingBomImportRecordViewConfig : WebViewConfig<RoutingBomImportRecord>
	{
		/// <summary>
		/// 工序Bom视图配置
		/// </summary>
		public const string RoutingBomDetailView = "RoutingBomDetailImportRecordView";


		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseCommands(typeof(DownloadImportFileCommand).FullName, "SIE.Web.MES.Routings.RoutingBoms.Commands.ShowRoutingBomDetailCommand");
			View.Property(p => p.OperatorName).Readonly();
			View.Property(p => p.ImportDate).HasLabel("导入时间").Readonly();
			View.Property(p => p.ProductCode).ShowInList(200).Readonly();
			View.Property(p => p.RoutingName).Readonly();
			View.Property(p => p.VersionName).ShowInList(200).HasLabel("版本").Readonly();
		}

	}
}
