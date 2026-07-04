using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 备件基础数据
    /// </summary>
    [Serializable]
    public class SparePartData
    {
        /// <summary>
        /// 库存总数
        /// </summary>
        public int StoreTotalQty { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 备件数据
        /// </summary>
        public List<SparePartInfo> Data { get; set; } = new List<SparePartInfo>();
    }
}
