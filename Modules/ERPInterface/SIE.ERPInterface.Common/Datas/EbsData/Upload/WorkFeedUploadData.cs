using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 工单发料上传数据
    /// </summary>
    [Serializable]
    public class WorkFeedUploadData : EbsUploadDataBase
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 组件编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 事务处理类型
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// 工序号
        /// </summary>
        public string OperationNo { get; set; }

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal ProductBatchQuantity { get; set; }
    }
}
