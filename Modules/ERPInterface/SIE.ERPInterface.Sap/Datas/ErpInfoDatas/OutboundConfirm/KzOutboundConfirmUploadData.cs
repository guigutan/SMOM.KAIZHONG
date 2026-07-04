using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.OutboundConfirm
{
    [Serializable]
    public class KzOutboundConfirmUploadData
    {
        /// <summary>
        /// 流程单号
        /// </summary>
        public string FLOWNO { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>
        public string OUTER { get; set; }

        /// <summary>
        /// 发出工厂
        /// </summary>
        public string INITIATORFACTORY { get; set; }

        /// <summary>
        /// 接收工厂
        /// </summary>
        public string OUTFACTORY { get; set; }

        /// <summary>
        /// 状态(0:新建,1:更新)
        /// </summary>
        public string STATE { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public string DATE { get; set; }

        /// <summary>
        /// 框数
        /// </summary>
        public decimal QTY { get; set; }

        /// <summary>
        /// 唯一码
        /// </summary>
        public string ZUID { get; set; }

    }
}
