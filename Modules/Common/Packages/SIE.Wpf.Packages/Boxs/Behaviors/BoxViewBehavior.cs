using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using SIE.Common.Configs;
using SIE.Domain.Validation;
using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using System;
using System.Linq;
using System.Windows;

namespace SIE.Wpf.Packages.Boxs.Behaviors
{
    /// <summary>
    /// 周转箱视图行为
    /// </summary>
    public class BoxViewBehavior : ViewBehavior
    {
        /// <summary>
        /// 默认容量列表
        /// </summary>
        GridColumn _capacityColumn;

        /// <summary>
        /// 生产周转箱类型
        /// </summary>
        string _boxType;

        /// <summary>
        /// 附加视图行为
        /// </summary>
        protected override void OnAttach()
        {
            View.CurrentChanged += MainView_CurrentChanged;
        }

        /// <summary>
        /// 当前实体变更事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void MainView_CurrentChanged(object sender, EventArgs e)
        {
            var box = View.Current as TurnoverBox;
            if (box == null)
                return;
            SetColumnVisible(box);
        }

        /// <summary>
        /// 设置列是否可见
        /// </summary>
        /// <param name="box">周转箱</param>
        private void SetColumnVisible(TurnoverBox box)
        {
            CRT.MainThread.InvokeAsync(() =>
            {
                try
                {
                    if (_capacityColumn == null)
                        GetCapacityColumn();
                    if (_capacityColumn == null)
                        return;
                    bool visible = IsVisible(box);
                    if (_capacityColumn != null)
                        _capacityColumn.Visible = visible;
                    SetChildPanelVisible(visible);
                }
                catch (Exception exc)
                {
                    exc.Alert();
                }
            });
        }

        /// <summary>
        /// 设置子列表是否可见
        /// </summary>
        /// <param name="visible">是否可见</param>
        private void SetChildPanelVisible(bool visible)
        {
            var tapControl = View.LayoutControl.GetLogicalChild<DXTabControl>();
            if (tapControl == null) return;
            var childPanel = tapControl.GetLogicalParent<LayoutPanel>();
            if (childPanel == null) return;
            childPanel.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 判断默认容量类，产品容量视图是否可见
        /// 生产周转箱时都可见，其他情况不可见
        /// </summary>
        /// <param name="box">周转箱</param>
        /// <returns>可见返回true，否则返回false</returns>
        private bool IsVisible(TurnoverBox box)
        {
            if (_boxType.IsNullOrEmpty())
            {
                var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
                if (config == null || config.BoxType.IsNullOrEmpty())
                    throw new ValidationException("生产周转箱类型未配置，请在全局配置中配置并重新打开界面".L10N());
                _boxType = config.BoxType;
            }
            return _boxType == box.Type;
        }

        /// <summary>
        /// 获取默认容量列
        /// </summary>
        void GetCapacityColumn()
        {
            var control = View.Control as GridControl;
            _capacityColumn = control.Columns.FirstOrDefault(p => p.FieldName == nameof(TurnoverBox.Capacity));
        }
    }
}