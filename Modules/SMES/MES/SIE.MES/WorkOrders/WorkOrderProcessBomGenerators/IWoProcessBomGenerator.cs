using System.Collections.Generic;

namespace SIE.MES.WorkOrders.WorkOrderProcessBomGenerators
{
    /// <summary>
    /// 工单工序BOM 生成器接口
    /// </summary>
    public interface IWoProcessBomGenerator
    {
        /// <summary>
        /// 工单工序BOM 生成方法
        /// </summary>
        /// <param name="workOrder">工单</param>
        void GenerateProcessBoms(WorkOrder workOrder);
    }
}
