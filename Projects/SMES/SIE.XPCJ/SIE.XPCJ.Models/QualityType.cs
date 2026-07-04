using SIE.XPCJ.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models
{
    /// <summary>
    /// 质量类型
    /// </summary>
    public enum QualityType
    {
        /// <summary>
        /// 原材料
        /// </summary>
        [Label("材料")]
        Item = 0,

        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        Product = 1,

        /// <summary>
        /// 通用
        /// </summary>
        [Label("通用")]
        Common = 4,
    }
}
