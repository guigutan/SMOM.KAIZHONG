using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Resources.IconPacks;
using SIE.Domain;
using SIE.MES.WorkOrders.Reworks;
using SIE.Wpf.Command;
using SIE.Wpf.Controls;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关键件行为
    /// </summary>
    public class KeyItemUnboundViewBehavior : ViewBehavior
    {
        /// <summary>
        /// tableView
        /// </summary>
        private TableView tv;

        /// <summary>
        /// gridControl
        /// </summary>
        private PagingGridControl gridControl;

        /// <summary>
        /// 加载完成标记
        /// </summary>
        bool _isLoaded = false;

        /// <summary>
        /// 附加行为
        /// </summary>
        protected override void OnAttach()
        {
            gridControl = View.Control as PagingGridControl;
            tv = gridControl.View as TableView;
            var view = View as ListLogicalView;
            View.Control.Loaded += (o, e) =>
            {
                if (_isLoaded) return;
                var commnad = View.Commands.FirstOrDefault(p => p.GetType() == typeof(ListEditCommand));
                if (commnad != null)
                    commnad.IsVisible = false;
                view.CurrentChanged -= View_CurrentChanged;
                view.CurrentChanged += View_CurrentChanged;
                var vm = View.Relations.Find("mainView")?.Current as WorkOrderUnionBarcode;
                if (vm != null)
                    vm.KeyItemList.CollectionChanged += KeyItemList_CollectionChanged;
                SetKeyItemSelectPickIcon();
                _isLoaded = true;
            };
        }

        /// <summary>
        /// 关键件解绑配置集合变更事件
        /// </summary>
        /// <param name="sender">GridControl</param>
        /// <param name="e">参数</param>
        private void KeyItemList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetKeyItemSelectPickIcon();
        }

        /// <summary>
        /// 选中变更事件
        /// </summary>
        /// <param name="sender">ListLogicalView</param>
        /// <param name="e">参数</param>
        private void View_CurrentChanged(object sender, System.EventArgs e)
        {
            var config = (sender as ListLogicalView)?.Current as KeyItemUnboundConfig;
            if (config == null) return;
            config.PropertyChanged -= ConfigPropertyChanged;
            config.PropertyChanged += ConfigPropertyChanged;
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="sender">KeyItemUnboundConfig</param>
        /// <param name="e">参数</param>
        private void ConfigPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == KeyItemUnboundConfig.IsUnboundProperty.Name)
                SetKeyItemSelectPickIcon();
        }

        /// <summary>
        /// 设置命令图标
        /// </summary>
        void SetKeyItemSelectPickIcon()
        {
            var vm = View.Relations.Find("mainView")?.Current as WorkOrderUnionBarcode;
            if (vm == null || vm.IsFromCmd) return;
            ////未关联条码数量 
            var isAllselect = vm.KeyItemList.Count > 0 && vm.KeyItemList.All(p => p.IsUnbound);
            SetPackIcon(isAllselect);
            vm.SelectKeyItems = isAllselect;
        }

        /// <summary>
        /// 命令图标控件
        /// </summary>
        PackIcon packIcon;

        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="isAllselect">是否全选</param>
        void SetPackIcon(bool isAllselect)
        {
            if (packIcon == null)
                GetCmdPackIcon();
            if (packIcon == null)
                return;
            packIcon.Kind = isAllselect ? PackIconKind.Checkmark : PackIconKind.CheckmarkUncrossed;
        }

        /// <summary>
        /// 获取全选命令图标控件
        /// </summary>
        private void GetCmdPackIcon()
        {
            foreach (var item in View.CommandsContainer.Items)
            {
                var cmd = item as SimpleButton;
                if (cmd == null || (cmd.Command as UICommand)?.ClientCommand?.GetType() != typeof(KeyItemAllSelectCommand))
                    continue;
                packIcon = (cmd.Content as CommandContentControl)?.Icon as PackIcon;
                break;
            }
        }
    }
}