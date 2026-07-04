using SIE.MES.Workbench.Experiences;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.Workbench.Experiences.Commands
{
    /// <summary>
    /// 删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除选中行数据", Gestures = "Delete", GroupType = 10)]
    public class ExperienceDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 重写删除方法
        /// </summary>
        /// <param name="view">view</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var entity = view.Current as HistoryExperience;
            return entity != null && entity.ExperienceDetailList.Count == 0;
        }
    }
}
