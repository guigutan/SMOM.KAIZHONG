using SIE.Core.ApiModels;
using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 上架任务查询信息
    /// </summary>
    [Serializable]
    public class ReceiveQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// ID编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 需求单号
        /// </summary>
        public string No { get; set; }
    }
}
