using DevExpress.Xpf.Grid;
using SIE.MES.WIP.PackRecombine;
using SIE.Packages;
using SIE.Wpf.Controls;
using System;

namespace SIE.Wpf.MES.WIP.PackRecombine.ViewBehaviors
{
    /// <summary>
    /// 包装管理视图行为
    /// </summary>
    public class PackRecombineViewBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加行为
        /// </summary>
        protected override void OnAttach()
        {
            var mainView = View as ListLogicalView;
            if (mainView == null) return;
            mainView.Control.SelectedItemChanged += LoadItemLabelBySelectPackingRelaion;
            mainView.Closed += MainView_Closed;
        }

        /// <summary>
        /// 包装关系行选中事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void LoadItemLabelBySelectPackingRelaion(object sender, SelectedItemChangedEventArgs e)
        {
            var control = sender as PagingGridControl;
            var packingRelation = control.CurrentItem as PackingRelation;
            if (packingRelation == null) return;
            var packingViewData = View.Relations.Find("mainView")?.Current as PackRecombineViewModel;
            if (packingViewData == null) return;
            var labels = RT.Service.Resolve<PackRecombineController>().GetItemLabels(packingRelation.Id);
            packingViewData.ItemLabelList.Clear();
            packingViewData.ItemLabelList.AddRange(labels);
            packingViewData.ItemLabelList.MarkSaved();
        }

        /// <summary>
        /// 主视图关闭事件，取消事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void MainView_Closed(object sender, EventArgs e)
        {
            var mainView = View as ListLogicalView;
            if (mainView == null) return;
            mainView.Control.SelectedItemChanged -= LoadItemLabelBySelectPackingRelaion;
        }
    }
}
