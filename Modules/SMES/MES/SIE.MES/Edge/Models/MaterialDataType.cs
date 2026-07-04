using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 物料数据类型
    /// </summary>
    public enum MaterialDataType
    {
        /// <summary>
        /// 上料
        /// </summary>
        Load,
        /// <summary>
        /// 下料
        /// </summary>
        Unload,
        /// <summary>
        /// 扣料
        /// </summary>
        Descrease,
        /// <summary>
        /// 用毕
        /// </summary>
        Used,
        /// <summary>
        /// 离线采集用毕
        /// </summary>
        PreUsed
    }
}
