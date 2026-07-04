using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 采购退货上传数据
    /// </summary>
    [Serializable]
    public class SupplierReturnUploadData : EbsUploadDataBase
    {
        /// <summary>
        /// 采购订单编号
        /// </summary>
        public string PoErpKey { get; set; }

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string PoLineErpKey { get; set; }

        /// <summary>
        /// 事务处理类型
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 接收单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// 接收单行号
        /// </summary>
        public string AsnLineNo { get; set; }
    }
}
