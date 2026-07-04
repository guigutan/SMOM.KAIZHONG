using SIE.Inventory.Piles;
using SIE.MetaModel.View;

namespace SIE.WPF.Inventory.Piles
{
    /// <summary>
    /// 垛日志视图配置
    /// </summary>
    internal class PileLogViewConfig : WebViewConfig<PileLog>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.PileOpType);
            View.Property(p => p.PileState);
            View.Property(p => p.PileCode);
            View.Property(p => p.CurLocation);
            View.Property(p => p.BillNo);
            View.Property(p => p.BusinessType);
            View.Property(p => p.Weight);
            View.Property(p => p.Length);
            View.Property(p => p.Width);
            View.Property(p => p.Height);
            View.Property(p => p.ItemState);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.PileOpType).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.PileState).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.CurLocation);
            View.Property(p => p.BillNo);
            View.Property(p => p.BusinessType).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.ItemState).UseEnumEditor(p => p.AllowBlank = true);
        }
    }
}