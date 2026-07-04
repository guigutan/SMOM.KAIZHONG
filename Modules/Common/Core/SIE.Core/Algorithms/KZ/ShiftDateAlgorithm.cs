using SIE.Common.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Algorithms.KZ
{
    /// <summary>
    /// 白夜班时间编码算法
    /// </summary>
    [Algorithm("白夜班时间编码算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class ShiftDateAlgorithm: EntityCodeAlgorithm
    {
        public override string GetCode()
        {
            return RT.Service.Resolve<ItemCusotmerDataController>().GetShiftDate().ToString("yyMMdd");
        }
    }
}
