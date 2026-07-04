using SIE.Common.Algorithm;
using System;

namespace SIE.Core.WorkOrders.Algorithms
{
    /// <summary>
    /// 工单号编码算法
    /// </summary>
    [Algorithm("工单号编码算法", typeof(CodeAlgorithmConfig), AlgorithmType.Barcode)]
    [RootEntity, Serializable]
    public class WorkOrderNoAlgorithm : CodeAlgorithm<WorkOrder>
    {
        /// <summary>
        /// 获取工单号
        /// </summary>
        /// <param name="data">工单</param>
        /// <returns>工单号</returns>
        public override string GetCode(WorkOrder data)
        {
            return data?.No;
        }
    }
}
