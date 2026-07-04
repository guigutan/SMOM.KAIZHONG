using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Enum
{
    /// <summary>
    /// 安灯类型维护安灯大类枚举
    /// </summary>
    public enum AndonTypeClass
    {
        /// <summary>
        /// 人
        /// </summary>
        [Label("人")]
        Person = 10,

        /// <summary>
        /// 机
        /// </summary>
        [Label("机")]
        Machine = 20,

        /// <summary>
        /// 料
        /// </summary>
        [Label("料")]
        Material = 30,

        /// <summary>
        /// 法
        /// </summary>
        [Label("法")]
        Method = 40,
        
        /// <summary>
        /// 环
        /// </summary>
        [Label("环")]
        Ring = 50,
        
        /// <summary>
        /// 测
        /// </summary>
        [Label("测")]
        Test = 60,
    }
}
