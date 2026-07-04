using SIE.Core.ApiModels;
using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 工治具需求清单查询信息
    /// </summary>
    [Serializable]
    public class FixtureDemandQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 需求单号
        /// </summary>
        public string No { get; set; }
    }
}
