using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 按单号或者Id编码获取领用需求信息
    /// </summary>
    [Serializable]
    public class ReceiveSearchInfo
    {
        /// <summary>
        /// 是否进入领用明细
        /// </summary>
        public bool IsReceive { get; set; }

        /// <summary>
        /// 工治具领用需求明细列表
        /// </summary>
        public List<ReceiveDetailInfo> ReceiveDetailInfos { get; } = new List<ReceiveDetailInfo>();

        /// <summary>
        /// 工治具领用清单列表
        /// </summary>
        public List<FixtureReceiveInfo> FixtureReceiveInfos { get; } = new List<FixtureReceiveInfo>();
    }
}
