using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 库存调拨
    /// </summary>
    [Serializable]
    public class AllocateInUploadData : EbsUploadDataBase
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
        /// 来源货位
        /// </summary>
        public string FromLocationCode { get; set; }

        /// <summary>
        /// 目标子库存
        /// </summary>
        public string TargetErpWarehouseCode { get; set; }

        /// <summary>
        /// 目标货位
        /// </summary>
        public string ToLocationCode { get; set; }

        /// <summary>
        /// 目标库存组织名称
        /// </summary>
        public string TargetOrganizationName { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string SourceBillNo { get; set; }

        /// <summary>
        /// 来源单行号
        /// </summary>
        public string SourceBillLineNo { get; set; }
    }
}
