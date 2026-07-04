using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using System;

namespace SIE.Wpf.MES.WIP.Packings.ViewBehaviors
{
    /// <summary>
    /// 包装关系视图行为
    /// </summary>
    public class NewPackageRelationViewBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加行为
        /// </summary>
        protected override void OnAttach()
        {
            var mainView = View as ListLogicalView;
            if (mainView == null) return;
            mainView.Control.SelectedItemChanged += LoadItemLabelBySelectPackingRelaion;
            //(mainView.Control.View as TreeListView).RowDoubleClick += PackageRelation_RowDoubleClick;
            mainView.Closed += MainView_Closed;
        }

        /// <summary>
        /// 包装关系行选中事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void LoadItemLabelBySelectPackingRelaion(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.NewItem == null) return;
            var packingViewData = View.Parent?.Current as NewPackingViewModel;
            if (packingViewData == null) return;
            var packingRelation = e.NewItem as PackingRelation;
            var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelByRelationId(packingRelation.Id, new EagerLoadOptions().LoadWithViewProperty());
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
            //(mainView.Control.View as TreeListView).RowDoubleClick -= PackageRelation_RowDoubleClick;
        }
    }
}
