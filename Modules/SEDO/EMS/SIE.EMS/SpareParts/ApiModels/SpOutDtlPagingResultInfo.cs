using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 备件出库单分页查询结果实体
    /// </summary>
    [Serializable]
    public class SpOutDtlPagingResultInfo : PagingResultInfo
    {
        /// <summary>
        /// 备件出库单查询结果明细实体
        /// </summary>
        public List<SparePartOutInfo> SparePartOutDtlInfos { get; set; } = new List<SparePartOutInfo>();
    }
}
