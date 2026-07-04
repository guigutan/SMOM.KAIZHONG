using SIE.Domain;
using SIE.Fixtures.MaintainTasks;
using SIE.Web.Data;

namespace SIE.Web.Fixtures.MaintainTasks.DataQuery
{
    /// <summary>
    /// 保养任务和上架任务查询器
    /// </summary>
    public class TaskDataQueryer : DataQueryer
    {
        /// <summary>
        /// 根据保养任务Id获取保养执行详情列表
        /// </summary>
        /// <param name="taskId">保养任务Id</param>
        /// <returns>保养执行详情列表</returns>
        public EntityList<MaintainTaskDetail> GetMaintainTaskDetails(double taskId)
        {
            return RT.Service.Resolve<MaintainTaskController>().GetMaintainTaskDetails(taskId);
        }
    }
}
