using SIE.CrossPlatform.Collect.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 追溯方式
    /// </summary>
    public enum RetrospectType
    {
        /// <summary>
        /// 单体追溯
        /// </summary>
        [Label("单体追溯")]
        Single,
        /// <summary>
        /// 批次追溯
        /// </summary>
        [Label("批次追溯")]
        Batch,
    }
}
