using SIE.Common.Algorithm;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Algorithms
{
    /// <summary>
    /// 工单产品编码算法
    /// </summary>
    [Algorithm("工单工厂编码算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class WoFactoryGlgorithm : CodeAlgorithm
    {
        public override string GetCode()
        {
            var factory = string.Empty;
            if (Context.Data is DispatchTask)
            {
                var task = Context.Data as DispatchTask;
                factory = task.WorkOrder?.Factory?.Code;
            }
            else if (Context.Data is WorkOrder)
            {
                var wo = Context.Data as WorkOrder;
                factory = wo.Factory?.Code;
            }
            return factory;
        }
    }
}
