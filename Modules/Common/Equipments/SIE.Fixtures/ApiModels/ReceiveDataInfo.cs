using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 分页工治具领用清单信息
    /// </summary>
    [Serializable]
    public class ReceiveDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 工治具领用清单列表
        /// </summary>
        public List<FixtureReceiveInfo> FixtureReceiveInfos { get; } = new List<FixtureReceiveInfo>();
    }
}
