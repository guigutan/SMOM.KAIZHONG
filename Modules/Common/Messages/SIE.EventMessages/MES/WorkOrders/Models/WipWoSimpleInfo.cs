using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 在制工单信息
    /// </summary>
    [Serializable]
    public class WipWoSimpleInfo
    {
        /// <summary>
        /// 工单编码
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 计划ID
        /// </summary>
        public string DetailId { get; set; }
        /// <summary>
        /// APS工艺单编号
        /// </summary>
        public string ProcessTechOrderCode { get; set; }
        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStart { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEnd { get; set; }

        /// <summary>
        /// 专用号
        /// </summary>
        public string SpecialNo { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 在制工单物料需求
        /// </summary>
        public List<WipMaterialRequirementInfo> WipMaterialRequirementList { get; set; } = new List<WipMaterialRequirementInfo>();
    }

    /// <summary>
    /// 在制工单物料需求信息
    /// </summary>
    [Serializable]
    public class WipMaterialRequirementInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 工序序号
        /// </summary>
        public int? ProcessNum { get; set; }

        /// <summary>
        /// 装配件单位用量
        /// </summary>
        public decimal? QtyPerItem { get; set; }

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal Requirement { get; set; }

        /// <summary>
        /// 已发料量
        /// </summary>
        public decimal IssuedQty { get; set; }

        /// <summary>
        /// 未发料数量
        /// </summary>
        public decimal NotIssuedQty { get; set; }

        /// <summary>
        /// 物料属性值
        /// </summary>
        public List<MaterialRequirementPropertyValueInfo> MaterialRequirementPropertyValueList { get; set; } = new List<MaterialRequirementPropertyValueInfo>();
    }

    /// <summary>
    /// 在制工单物料需求属性值信息
    /// </summary>
    [Serializable]
    public class MaterialRequirementPropertyValueInfo
    {
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double ItemPropertyDefinitionId { get; set; }
    }
}
