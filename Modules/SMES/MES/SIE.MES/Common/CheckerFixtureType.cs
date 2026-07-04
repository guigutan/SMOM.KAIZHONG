using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Common
{
    /// <summary>
    /// 工具类型
    /// </summary>
    public enum CheckerFixtureType
    {
        /// <summary>
        /// 工装
        /// </summary>
        [Label("工装")]
        Fixture = 1,

        /// <summary>
        /// 检具
        /// </summary>
        [Label("检具")]
        Checker = 2,

        /// <summary>
        /// 模具
        /// </summary>
        [Label("模具")]
        Mold = 3
    }
}
