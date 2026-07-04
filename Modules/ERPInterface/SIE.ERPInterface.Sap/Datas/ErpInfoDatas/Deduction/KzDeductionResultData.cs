using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Deduction
{
    /// <summary>
    /// 扣料接口返回
    /// </summary>
    [Serializable]
    public class KzDeductionResultData
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 返回标识
        /// </summary>
        public string ZFKBS { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string ZFKXX { get; set; }

        /// <summary>
        /// 返回物料凭证
        /// </summary>
        public string MBLNR { get; set; }

        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        public string MJAHR { get; set; }
    }
}
