using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 产品报检信息
    /// </summary>
    [Serializable]
    public class ProductInspectInfo
    {
        /// <summary>
        /// 报检单Id
        /// </summary>
        public double InspLogId { get; set; }

        /// <summary>
        /// 报检类型
        /// </summary>
        public int InspType {  get; set; }

        /// <summary>
        /// Qms报检单号
        /// </summary>
        public string QmsInspNo { get; set; }
    }
}
