using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Resources.WipResources.Models
{
    /// <summary>
    /// 车间查询信息
    /// </summary>
    [Serializable]
    public class WipResourceQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 车间ID列表
        /// </summary>
        public List<double?> WorkShopIdList { get; set; }
    }
}
