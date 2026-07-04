using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 工单退料上传数据
    /// </summary>
    [Serializable]
    public class ProductReturnData : EbsUploadDataBase
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string TransactionType { get; set; }

    }
}
