using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 备件仓库分页查询实体
    /// </summary>
    [Serializable]
    public class SparePartDepotQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 备件ID
        /// </summary>
        public double SparePartId { get; set; }
    }
}
