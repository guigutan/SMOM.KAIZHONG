using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Items.ProductBoms.Models
{
    /// <summary>
    /// 产品bom明细信息
    /// </summary>
    [Serializable]
    public class ProBomDtlInfo
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal UnitQty { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal LossRate { get; set; }

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool? IsRecoilItem { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Bom编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// Bom名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
