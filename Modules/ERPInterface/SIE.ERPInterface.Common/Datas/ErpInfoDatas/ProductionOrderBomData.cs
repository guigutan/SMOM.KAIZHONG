using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 生产订单BOM
    /// </summary>
    [Serializable]
    public class ProductionOrderBomData : ErpInfoData
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 规格描述
        /// </summary>
        public string SpecificationDesc { get; set; }

        /// <summary>
        /// 替代料值 0--主料，1--替代料
        /// </summary>
        public int ReplateItemType { get; set; }

        /// <summary>
        /// 主物料编码
        /// </summary>
        public string MainMaterialCode { get; set; }

        /// <summary>
        /// 元件位号
        /// </summary>
        public string ElementNo { get; set; }

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 制程
        /// </summary>
        public string ProcessTech { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
