using SIE.MetaModel.View;
using SIE.Tech.Processs;

namespace SIE.Web.Tech.Processs
{
    /// <summary>
    /// 工序参数视图配置
    /// </summary>
    class ProcessParameterViewConfig : WebViewConfig<ProcessParameter>
    {
        /// <summary>
        /// 工艺路线维护工序的工序参数视图
        /// </summary>
        public const string ProcessParameterView = "ProcessParameterView";

        /// <summary>
        /// 配置默认视图 
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ProcessParameterView);
            if (ViewGroup == ProcessParameterView)
                ConfigProcessParameterView();
        }

        /// <summary>
        /// 配置工序参数视图（工艺连续维护工序使用）
        /// </summary>
        private void ConfigProcessParameterView()
        {
            View.UseCommands("SIE.Web.Tech.Processs.Commands.AddParameterCommand", "SIE.Web.Tech.Processs.Commands.EditParameterCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Type).HasLabel("结果").UseEnumEditor(p => { p.XType = "StepBarcodeTypeEditorRouting"; }).ShowInList().HasOrderNo(1);
                View.Property(p => p.Script).UseMemoEditor().Readonly(p => p.Type != ResultTypeForDesign.Custom).ShowInList().HasOrderNo(2)
            .UseListSetting(e => { e.HelpInfo = "采集结果自定义可编辑"; });
                View.Property(p => p.Description).HasLabel("结果描述").ShowInList().HasOrderNo(3);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands("SIE.Web.Tech.Processs.Commands.AddParameterCommand", "SIE.Web.Tech.Processs.Commands.EditParameterCommand", WebCommandNames.Delete, WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.Type).HasLabel("结果").UseEnumEditor(p => { p.XType = "StepBarcodeTypeEditor"; }).ShowInList().HasOrderNo(1);
                View.Property(p => p.Script).UseProcessConditionEditor().Readonly(p => p.Type != ResultTypeForDesign.Custom).ShowInList().HasOrderNo(2)
            .UseListSetting(e => { e.HelpInfo = "采集结果自定义可编辑"; });
                View.Property(p => p.Description).HasLabel("结果描述").ShowInList().HasOrderNo(3);
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Type).HasLabel("结果");
            View.Property(p => p.Script).UseMemoEditor().Readonly(p => p.Type != ResultTypeForDesign.Custom)
            .UseListSetting(e => { e.HelpInfo = "采集结果自定义可编辑"; });
            View.Property(p => p.Description).HasLabel("结果描述");
        }
    }
}