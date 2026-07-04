using SIE.MetaModel.View;
using SIE.WorkBenchCommon.Workbench.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.WorkBenchCommon.Workbench.Base
{
	/// <summary>
	/// 组件信息视图配置
	/// </summary>
	internal class ComponentInfoViewConfig : WebViewConfig<ComponentInfo>
	{
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.HasDelegate(ComponentInfo.CodeProperty);
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseDefaultCommands();
			View.ReplaceCommands(WebCommandNames.Save, "SIE.Web.WorkBenchCommon.Workbench.Base.Commands.WorkbenchSaveCommand");
			View.UseImportCommands();
			View.UseCommand("SIE.Web.WorkBenchCommon.Workbench.Base.Commands.ComponentPreviewCommand");
			View.Property(p => p.Code);
			View.Property(p => p.Description);
			View.Property(p => p.Content).UseAceCodeFieldEditor(p => {
				p.RunButtonJs = "SIE.Web.WorkBenchCommon.Workbench.Base.Scripts.ComponentRunCommand";
				p.CodeMode = "ace/mode/javascript";
			}).DisableSort(); 
		}


		///<summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView()
		{
			View.Property(p => p.Code);
			View.Property(p => p.Description);
		}

		protected override void ConfigQueryView()
		{
			View.Property(p => p.Code);
			View.Property(p => p.Description);
		}

		protected override void ConfigImportView()
		{
			View.Property(p => p.Code).ImportIndexer();
			View.Property(p => p.Description);
			View.Property(p => p.Content);
		}
	}

}
