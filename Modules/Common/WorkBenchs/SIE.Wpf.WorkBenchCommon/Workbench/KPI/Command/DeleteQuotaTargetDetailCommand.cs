using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Command;

namespace SIE.Wpf.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    class DeleteQuotaTargetDetailCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">指标目标定义明细</param>
        /// <returns>返回是否能被执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var quotatargetdetail = view.Current as QuotaTargetDetail;
            if (quotatargetdetail != null && quotatargetdetail.State == State.Disable)
            {
                return true;
            }
            return false;
        }
    }
}
