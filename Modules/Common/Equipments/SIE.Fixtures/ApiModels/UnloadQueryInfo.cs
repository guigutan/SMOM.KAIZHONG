using SIE.Core.ApiModels;
using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 出库查询信息
    /// </summary>
    [Serializable]
    public class UnloadQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 需求单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 治具编码Id
        /// </summary>
        public double EncodeCodeId { get; set; }
    }
}