using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.OutboundConfirm
{
    [Serializable]
    public class KzOutboundConfirmResultData
    {
        /// <summary>
        /// 状态(E失败，S成功)
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 流程单号
        /// </summary>
        public string FLOWNO { get; set; }

        /// <summary>
        /// 唯一码
        /// </summary>
        public string ZUID { get; set; }
    }
}
