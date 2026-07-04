using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// CS生产报工-上料
    /// </summary>
    [Serializable]
    public class CsFeedingItemInfos
    {
        /// <summary>
        /// 工序BomId
        /// </summary>
        public double ProcessBomId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty { get; set; }
    }
}
