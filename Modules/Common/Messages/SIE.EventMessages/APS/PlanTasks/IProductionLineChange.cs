using System;

namespace SIE.EventMessages.APS.PlanTasks
{
    /// <summary>
    /// 转产计划接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyKanbanPlanTaskUpdate))]
    public interface IProductionLineChange
    {
        /// <summary>
        /// 转产计划接口
        /// </summary>
        /// <param name="lineChangeInfo">转产线信息</param>
        /// <returns></returns>
        int ProductionLineChange(ProductionLineChangeInfo lineChangeInfo);
    }
    /// <summary>
    /// 转产计划接口默认实现
    /// </summary>
    public class EmptyProductionLineChange : IProductionLineChange
    {
        /// <summary>
        /// 转产计划接口
        /// </summary>
        /// <param name="lineChangeInfo">转产线信息</param>
        /// <returns></returns>
        public int ProductionLineChange(ProductionLineChangeInfo lineChangeInfo)
        {
            return 0;
        }
    }



    /// <summary>
    /// 转产类
    /// </summary>
    [Serializable]
    public class ProductionLineChangeInfo
    {
        /// <summary>
        /// 计划明细id
        /// </summary>
        public string DetaillId { get; set; }
        /// <summary>
        /// 切分数量
        /// </summary>
        public double SplitQty { get; set; }
        /// <summary>
        /// 目标产线
        /// </summary>
        public double ToFaId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime ToStart { get; set; }
    }
}
