using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 目标预警设定限制添加按钮
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class TargetWarnDetailAddCommand : ListAddCommand
   {
        /// <summary>
        /// 如果当前有三条数据，就不可添加
        /// </summary>
        /// <param name="view">详细逻辑视图</param>
        /// <returns>默认行为</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var detail = view.Data as EntityList<TargetWarnDetail>;
            if (detail == null)
                return false;
            if (detail.Count() == 3)
            {
                return false;
            }

            return base.CanExecute(view);
        }
    }
}
