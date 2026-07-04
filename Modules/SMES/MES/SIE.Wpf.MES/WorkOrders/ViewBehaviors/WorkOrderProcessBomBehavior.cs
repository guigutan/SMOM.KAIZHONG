using SIE.Domain;
using SIE.MES.WorkOrders;

namespace SIE.Wpf.MES.WorkOrders.ViewBehaviors
{
    /// <summary>
    /// 工序BOM行为
    /// </summary>
    public class WorkOrderProcessBomBehavior : ViewBehavior
    {
        /// <summary>
        /// 是否正在切换工序BOM
        /// </summary>
        private bool isRun;

        /// <summary>
        /// 当前实体
        /// </summary>
        private Entity Current;

        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            if (view != null)
            {
                view.CurrentChanged -= ProcessBom_CurrentChanged;
                view.CurrentChanged += ProcessBom_CurrentChanged;
            }
        }

        /// <summary>
        /// 当前工序BOM对象变更
        /// </summary>
        /// <param name="sender">当前变更的视图对象</param>
        /// <param name="e">事件参数</param>
        private void ProcessBom_CurrentChanged(object sender, System.EventArgs e)
        {
            ListLogicalView logicalView = sender as ListLogicalView;
            if (logicalView != null)
            {
                if (Current != null)
                {
                    Current.PropertyChanged -= ProcessBom_PropertyChanged;
                }

                WorkOrderProcessBom processBom = logicalView.Current as WorkOrderProcessBom;
                Current = processBom;
                if (processBom != null)
                {
                    processBom.PropertyChanged -= ProcessBom_PropertyChanged;
                    processBom.PropertyChanged += ProcessBom_PropertyChanged;
                }
            }
        }

        /// <summary>
        /// 工序BOM变更
        /// </summary>
        /// <param name="sender">变更对象</param>
        /// <param name="e">变更事件</param>
        private void ProcessBom_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!isRun)
            {
                isRun = true;
                try
                {
                    if (e.PropertyName == WorkOrderProcessBom.ItemProperty.Name)
                    {
                        WorkOrderProcessBom processBom = sender as WorkOrderProcessBom;
                        if (processBom != null && processBom.ItemId > 0)
                        {
                            processBom.UnitId = processBom.Item.UnitId;
                        }
                    }
                }
                finally
                {
                    isRun = false;
                }
            }
        }
    }
}
