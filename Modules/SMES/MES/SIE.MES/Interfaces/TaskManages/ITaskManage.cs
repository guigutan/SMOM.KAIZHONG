using SIE.Services;

namespace SIE.MES.Interfaces.TaskManages
{
    /// <summary>
    /// 任务管理接口，实现MES跟任务管理通讯
    /// </summary>
    [Service(FallbackType = typeof(DefaultTaskManage))]
    public interface ITaskManage
    {
        /// <summary>
        /// 是否生成任务单
        /// </summary>
        /// <returns>生成返回true，否则返回false</returns>
        bool IsGenerateTask();
    }

    /// <summary>
    ///  默认实现
    /// </summary>
    public class DefaultTaskManage : ITaskManage
    {
        /// <summary>
        /// 是否生成任务单
        /// </summary>
        /// <returns>生成返回true，否则返回false</returns>
        public bool IsGenerateTask()
        {
            return false;
        }
    }
}