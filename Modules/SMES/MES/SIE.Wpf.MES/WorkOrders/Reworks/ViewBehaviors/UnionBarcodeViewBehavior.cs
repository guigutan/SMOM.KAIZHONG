using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Resources.IconPacks;
using SIE.MES.WorkOrders.Reworks;
using SIE.Wpf.Command;
using SIE.Wpf.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 显示行选择复选框
    /// </summary>
    public class UnionBarcodeViewBehavior : ViewBehavior
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
        /// 控件是否已加载
        /// </summary>
        bool isLoaded = false;

        /// <summary>
        /// 初始化方法
        /// </summary>
        protected override void OnAttach()
        {
            gridControl = View.Control as PagingGridControl;
            tv = gridControl.View as TableView;
            gridControl.SelectionMode = MultiSelectMode.MultipleRow;
            tv.ShowCheckBoxSelectorColumn = true;
            tv.CheckBoxSelectorColumnHeaderTemplate = new DataTemplate();
            tv.PreviewMouseDown += OnViewPreviewMouseDown;
            gridControl.SelectionChanged += GridControl_SelectionChanged;
            View.Control.Loaded += (s, e) =>
            {
                if (isLoaded) return;
                //// 视图关闭时，取消注册事件
                var mainView = View.Relations.Find("mainView") as DetailLogicalView;
                if (mainView != null)
                {
                    mainView.Closed += (x, y) =>
                    {
                        gridControl.SelectionChanged -= GridControl_SelectionChanged;
                        tv.PreviewMouseDown -= OnViewPreviewMouseDown;
                    };
                    var vm = mainView.Current as WorkOrderUnionBarcode;
                    if (vm == null) return;
                    vm.BarcodeList.CollectionChanged -= BarcodeList_CollectionChanged;
                    vm.BarcodeList.CollectionChanged += BarcodeList_CollectionChanged;
                }

                isLoaded = true;
            };
        }

        /// <summary>
        /// 关键件条码关联集合变更事件
        /// </summary>
        /// <param name="sender">GridControl</param>
        /// <param name="e">参数</param>
        private void BarcodeList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetUnionBarcodeSelectPickIcon();
        }

        /// <summary>
        /// 选中勾选框改变事件
        /// </summary>
        /// <param name="sender">s</param>
        /// <param name="e">e</param>
        private void GridControl_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var current = View.Current as UnionBarcode;
            if (current != null && current.CodeState == CodeState.Associated)
            {
                e.Handled = true;
                (View as ListLogicalView)?.UnSelectEntities(current);
                return;
            }

            SetUnionBarcodeSelectPickIcon();
        }

        /// <summary>
        /// 设置命令图标
        /// </summary>
        private void SetUnionBarcodeSelectPickIcon()
        {
            var vm = View.Relations.Find("mainView")?.Current as WorkOrderUnionBarcode;
            if (vm == null || vm.IsFromCmd) return;
            ////未关联条码数量 
            var notAssociatedCount = vm.BarcodeList.Where(p => p.CodeState == CodeState.NotAssociated).Count();
            ////选中未关联条码数据
            var selectedCount = (View as ListLogicalView).SelectedEntities.Count;
            bool isAllSelect = selectedCount == notAssociatedCount && notAssociatedCount != 0;
            SetPackIcon(isAllSelect);
            vm.SelectBarcodes = isAllSelect;
        }

        /// <summary>
        /// 根据选择行查找里面的控件元素
        /// </summary>
        /// <typeparam name="childitem">c</typeparam>
        /// <param name="obj">o</param>
        /// <returns>复选框</returns>
        public CellsControl FindVisualChild<childitem>(DependencyObject obj) where childitem : FrameworkElement
        {
            if (obj == null)
                return null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child.GetType().Name == "CellsControl")
                {
                    var s = child as CellsControl;
                    return s;
                }
                else
                {
                    var childOfChild = FindVisualChild<childitem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }

        /// <summary>
        /// header不显示全选
        /// </summary>
        /// <param name="sender">s</param>
        /// <param name="e">e</param>
        protected void OnViewPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var hi = tv.CalcHitInfo((DependencyObject)e.OriginalSource);

            if (hi.InColumnHeader && hi.Column.FieldName == "DX$CheckboxSelectorColumn")
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 命令图标控件
        /// </summary>
        PackIcon packIcon;

        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="isAllSelect">是否全选</param>
        void SetPackIcon(bool isAllSelect)
        {
            if (packIcon == null)
                GetCmdPackIcon();
            if (packIcon == null)
                return;
            packIcon.Kind = isAllSelect ? PackIconKind.Checkmark : PackIconKind.CheckmarkUncrossed;
        }

        /// <summary>
        /// 获取全选命令图标控件
        /// </summary>
        private void GetCmdPackIcon()
        {
            foreach (var item in View.CommandsContainer.Items)
            {
                var cmd = item as SimpleButton;
                if (cmd == null || (cmd.Command as UICommand)?.ClientCommand?.GetType() != typeof(UnionBarcodeAllSelectCommand))
                    continue;
                packIcon = (cmd.Content as CommandContentControl)?.Icon as PackIcon;
                break;
            }
        }
    }
}