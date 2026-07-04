using DevExpress.Xpf.Grid;
using SIE.Tech.Processs;
using System;
using System.Linq;

namespace SIE.Wpf.Tech.Processs.Behaviors
{
    /// <summary>
    /// 工序列表视图行为
    /// </summary>
    public class ProcessBehavior : ViewBehavior
    {
        /// <summary>
        /// 批次工序需显示采集步骤列
        /// </summary>
        System.Collections.Generic.List<GridColumn> _column;

        /// <summary>
        /// 采集步骤是否解绑列，批次工序需隐藏
        /// </summary>
        GridColumn _unboundColumn;

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
            var process = View.Current as Process;
            if (process != null)
            {
                process.PropertyChanged -= Process_PropertyChanged;
                process.PropertyChanged += Process_PropertyChanged;
            }

            SetColumnVisible(process);
            ActiveView();
        }

        /// <summary>
        /// 激活视图
        /// </summary>
        private void ActiveView()
        {
            var activeView = View.ChildrenViews.FirstOrDefault(p => p.IsActive);
            if (activeView != null)
                activeView.IsActive = true;
        }

        /// <summary>
        /// 工序属性变更事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        void Process_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var process = sender as Process;
            SetColumnVisible(process);
        }

        /// <summary>
        /// 设置列是否可见
        /// </summary>
        /// <param name="process">工序</param>
        void SetColumnVisible(Process process)
        {
            CRT.MainThread.InvokeAsync(() =>
            {
                if (_column == null)
                    GetPlugTypeColumn();
                if (_column == null)
                    return;
                if (process == null || process.Type == ProcessType.Assembly || process.Type == ProcessType.Fix /*|| process.Type == ProcessType.Fqc*/ || process.Type == ProcessType.Packing || process.Type == ProcessType.Pqc || process.Type == ProcessType.Rework)
                {
                    _column.ForEach(p => p.Visible = false);
                    SetUnboundColumnVisible(true);
                }
                else
                {
                    _column.ForEach(p => p.Visible = true);
                    SetUnboundColumnVisible(false);
                }
                int index = 0;
                (_unboundColumn.Parent as GridControl)?.Columns.ForEach(e => e.VisibleIndex = ++index);
            });
        }

        /// <summary>
        /// 设置采集步骤是否解绑列是否可见
        /// </summary>
        /// <param name="isVisible">是否可见</param>
        void SetUnboundColumnVisible(bool isVisible)
        {
            if (_unboundColumn != null)
                _unboundColumn.Visible = isVisible;
        }

        /// <summary>
        /// 获取出入类型列
        /// </summary>
        void GetPlugTypeColumn()
        {
            var stepView = View.ChildrenViews.FirstOrDefault(p => p.EntityType == typeof(ProcessCollectStep)) as ListLogicalView;
            if (stepView == null) return;
            var control = stepView.Control as GridControl;
            _column = control.Columns.Where(p => p.FieldName == nameof(ProcessCollectStep.IsGenerateBatch) || p.FieldName == nameof(ProcessCollectStep.PlugType)).ToList();
            _unboundColumn = control.Columns.FirstOrDefault(p => p.FieldName == nameof(ProcessCollectStep.IsUnbound));
        }
    }
}