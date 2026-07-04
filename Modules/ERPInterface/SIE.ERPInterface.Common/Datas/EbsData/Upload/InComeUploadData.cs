using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 来料暂收
    /// </summary>
    [Serializable]
    public class InComeUploadData : EbsUploadDataBase
    {
        /// <summary>
        /// 事务处理类型
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// 组件编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 接收单号
        /// </summary>
        public string ReceiveNo { get; set; }

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PurchaseNo { get; set; }

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string PurchaseLineNo {  get; set; }
    }
}
