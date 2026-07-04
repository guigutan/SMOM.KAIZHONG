using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 分页需求明细信息
    /// </summary>
    [Serializable]
    public class DemandDetailDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 工治具需求清单列表
        /// </summary>
        public List<DemandDetailInfo> DemandDetailInfos { get; } = new List<DemandDetailInfo>();
    }
}
