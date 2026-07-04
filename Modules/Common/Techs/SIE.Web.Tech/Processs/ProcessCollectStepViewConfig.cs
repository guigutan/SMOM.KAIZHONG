using SIE.MetaModel.View;
using SIE.Tech.Processs;

namespace SIE.Web.Tech.Processs
{
    /// <summary>
    /// 工序采集步骤视图配置
    /// </summary>
    class ProcessCollectStepViewConfig : WebViewConfig<ProcessCollectStep>
    {
        /// <summary>
        /// 工艺路线维护工序的采集步骤视图
        /// </summary>
        public const string ProcessCollectStepView = "ProcessCollectStepView";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ProcessCollectStepView);
            if (ViewGroup == ProcessCollectStepView)
                ConfigProcessCollectStepView();
        }

        /// <summary>
        /// 配置工艺路线维护工序的采集步骤视图
        /// </summary>
        private void ConfigProcessCollectStepView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.Tech.Processs.Commands.ProcessCollectStepAddCommand", WebCommandNames.Edit, WebCommandNames.Delete);
            View.UseCommands("SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveTop", "SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveUp", "SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveDown", "SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveBottom");
            using (View.OrderProperties())
            {
                View.Property(p => p.BarcodeType).UseEnumEditor(p => { p.XType = "StepBarcodeTypeEditorRouting"; }).Show();
                View.Property(p => p.PlugType).HasLabel("出入类型").Show();
                View.Property(p => p.IsGenerateBatch).HasLabel("是否生成批次").Readonly(p => p.PlugType != PlugType.Out).Show(ShowInWhere.Hide)
                .UseListSetting(e => { e.HelpInfo = "入站不可编辑"; });
                View.Property(p => p.IsUnbound).UseCheckEditor().HasLabel("是否解绑").Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.UseCommands("SIE.Web.Tech.Processs.Commands.ProcessCollectStepAddCommand", WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.ExportXls);
                View.UseCommands("SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveTop", "SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveUp", "SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveDown", "SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveBottom");
                View.Property(p => p.BarcodeType).UseEnumEditor(p => { p.XType = "StepBarcodeTypeEditor"; });
                View.Property(p => p.PlugType).HasLabel("出入类型");
                View.Property(p => p.IsGenerateBatch).HasLabel("是否生成批次").Readonly(p => p.PlugType != PlugType.Out).Show(ShowInWhere.Hide)
                .UseListSetting(e => { e.HelpInfo = "出站不可编辑"; });
                View.Property(p => p.IsUnbound).UseCheckEditor().HasLabel("是否解绑");
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.BarcodeType).UseEnumEditor(p => p.AllowBlank = false);
            View.Property(p => p.PlugType).HasLabel("出入类型");
            View.Property(p => p.IsGenerateBatch).HasLabel("是否生成批次").Show(ShowInWhere.Hide);
            View.Property(p => p.IsUnbound).UseCheckEditor().HasLabel("是否解绑");
        }
    }
}