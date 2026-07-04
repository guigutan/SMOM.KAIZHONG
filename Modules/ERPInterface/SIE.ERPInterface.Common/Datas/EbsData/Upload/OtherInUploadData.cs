using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 其他入库上传数据
    /// </summary>
    public class OtherInUploadData: EbsUploadDataBase
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 账户别名
        /// </summary>
        public string ErpAccountCode { get; set; }
    }
}
