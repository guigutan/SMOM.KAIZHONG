using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 销售退货上传数据
    /// </summary>
    public class SaleReturnUploadData: EbsUploadDataBase
    {
        /// <summary>
        /// 接收人
        /// </summary>
        public string CreateBy { get; set; }
    }
}
