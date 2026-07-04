using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIE.XPCJ.Models.Attributes;

namespace SIE.XPCJ.Models.Enums
{
    [Serializable]
    public enum ResultType
    {
        /// <summary>
        /// 通过
        /// </summary>
        [Label("通过")]
        Pass = 1,
        /// <summary>
        /// 失败
        /// </summary>
        [Label("失败")]
        Fail = 2,
        /// <summary>
        /// 自定义
        /// </summary>
        [Label("自定义")]
        Custom = 4,
        /// <summary>
        /// 可选
        /// </summary>
        [Label("可选")]
        Optional = 8,
    }
}
