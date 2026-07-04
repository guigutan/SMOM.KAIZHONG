using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CapacityUtilizationRateData
    {
        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }
        /// <summary>
        /// 厂部名称
        /// </summary>
        public string PlantName { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 生产产出数
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        /// 标准产能
        /// </summary>
        public decimal StandardCapacity { get; set; }

        /// <summary>
        /// 产能利用率
        /// </summary>
        public decimal CapacityUtilization { get; set; }
    }
}
