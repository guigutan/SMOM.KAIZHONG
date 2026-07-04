using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 分页推荐位置信息
    /// </summary>
    [Serializable]
    public class FixtureStockDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 推荐位置列表
        /// </summary>
        public List<FixtureStockInfo> FixtureStockInfos { get; } = new List<FixtureStockInfo>();
    }
}
