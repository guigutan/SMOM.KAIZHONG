using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 备件仓库基础数据分页查询结果信息
    /// </summary>
    [Serializable]
    public class SparePartWhPagingResultInfo : PagingResultInfo
    {
        /// <summary>
        /// 备件数据
        /// </summary>
        public List<SparePartWarehouseInfo> SparePartWarehouseInfos { get; set; } = new List<SparePartWarehouseInfo>();
    }
}
