using System;
using System.Collections.Generic;

namespace SIE.Items.ProductBoms.Models
{
    /// <summary>
    /// 物料唯一标识信息
    /// </summary>
    [Serializable]
    public class ItemUniqueInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 扩展属性（不要做逻辑处理，只作展示、调试）
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性值（不要做逻辑处理，只作展示、调试）
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性列表
        /// </summary>
        public List<ItemPropertyInfo> ItemPropertyInfoList { get; set; }
    }
}