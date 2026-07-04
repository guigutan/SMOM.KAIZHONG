using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 采购入库上传对象
    /// </summary>
    public class PurchaseInData:EbsUploadDataBase
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PoNum { get; set; }

        /// <summary>
        /// 采购订单主键
        /// </summary>
        public string ErpDetailId { get; set; }
    }
}
