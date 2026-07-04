using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;

namespace SIE.Web.Fixtures.MaintainTasks
{
    /// <summary>
    /// 工装台帐扩展视图
    /// </summary>
    public class MaintainTaskExtViewConfig : WebViewConfig<FixtureIDAccount>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssociateChildrenProperty(MaintainTaskDetailProperty.MaintainTaskListProperty, (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var account = args.Parent as FixtureAccount;
                var maintainTasks = RT.Service.Resolve<MaintainTaskController>().GetMaintainTasks(account.Id, args.PagingInfo);
                return maintainTasks;
            }, MaintainTaskViewConfig.TaskResumeView).HasLabel("保养履历").OrderNo = 45;
        }
    }

    /// <summary>
    /// 工装台帐扩展视图
    /// </summary>
    public class TaskExtViewConfig : WebViewConfig<FixtureCodeAccount>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssociateChildrenProperty(TaskDetailProperty.MaintainTaskListProperty, (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var account = args.Parent as FixtureAccount;
                var maintainTasks = RT.Service.Resolve<MaintainTaskController>().GetMaintainTasks(account.Id, args.PagingInfo);
                return maintainTasks;
            }, MaintainTaskViewConfig.MaintainResumeView).HasLabel("保养履历").OrderNo = 45;
        }
    }
}
