using DevExpress.Xpf.Grid;
using SIE.Wpf.Controls;

namespace SIE.Wpf.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 报检条码视图行为
    /// </summary>
    public class FirstInspViewBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加视图行为
        /// </summary>
        protected override void OnAttach()
        {
            var gridControl = View.Control as PagingGridControl;
            var tableView = gridControl.View as TableView;
            gridControl.SelectionMode = MultiSelectMode.MultipleRow;
            tableView.ShowCheckBoxSelectorColumn = true;
        }
    }
}