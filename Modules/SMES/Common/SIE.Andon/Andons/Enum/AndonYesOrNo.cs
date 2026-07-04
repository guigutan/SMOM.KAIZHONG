using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Enum
{
    /// <summary>
    /// 是否停线、叫料枚举
    /// </summary>
    public enum AndonYesOrNo
    {
        /// <summary>
        /// 是
        /// </summary>
        [Label("是")]
        Yes = 10,

        /// <summary>
        /// 否
        /// </summary>
        [Label("否")]
        No = 20,

        /// <summary>
        /// 人工
        /// </summary>
        [Label("人工")]
        Artificial = 30
    }
}
