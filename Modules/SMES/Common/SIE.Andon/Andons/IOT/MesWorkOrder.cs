using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    /// <summary>
    /// 返回给MES
    /// </summary>
    [Serializable]
    public class MesWorkOrder
    {
        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal OutPutNum { get; set; }
    }
}
