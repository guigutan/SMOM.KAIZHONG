using SIE.CrossPlatform.Collect.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models
{
    /// <summary>
    /// 严重度
    /// </summary>
    public enum DefectSeverity
    {
        /// <summary>
        /// 致命
        /// </summary>
        [Label("致命")]
        deadly = 0,

        /// <summary>
        /// 高
        /// </summary>
        [Label("高")]
        high = 1,

        /// <summary>
        /// 中
        /// </summary>
        [Label("中")]
        middle = 2,

        /// <summary>
        /// 轻
        /// </summary>
        [Label("轻")]
        light = 3,
    }
}
