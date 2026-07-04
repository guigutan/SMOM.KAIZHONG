using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 分页工治具需求清单信息
    /// </summary>
    [Serializable]
    public class FixtureDemandDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 工治具需求清单列表
        /// </summary>
        public List<FixtureDemandInfo> FixtureDemandInfos { get; } = new List<FixtureDemandInfo>();
    }
}
