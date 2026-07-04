using SapNwRfc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    public class SupplierInfo
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        [SapName("LIFNR")]
        public string LIFNR { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [SapName("NAME1")]
        public string NAME1 { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        [SapName("ERDAT")]
        public DateTime LastUpdateTime { get; set; }
    }
}
