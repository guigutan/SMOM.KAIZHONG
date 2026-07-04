using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// OA流程退回
    /// </summary>
    [Serializable]
    public class OAFlowReturnData
    {
        /// <summary>
        /// 流程单
        /// </summary>
        public string FLOWNO { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 唯一码
        /// </summary>
        public string ZUID { get; set; }

        /// <summary>
        /// 发出工厂
        /// </summary>
        public string INITIATORFACTORY { get; set; }
    }
}
