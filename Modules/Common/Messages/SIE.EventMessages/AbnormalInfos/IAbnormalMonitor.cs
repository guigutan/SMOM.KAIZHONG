using SIE.Core.AbnormalInfos;
using System;

namespace SIE.EventMessages.AbnormalInfos
{

    /// <summary>
    /// 异常管理相关接口
    /// </summary>
    [Services.Service(FallbackType = typeof(AbnormalMonitorDefault))]
    public interface IAbnormalMonitor
    {
        /// <summary>
        /// 根据业务类型获取异常预警定义
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        double? GetAbnormalWarnDefineId(BusinessType businessType);

        /// <summary>
        /// 生成异常任务（Spc）
        /// </summary>
        /// <param name="createTaskSpcEvent">参数</param>
        void CreateTaskBySpc(CreateTaskSpcEvent createTaskSpcEvent);
    }

    /// <summary>
    /// 接口的默认实现
    /// </summary>
    class AbnormalMonitorDefault : IAbnormalMonitor
    {

        /// <summary>
        /// 生成异常任务（Spc）
        /// </summary>
        /// <param name="createTaskSpcEvent">参数</param>
        public void CreateTaskBySpc(CreateTaskSpcEvent createTaskSpcEvent)
        {
            return;
        }
        /// <summary>
        /// 根据业务类型获取异常预警定义
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public double? GetAbnormalWarnDefineId(BusinessType businessType)
        {
            return null;
        }
    }

    /// <summary>
    /// 红牌管理生成异常任务Event
    /// </summary>
    [Serializable]
    public class CreateTaskSpcEvent
    {
        /// <summary>
        /// 异常预警ID
        /// </summary>
        public double AbnormalWarnDefineId { get; set; }
        
        /// <summary>
        /// 异常预警ID
        /// </summary>
        public string ProblemDescription { get; set; }
        
        /// <summary>
        /// 当前操作人ID
        /// </summary>
        public double UserId { get; set; }


        
    }


}
