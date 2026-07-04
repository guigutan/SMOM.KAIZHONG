using SIE.Core.ApiModels;
using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 推荐位置查询信息
    /// </summary>
    [Serializable]
    public class FixtureStockQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 治具编码Id
        /// </summary>
        public double EncodeCodeId { get; set; }
    }
}
