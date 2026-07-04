using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 分页领用需求明细信息
    /// </summary>
    [Serializable]
    public class ReceiveDetailDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 工治具领用需求明细列表
        /// </summary>
        public List<ReceiveDetailInfo> ReceiveDetailInfos { get; } = new List<ReceiveDetailInfo>();
    }
}
