using SIE.Core.ApiModels;
using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 需求明细查询信息
    /// </summary>
    [Serializable]
    public class DemandDetailQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 需求单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>

        public double? WareHouseId { get; set; }
    }
}
