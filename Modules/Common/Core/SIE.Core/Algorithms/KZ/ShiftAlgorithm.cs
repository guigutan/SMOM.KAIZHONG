using SIE.Common.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Algorithms.KZ
{
    /// <summary>
    /// 班次算法(早A晚B)
    /// </summary>
    [Algorithm("班次算法(早A晚B)", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]

    public class ShiftAlgorithm : EntityCodeAlgorithm
    {
        public override string GetCode()
        {
            return RT.Service.Resolve<ItemCusotmerDataController>().ShiftAlgorithmGetCode();
        }
    }
}
