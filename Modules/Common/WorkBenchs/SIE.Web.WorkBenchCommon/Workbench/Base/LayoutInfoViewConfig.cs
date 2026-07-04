using SIE.WorkBenchCommon.Workbench.Base;
using System;
using SIE.Web;
using SIE.Web.WorkBenchCommon.Workbench.Base.Commands;

namespace SIE.Web.WorkBenchCommon.Workbench.Base
{
    /// <summary>
    /// 布局视图配置
    /// </summary>
    internal class LayoutInfoViewConfig : WebViewConfig<LayoutInfo>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(LayoutInfo.CodeProperty);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(ImportLayoutInfoCommand).FullName);
            View.UseCommand("SIE.Web.WorkBenchCommon.Workbench.Base.Commands.LayoutPreviewCommand");
            View.Property(p => p.Code);
            View.Property(p => p.Description);
            View.Property(p => p.Content).UseAceCodeFieldEditor(p =>
            {
                p.RunButtonJs = "SIE.Web.WorkBenchCommon.Workbench.Base.Scripts.LayoutRunCommand";
                p.CodeMode = "ace/mode/json";
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
