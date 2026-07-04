using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BarcodeProcesses.DataModels
{
    /// <summary>
    /// 工单工序清单信息
    /// </summary>
    [Serializable]
    public class WoProcessData
    {
        /// <summary>
        /// 工序id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public double ProcessCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public double ProcessName { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Index { get; set; }
    }
}
