using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.APS.DeliveryPlans
{
    /// <summary>
    /// 高级排程接口
    /// </summary>
    public interface ISchedule
    {
        /// <summary>
        /// 根据计划编号获取计划信息
        /// </summary>
        /// <param name="planNos">计划编号列表</param>
        /// <returns>计划信息字典。key：计划编号；value：1：制程ID；2：计划数量；3：计划开始时间；4：计划结束时间；5：资源ID</returns>
        Dictionary<string, Tuple<double?, decimal?, DateTime?, DateTime?, double?>> GetPlanNoInfoTupleDic(List<string> planNos);
    }


    /// <summary>
    /// 高级排程接口默认实现
    /// </summary>
    public class EmptySchedule : ISchedule
    {
        /// <summary>
        /// 根据计划编号获取计划信息
        /// </summary>
        /// <param name="planNos">计划编号列表</param>
        /// <returns>计划信息字典。key：计划编号；value：1：制程ID；2：计划数量；3：计划开始时间；4：计划结束时间；5：资源ID</returns>
        public Dictionary<string, Tuple<double?, decimal?, DateTime?, DateTime?, double?>> GetPlanNoInfoTupleDic(List<string> planNos)
        {
            return new Dictionary<string, Tuple<double?, decimal?, DateTime?, DateTime?, double?>>();
        }
    }
}
