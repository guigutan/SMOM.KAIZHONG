using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 分页出库明细信息
    /// </summary>
    [Serializable]
    public class UnloadDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 出库明细列表
        /// </summary>
        public List<UnloadInfo> UnloadInfos { get; } = new List<UnloadInfo>();
    }
}
