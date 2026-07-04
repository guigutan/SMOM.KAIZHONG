using System;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 工序Bom
    /// </summary>
    [Serializable]
    public class ProcessBom
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 是否需要扣料
        /// </summary>
        public bool IsBuckleMaterial { get; set; } = true;

        /// <summary>
        /// 点位
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 工步Id
        /// </summary>
        public double? WorkStepId { get; set; }

        /// <summary>
        /// 是否附件
        /// </summary>
        public bool IsAttachment { get; set; }

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

        /// <summary>
        /// 条码解析
        /// </summary>
        public bool HasBarcodeRule { get; set; }

        /// <summary>
        /// 主料物料ID
        /// </summary>
        public double? MainMaterialId { get; set; }

        /// <summary>
        ///物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }
    }
}
