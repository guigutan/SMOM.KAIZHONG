using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.EventMessages.EngineerPlans
{
    /// <summary>
    /// 同步产生或更新Mi任务 接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalutScheduleAndCreateMITask))]
    public interface IScheduleAndCreateMITask
    {
        /// <summary>
        /// 同步产生或更新Mi任务
        /// </summary>
        /// <param name="datas">发料数据</param>
        /// <returns>WMS发料单号</returns>
        void SyncMiTask(List<ScheduleAndCreateMITaskData> datas);
    }

    /// <summary>
    /// 接口实现
    /// </summary>
    public class DefalutScheduleAndCreateMITask : IScheduleAndCreateMITask
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        public void SyncMiTask(List<ScheduleAndCreateMITaskData> datas)
        {
            return;
        }
    }
}
