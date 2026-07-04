using System;
using System.Collections.Generic;

namespace SIE.MES.WIP.Runtime
{
    /// <summary>
    /// 采集运行时工序BOM
    /// </summary>
    [Serializable]
    public class bom
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public bom()
        {
            AltBom = new List<bom>();
        }

        /// <summary>
        /// 工单工序BOM ID
        /// </summary>
        public double BomId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 用量
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
        /// 替代料
        /// </summary>
        public List<bom> AltBom { get; }

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
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup { get; set; }
    }
}
