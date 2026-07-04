using DocumentFormat.OpenXml.Office2016.Presentation.Command;
using SIE.Core.Common;
using SIE.Units;
using System;
using System.Collections.Generic;

namespace SIE.Items
{
    /// <summary>
    /// 物料基本信息
    /// </summary>
    [Serializable]
    public class ItemBaseData : EntityBaseData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string SpecificationModel { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
    }

    /// <summary>
    /// 扩展属性数据
    /// </summary>
    [Serializable]
    public class StockExtensionData
    {
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 属性id
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public List<string> valList { get; set; }
    }

    /// <summary>
    /// 简单的物料数据
    /// </summary>
    public class SimpleItemData
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel { get; set; }
    }

    /// <summary>
    /// 单位进位
    /// </summary>
    public class ItemUnitsModel : UnitsModel
    {
        /// <summary>
        /// 物料id
        /// </summary>
        public double SecondUnitId { get; set; }
    }

    /// <summary>
    /// 物料信息
    /// </summary>
    [Serializable]
    public class TransferItemInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
        
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 是否批次管理
        /// </summary>
        public bool IsBatch { get; set; }

        /// <summary>
        /// 是否位置跟踪
        /// </summary>
        public bool IsLocation { get; set; }

        /// <summary>
        /// 启用扩展属性
        /// </summary>
        public bool EnableExtendProperty { get; set; }

        /// <summary>
        /// 物料包装规则明细
        /// </summary>
        public List<RuleDetail> ItemRuleDetail { get; set; } = new List<RuleDetail>();
    }

    /// <summary>
    /// 包装规则明细
    /// </summary>
    public class RuleDetail
    {
        

        /// <summary>
        /// 产品数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 包装数
        /// </summary>
        public decimal LevelQty { get; set; }

        /// <summary>
        /// 是否最小包装
        /// </summary>
        public bool IsMinPacking { get; set; }

        /// <summary>
        /// 是否序列
        /// </summary>
        public bool IsSequence { get; set; }

        /// <summary>
        /// 是否主单位
        /// </summary>
        public bool IsMasterUnit { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string PackageUnitName { get; set; }
    }
}
