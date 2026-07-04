using SIE.Items.ProductBoms.Models;
using System;
using System.Collections.Generic;

namespace SIE.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM关系
    /// </summary>
    [Serializable]
    public class ProductBomRelationViewModel
    {
        /// <summary>
        /// 产品BOM ID
        /// </summary>
        public double BomId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性列表
        /// </summary>
        public List<ItemPropertyInfo> ItemPropertyInfoList { get; set; }

        /// <summary>
        /// 子物料ID
        /// </summary>
        public double? ChildItemId { get; set; }

        /// <summary>
        /// 子物料扩展属性
        /// </summary>
        public string ChildItemExtProp { get; set; }

        /// <summary>
        /// 子物料扩展属性值
        /// </summary>
        public string ChildItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性列表
        /// </summary>
        public List<ItemPropertyInfo> ChildItemPropertyInfoList { get; set; }

        /// <summary>
        /// 子物料单位用量
        /// </summary>
        public decimal? UnitQty { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string ChildItemCode { get; set; }

        /// <summary>
        /// 子物料来源类型
        /// </summary>
        public ItemSourceType? ChildItemSourceType { get; set; }

        /// <summary>
        /// 工段ID
        /// </summary>
        public double? ProcessSegmentId { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType? ChildType { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal LossRate { get; set; }

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool? IsRecoilItem { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductBomRelationViewModel()
        {
            this.ItemPropertyInfoList = new List<ItemPropertyInfo>();
            this.ChildItemPropertyInfoList = new List<ItemPropertyInfo>();
        }
    }
}