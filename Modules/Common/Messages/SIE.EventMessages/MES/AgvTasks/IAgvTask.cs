using SIE.EventMessages.MES.AgvTasks.Datas;

namespace SIE.EventMessages.MES.AgvTasks
{
    /// <summary>
    /// AGV任务接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultIAgvTaskInterface))]
    public interface IAgvTask
    {
        /// <summary>
        /// 创建Agv任务
        /// </summary>
        void CreateAgvTask(AgvTaskData data);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultIAgvTaskInterface : IAgvTask
    {
        /// <summary>
        /// 创建Agv任务
        /// </summary>
        public void CreateAgvTask(AgvTaskData data)
        {
            // 创建Agv任务
        }
    }
}
