using DevExpress.Xpf.Grid;
using SIE.Domain;
using System;

namespace SIE.Wpf.MES.PanelBindings.ViewBehaviors
{
    /// <summary>
    /// 拼板码视图行为
    /// </summary>
    public class PanelBindingBehavior : ViewBehavior
    {
        /// <summary>
        /// 控件
        /// </summary>
        GridControl _gridControl;

        /// <summary>
        /// 附加行为
        /// </summary>
        protected override void OnAttach()
        {
            var mainView = View as ListLogicalView;
            if (mainView == null) return;
            _gridControl = mainView.Control as GridControl;
            mainView.Control.SelectedItemChanged += CurrentSelect;
            mainView.Closed += MainView_Closed;
            mainView.DataChanged += (o, e) =>
              {
                  var itemsSource = _gridControl.ItemsSource as EntityList;
                  itemsSource.CollectionChanged += ItemsSource_CollectionChanged;
              };
        }

        /// <summary>
        /// 集合变更事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                _gridControl.SelectedItem = e.NewItems[0];
                _gridControl.CurrentItem = e.NewItems[0];
            }
        }

        /// <summary>
        /// 当前选择变更事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void CurrentSelect(object sender, SelectedItemChangedEventArgs e)
        {
            var vm = (View.Relations[0].View.Current as PanelBindingViewModel);
            var panelViewModel = e.NewItem as PanelViewModel;
            vm.CurrentSelectPanel = panelViewModel?.Panel;
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
            mainView.Control.SelectedItemChanged -= CurrentSelect;
            var itemsSource = _gridControl.ItemsSource as EntityList;
            itemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
        }
    }
}
