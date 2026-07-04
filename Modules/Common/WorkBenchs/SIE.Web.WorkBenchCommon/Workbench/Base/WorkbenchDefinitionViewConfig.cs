using SIE.MetaModel.View;
using SIE.WorkBenchCommon.Workbench.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.WorkBenchCommon.Workbench.Base
{
	/// <summary>
	/// 工作台定义视图配置
	/// </summary>
	internal class WorkbenchDefinitionViewConfig : WebViewConfig<WorkbenchDefinition>
	{
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.HasDelegate(WorkbenchDefinition.NameProperty);
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseDefaultCommands();
			View.UseImportCommands();
			View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.WorkBenchCommon.Workbench.Base.Commands.AddWorkbenchCommand");
			//View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.WorkBenchCommon.Workbench.Base.Commands.EditWorkbenchCommand");
			View.UseCommands("SIE.Web.WorkBenchCommon.Workbench.Base.Commands.DesignWorkbenchCommand");
			View.UseCommands("SIE.Web.WorkBenchCommon.Workbench.Base.Commands.WorkbenchPreviewCommand");
			View.UseCommands("SIE.Web.WorkBenchCommon.Workbench.Base.Commands.PublishWorkbenchCommand", "SIE.Web.WorkBenchCommon.Workbench.Base.Commands.UnPublishWorkbenchCommand");
			View.Property(p => p.Code).Readonly("p=> p.isNew()==false");
			View.Property(p => p.Name);
			View.Property(p => p.Description);
			View.Property(p => p.LayoutCode).Readonly(true);
			View.Property(p => p.ComponentContent).Readonly(true).DisableSort();
		}

		///<summary>
		/// 配置明细视图
		/// </summary>
		protected override void ConfigDetailsView()
		{
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.Description);
		}

		protected override void ConfigQueryView()
		{
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.Description);
		}

		///<summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView()
		{
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.Description);
		}

		protected override void ConfigImportView()
		{
			View.Property(p => p.Code).ImportIndexer();
			View.Property(p => p.Name);
			View.Property(p => p.Description);
			View.Property(p => p.LayoutCode);
			View.Property(p => p.ComponentContent);
		}
	}

}
