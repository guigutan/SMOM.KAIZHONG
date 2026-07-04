using SIE.Dock.DockRunMts;
using SIE.MetaModel.View;

namespace SIE.Web.Dock.DockRunMts
{
	/// <summary>
	/// 例外时段视图配置
	/// </summary>
	internal class ExcepTimeViewConfig : WebViewConfig<ExcepTime>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
			{
                View.Property(p => p.ExcepType);
                View.Property(p => p.BeginTime).UseDateTimeEditor(p =>
                {
                    p.XType = "ExceptTimeEditor";
                    p.DefaultValue = System.DateTime.Now.Date;
                });
                View.Property(p => p.EndTime).UseDateTimeEditor(p =>
                {
                    p.XType = "ExceptTimeEditor";
                    p.DefaultValue = System.DateTime.Now.Date;
                });
                View.Property(p => p.Remark);
            }
		}
	}
}