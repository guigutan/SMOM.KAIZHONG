using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 销售发货上传数据
    /// </summary>
    [Serializable]
    public class SaleOutUploadData: EbsUploadDataBase
    {
        public string ItemCode { get; set; }
    }
}
