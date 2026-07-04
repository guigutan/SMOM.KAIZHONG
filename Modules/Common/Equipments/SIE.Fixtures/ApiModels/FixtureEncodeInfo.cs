using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 工治具编码信息
    /// </summary>
    [Serializable]
    public class FixtureEncodeInfo
    {
        /// <summary>
        /// 工治具编码
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 库存可用数(合格数+不合格数)
        /// </summary>
        public int CanUseNum { get; set; }
    }
}
