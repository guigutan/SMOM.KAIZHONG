using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{    
    /// <summary>
     /// 用户状态
     /// </summary>
    public enum UserState
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Label("全部")]
        All,

        /// <summary>
        /// 可用
        /// </summary>
        [Label("可用")]
        Enable,

        /// <summary>
        /// 禁用
        /// </summary>
        [Label("禁用")]
        Disable,
    }
}
