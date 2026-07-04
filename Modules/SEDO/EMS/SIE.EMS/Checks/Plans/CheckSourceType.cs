using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
    /// 点检(保养)来源方式
    /// </summary>
    public enum CheckSourceType
    {
        /// <summary>
        /// 计划
        /// </summary>
        [Label("计划")]
        PLAN = 10,

        /// <summary>
        /// PDA新建
        /// </summary>
        [Label("PDA新建")]
        PDA = 20,

        /// <summary>
        /// 接口
        /// </summary>
        [Label("接口")]
        Interface = 30,

        /// <summary>
        /// 新建
        /// </summary>
        [Label("新建")]
        NewCreated = 40,
    }
}
