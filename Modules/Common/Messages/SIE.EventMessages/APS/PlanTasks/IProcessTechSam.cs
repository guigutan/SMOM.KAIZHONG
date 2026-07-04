using System;
using System.Collections.Generic;

namespace SIE.EventMessages.APS.PlanTasks
{
    /// <summary>
    /// 计划明细工艺定额接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyProcessTechSam))]
    public interface IProcessTechSam
    {
        /// <summary>
        /// 根据工艺定额查询对象获取计划明细工艺定额
        /// </summary>
        /// <param name="criteriaInfos">工艺定额查询对象</param>
        /// <returns>计划明细工艺定额</returns>
        IReadOnlyList<ProcessTechSamInfo> GetProcessTechSam(List<ProcessTechSamCriteriaInfo> criteriaInfos);
    }

    /// <summary>
    /// 工艺定额接口默认实现
    /// </summary>
    public class EmptyProcessTechSam : IProcessTechSam
    {
        /// <summary>
        /// 根据工艺定额查询对象获取计划明细工艺定额
        /// </summary>
        /// <param name="criteriaInfos">工艺定额查询对象</param>
        /// <returns>计划明细工艺定额</returns>
        public IReadOnlyList<ProcessTechSamInfo> GetProcessTechSam(List<ProcessTechSamCriteriaInfo> criteriaInfos)
        {
            return new List<ProcessTechSamInfo>();
        }
    }

    /// <summary>
    /// 工艺定额列表
    /// </summary>
    [Serializable]
    public class ProcessTechSamCriteriaInfo
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceID { get; set; }
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }
        /// <summary>
        /// 制程工艺ID
        /// </summary>
        public double ProcessTechId { get; set; }
    }


    /// <summary>
    /// 工艺定额列表
    /// </summary>
    [Serializable]
    public class ProcessTechSamInfo
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceID { get; set; }
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }
        /// <summary>
        /// 制程工艺ID
        /// </summary>
        public double ProcessTechId { get; set; }
        /// <summary>
        /// 工艺定额
        /// </summary>
        public double Sam { get; set; }
    }
}
