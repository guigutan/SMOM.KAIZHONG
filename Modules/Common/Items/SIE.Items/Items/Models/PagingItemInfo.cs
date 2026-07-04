using System;
using System.Collections.Generic;

namespace SIE.Items
{
    /// <summary>
    /// 分页物料数据模型
    /// </summary>
    [Serializable]
    public class PagingItemInfo
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 页数据数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 物料列表
        /// </summary>
        public List<ItemInfo> ItemInfos { get; } = new List<ItemInfo>();
    }

}
