using SIE.WorkBenchCommon.Workbench.KPI;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.Wpf.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 指标定义明细表行为
    /// </summary>
    class QuotaTargetSettingBehavior : ViewBehavior
    {
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
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void MainView_CurrentChanged(object sender, EventArgs e)
        {
            var vm = View.Current as QuotaTargetSetting;
            if (vm != null)
            {
                var childViews = View.ChildrenViews.Where(p => p.EntityType == typeof(QuotaTargetDetail)).ToList();
                if (childViews.Count >= 3)
                {
                    if (vm.DataType == DateType.YEAR)
                    {
                        childViews[0].IsVisible = true;
                        childViews[1].IsVisible = false;
                        childViews[2].IsVisible = false;
                    }
                    else if (vm.DataType == DateType.MONTH)
                    {
                        childViews[0].IsVisible = false;
                        childViews[1].IsVisible = true;
                        childViews[2].IsVisible = false;
                    }
                    else
                    {
                        childViews[0].IsVisible = false;
                        childViews[1].IsVisible = false;
                        childViews[2].IsVisible = true;
                    }
                }

                vm.PropertyChanged -= VM_PropertyChanged;
                vm.PropertyChanged += VM_PropertyChanged;
            }
        }

        /// <summary>
        /// 属性变更方法
        /// </summary>
        /// <param name="sender">变更对象</param>
        /// <param name="e">变更事件</param>
        private void VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var main = sender as QuotaTargetSetting;

            if (e.PropertyName == QuotaTargetSetting.CodeProperty.Name)
            {
                main.Name = null;
            }

            if (e.PropertyName != nameof(QuotaTargetSetting.DataType)) return;

            var childViews = View.ChildrenViews.Where(p => p.EntityType == typeof(QuotaTargetDetail)).ToList();

            if (childViews.Count < (int)main.DataType) return;

            if (main.DataType == DateType.MONTH || main.DataType == DateType.WEEK)
            {
                var childView = childViews[(int)main.DataType - 1];
                if (childView != null)
                {
                    childViews.ForEach(f => f.IsVisible = false);
                    childView.IsVisible = true;
                }
            }
            else
            { 
                var childView = childViews[(int)main.DataType];
                if (childView != null)
                {
                    childViews.ForEach(f => f.IsVisible = false);
                    childView.IsVisible = true;
                }
            }

            main.QuotaTargetDetailList.Clear();
        }
    }

    /// <summary>
    /// 指标目标定义查询实体行为
    /// </summary>
    class QuotaTargetSettingCriteriaBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加视图行为
        /// </summary>
        protected override void OnAttach()
        {
            var vm = View.Current as QuotaTargetSettingCriteria;
        
            vm.PropertyChanged -= VM2_PropertyChanged;
            vm.PropertyChanged += VM2_PropertyChanged;
        }

        /// <summary>
        /// 属性变更方法
        /// </summary>
        /// <param name="sender">变更对象</param>
        /// <param name="e">变更事件</param>
        private void VM2_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var main = sender as QuotaTargetSettingCriteria;

            if (e.PropertyName == QuotaTargetSettingCriteria.CodeProperty.Name)
            {
                main.Name = null;
            }
        }
    }
}
