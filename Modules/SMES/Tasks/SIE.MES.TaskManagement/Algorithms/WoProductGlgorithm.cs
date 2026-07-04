using SIE.Common.Algorithm;
using SIE.MES.TaskManagement.Dispatchs;
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
    [Algorithm("工单产品编码算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class WoProductGlgorithm : CodeAlgorithm<DispatchTask>
    {
        public override string GetCode(DispatchTask data)
        {
            return data?.Product?.Code;
        }
    }
}
