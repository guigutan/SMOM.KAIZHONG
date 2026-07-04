using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 上料:获取工序BOM信息
    /// </summary>
    [Serializable]
    public class Pda_AssemblyProcessBomInfo
    {
        /// <summary>
        /// 工序BomId
        /// </summary>
        public double ProcessBomId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainingQty { get; set; }

        /// <summary>
        /// 制卡需求量
        /// </summary>
        public decimal CardDemandQty { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }
    }
}
