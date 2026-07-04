using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData.Upload
{
    /// <summary>
    /// 完工入库(成品/半成品入库)
    /// </summary>
    public class FinishedInUploadData: EbsUploadDataBase
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
    }
}
