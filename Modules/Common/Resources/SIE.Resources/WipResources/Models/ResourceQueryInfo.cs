using SIE.Core.ApiModels;
using System;

namespace SIE.Resources.WipResources.Models
{
    /// <summary>
    /// 车间查询信息
    /// </summary>
    [Serializable]
    public class ResourceQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId { get; set; }
    }
}