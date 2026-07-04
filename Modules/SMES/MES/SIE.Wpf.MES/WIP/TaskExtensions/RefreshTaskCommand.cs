using SIE.Wpf.Command;

namespace SIE.Wpf.MES.WIP.TaskExtensions
{
    /// <summary>
    /// 刷新任务列表命令
    /// </summary>
    [Command(ImageName = "Refresh", Label = "刷新", ToolTip = "刷新任务列表", GroupType = CommandGroupType.Edit)]
    public class RefreshTaskCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Relations.Find("mainView")?.Current != null;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = view.Relations.Find("mainView")?.Current as DataCollectionViewModel;
            var retrospectType = vm is BatchWIP.BatchDataCollectionViewModel ? Core.Items.RetrospectType.Batch : Core.Items.RetrospectType.Single;
            vm?.RefrshReportTasks(retrospectType, false);
        }
    }
}