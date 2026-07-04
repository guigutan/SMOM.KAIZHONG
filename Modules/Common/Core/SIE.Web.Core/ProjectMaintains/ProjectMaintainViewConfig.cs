using SIE.Core.ProjectMaintains;
using SIE.MetaModel.View;
using SIE.Web.Core.ProjectMaintains.Commands;

namespace SIE.Web.Core.ProjectMaintains
{
    /// <summary>
    /// 项目维护视图配置
    /// </summary>
    internal class ProjectMaintainViewConfig : WebViewConfig<ProjectMaintain>
	{
		/// <summary>
		/// 只读视图
		/// </summary>
		public const string readOnlyView = "ReadOnlyView";

		/// <summary>
		/// 视图配置
		/// </summary>
		protected override void ConfigView()
		{
			if (ViewGroup == readOnlyView)
			{
				ReadOnlyConfigView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, typeof(DeleteProjectMaintainCommand).FullName, WebCommandNames.Save);
			View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.Desc);
			View.Property(p => p.State).Readonly();
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected void ReadOnlyConfigView()
		{			 
			View.Property(p => p.Code).ShowInList().Readonly();
			View.Property(p => p.Name).ShowInList().Readonly();
			View.Property(p => p.Desc).ShowInList().Readonly();
			View.Property(p => p.State).ShowInList().Readonly();
		}

		/// <summary>
		/// 配置查询视图
		/// </summary>
		protected override void ConfigQueryView()
		{
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.State).UseEnumEditor(p => p.AllowBlank = true);
		}

		/// <summary>
		/// 下拉选择视图
		/// </summary>
        protected override void ConfigSelectionView()
        {
			View.Property(p => p.Code).Readonly().Show();
			View.Property(p => p.Name).Readonly().Show();
            View.Property(p => p.Desc).Readonly().Show();
        }
    }
}