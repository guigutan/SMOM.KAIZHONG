using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 其他出库上传数据
    /// </summary>
    [Serializable]
    public class OtherOutUploadData : EbsUploadDataBase
    {
        /// <summary>
        /// 事务处理类型
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 账户别名
        /// </summary>
        public string ErpAccount {  get; set; }
    }
}
