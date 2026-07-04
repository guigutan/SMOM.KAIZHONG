using DocumentFormat.OpenXml.Bibliography;
using SIE.Common.Algorithm;
using SIE.Core.Algorithms.Enterprises;
using SIE.Core.Algorithms.KZ;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Algorithms.KZ
{

    /// <summary>
    /// KZ-比亚迪特征码算法
    /// </summary>
    [Algorithm("KZ-比亚迪特征码算法", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class BydCodeAlgorithm : CodeAlgorithm<ItemCusotmerData>
    {
        /// <summary>
        /// 获取 图号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override string GetCode(ItemCusotmerData data)
        {
            return RT.Service.Resolve<IBydCode>().GetBydCode(data?.BatchNo, data?.LineCode);
        }
    }

    /// <summary>
    /// 比亚迪特征码接口
    /// </summary>
    public interface IBydCode
    {
        /// <summary>
        /// 获取比亚迪特征码
        /// </summary>
        /// <param name="batchNo">报工批次号</param>
        /// <param name="lineCode">当前产线</param>
        /// <returns></returns>
        string GetBydCode(string batchNo, string lineCode);
    }
}
