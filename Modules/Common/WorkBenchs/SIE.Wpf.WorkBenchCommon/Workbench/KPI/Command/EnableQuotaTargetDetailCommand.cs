using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Common.Commands;
using System.Linq;

namespace SIE.Wpf.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 指标定义明细表启用命令
    /// </summary>
    [Command(ImageName = "Play", Label = "启用", ToolTip = "启用选中行数据", GroupType = 30)]
    class EnableQuotaTargetDetailCommand : EnableCommand
    {
        /// <summary>
        /// 命令是否能把执行
        /// </summary>
        /// <param name="view">详细逻辑视图</param>
        /// <returns>返回命令是否能把执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.SelectedEntities.OfType<QuotaTargetDetail>()?.Count(p => p.PersistenceStatus == PersistenceStatus.New) > 0)
            {
                return false;
            }

            return base.CanExecute(view);
        }
    }
}
