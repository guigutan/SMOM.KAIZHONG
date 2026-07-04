using System.Collections.Generic;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 产品BOM数据
    /// </summary>
    public class ProductBomData : ErpInfoData
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 产品BOM明细数据
        /// </summary>
        public List<ProductBomDetailData> DetailData { get; set; } = new List<ProductBomDetailData>();

    }

    /// <summary>
    /// 产品BOM明细数据
    /// </summary>
    public class ProductBomDetailData : ErpInfoData
    {
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 主从一起可以不用赋值，只有从需要赋值
        /// </summary>
        public string ProductBomCode { get; set; }
    }

}
