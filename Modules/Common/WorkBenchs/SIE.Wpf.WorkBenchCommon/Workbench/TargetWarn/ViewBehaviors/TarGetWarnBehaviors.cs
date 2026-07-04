using DevExpress.Xpf.Grid;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace SIE.Wpf.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 切换指标类型时，清除指标名称
    /// </summary>
    public class TargetWarnSettingChangeQuota : ViewBehavior
    {
        /// <summary>
        /// 加载时挂载
        /// </summary>
        protected override void OnAttach()
        {
            View.CurrentChanged += View_CurrentChanged;
        }

        private void View_CurrentChanged(object sender, EventArgs e)
        {
            var vm = View.Current as TargetWarnSetting;
            if (vm != null)
            {
                vm.PropertyChanged -= VM_PropertyChanged;
                vm.PropertyChanged += VM_PropertyChanged;
            }
        }

        /// <summary>
        /// 切换指标类型时，清除指标名称
        /// </summary>
        /// <param name="sender">数据发送</param>
        /// <param name="e">参数</param>
        private void VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var main = sender as TargetWarnSetting;

            if (e.PropertyName == TargetWarnSetting.CodeProperty.Name)
            {
                main.Name = null;
            }
        }
    }

    /// <summary>
    /// 切换指标类型时，清除指标名称
    /// </summary>
    public class TargetWarnSettingQueryChangeQuota : ViewBehavior
    {
        /// <summary>
        /// 加载时挂载
        /// </summary>
        protected override void OnAttach()
        {
            var vm = View.Current as TargetWarnSettingCriteria;
            if (vm != null)
            {
                vm.PropertyChanged -= VM_PropertyChanged;
                vm.PropertyChanged += VM_PropertyChanged;
            }
        }

        /// <summary>
        /// 切换指标类型时，清除指标名称
        /// </summary>
        /// <param name="sender">数据发送</param>
        /// <param name="e">参数</param>
        private void VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var main = sender as TargetWarnSettingCriteria;

            if (e.PropertyName == TargetWarnSettingCriteria.CodeProperty.Name)
            {
                main.Name = null;
            }
        }
    }

    /// <summary>
    /// 根据目标值区间定义颜色
    /// </summary>
    public class TarGetWarnDetailColorBehaviors : ViewBehavior
    {
        /// <summary>
        /// 加载时根据条件定义颜色
        /// </summary>
        protected override void OnAttach()
        {
            if (!(View is ListLogicalView))
                return;
            var listView = View as ListLogicalView;
            var tableView = listView.Control.View as TableView;
            var formatConditions = new FormatCondition();
            formatConditions.FieldName = TargetWarnDetail.TargetColorProperty.Name;
            formatConditions.ValueRule = DevExpress.Xpf.Core.ConditionalFormatting.ConditionRule.Expression;
            formatConditions.Value1 = true;
             formatConditions.Expression = "[{0}] == 1".FormatArgs(TargetWarnDetail.TargetOpetatorsProperty.Name);
            formatConditions.Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format() { Background = Brushes.Yellow };
            tableView.FormatConditions.Add(formatConditions);

            formatConditions = new FormatCondition();
            formatConditions.FieldName = TargetWarnDetail.TargetColorProperty.Name;
            formatConditions.ValueRule = DevExpress.Xpf.Core.ConditionalFormatting.ConditionRule.Expression;
            formatConditions.Value1 = true;
            formatConditions.Expression = "[{0}] == 2".FormatArgs(TargetWarnDetail.TargetOpetatorsProperty.Name);
            formatConditions.Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format() { Background = Brushes.Green };
            tableView.FormatConditions.Add(formatConditions);
            formatConditions = new FormatCondition();
            formatConditions.FieldName = TargetWarnDetail.TargetColorProperty.Name;
            formatConditions.ValueRule = DevExpress.Xpf.Core.ConditionalFormatting.ConditionRule.Expression;
            formatConditions.Value1 = true;
            formatConditions.Expression = "[{0}] == 0".FormatArgs(TargetWarnDetail.TargetOpetatorsProperty.Name);
            formatConditions.Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format() { Background = Brushes.Red };
            tableView.FormatConditions.Add(formatConditions);
        }
    }

    /// <summary>
    /// 条件变更时，清除数据
    /// </summary>
    public class TargetWarnDetailTypeChange : ViewBehavior
    {
        /// <summary>
        /// 加载时挂载
        /// </summary>
        protected override void OnAttach()
        {
            View.CurrentChanged += View_CurrentChanged;
        }

        /// <summary>
        /// 属性变更时执行
        /// </summary>
        /// <param name="sender">数据发送</param>
        /// <param name="e">参数</param>
        private void View_CurrentChanged(object sender, EventArgs e)
        {
            var vm = View.Current as TargetWarnDetail;
            if (vm != null)
            {
                vm.PropertyChanged -= TargetWarnDetail_PropertyChanged;
                vm.PropertyChanged += TargetWarnDetail_PropertyChanged;
            }
        }

        /// <summary>
        /// 条件属性变更时改变颜色和数据
        /// </summary>
        /// <param name="sender">数据发送</param>
        /// <param name="e">参数</param>
        private void TargetWarnDetail_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TargetWarnDetail.TargetOpetatorsProperty.Name)
            {
                TargetWarnDetail targetwarndetail = sender as TargetWarnDetail;
                targetwarndetail.MinValue = null;
                targetwarndetail.MaxValue = null;
                switch (targetwarndetail.TargetOpetators)
                {
                    case TargetOpetators.Between:
                        targetwarndetail.TargetColor = TargetColor.Yellow;
                        break;
                    case TargetOpetators.GreaterOrEqual:
                        targetwarndetail.TargetColor = TargetColor.Green;
                        break;
                    case TargetOpetators.LessOrEqual:
                        targetwarndetail.TargetColor = TargetColor.Red;
                        break;
                }
            }
        }
    }
}
