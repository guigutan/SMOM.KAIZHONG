using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 采集BOM数据
    /// </summary>
    [Serializable]
    public class EdgeBom
    {

        /// <summary>
        /// 工单工序BOM ID
        /// </summary>
        public string BomId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 替代料
        /// </summary>
        public List<EdgeBom> AltBom { get; } = new List<EdgeBom>();

        /// <summary>
        /// 系统外条码
        /// </summary>
        public bool IsExternal { get; set; }

        /// <summary>
        /// 单体条码管控
        /// </summary>
        public bool IsSingleLabel { get; set; }

        /// <summary>
        /// 是否可重复
        /// </summary>
        public bool IsRepeat { get; set; }
    }
}
