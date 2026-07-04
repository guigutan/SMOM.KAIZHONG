using SIE.MES.Interfaces.TaskManages;
using SIE.MES.TaskManagement.Dispatchs;

namespace SIE.MES.TaskManagement.Interfaces
{
    /// <summary>
    /// 任务管理接口实现
    /// </summary>
    public class TaskManage : ITaskManage
    {
        /// <summary>
        /// 是否生成任务单
        /// </summary>
        /// <returns>生成返回true，否则返回false</returns>
        public bool IsGenerateTask()
        {
            var taskConfig = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
            return taskConfig != null && taskConfig.IsGenerate;
        }
    }
}