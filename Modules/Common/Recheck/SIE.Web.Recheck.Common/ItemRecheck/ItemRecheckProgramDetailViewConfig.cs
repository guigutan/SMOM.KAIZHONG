using SIE.MetaModel.View;
using SIE.Recheck.Common.ItemRecheck;
using SIE.Web.Recheck.Common.ItemRecheck.Commands;

namespace SIE.Web.Recheck.Common.ItemRecheck
{
    /// <summary>
    /// IQC视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ItemRecheckProgramDetailViewConfig : WebViewConfig<ItemRecheckProgramDetail>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands(ItemRecheckCommands.RecheckProgramDetailAddCommand, WebCommandNames.Edit, WebCommandNames.Delete,
                ItemRecheckCommands.RecheckProgramDetailMoveUp,
                ItemRecheckCommands.RecheckProgramDetailMoveDown,
                ItemRecheckCommands.RecheckProgramDetailMoveTop,
                ItemRecheckCommands.RecheckProgramDetailMoveBottom,
                WebCommandNames.ExportXls);
            View.Property(p => p.RecheckSort).HasLabel("复检次序*").Readonly();
            View.Property(p => p.LimitDays).UseSpinEditor(p => { p.MinValue = 1; p.AllowDecimals = false; }).HasLabel("复检延长保质期(天)*");
        }
    }
}
