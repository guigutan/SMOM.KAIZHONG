using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Enums
{
    /// <summary>
    /// QT工序状态
    /// </summary>
    public enum QTProcessState
    {
        /// <summary>
        /// 开始
        /// </summary>
        [Label("开始")]
        Start = 0,

        /// <summary>
        /// 完成
        /// </summary>
        [Label("完成")]
        Finish = 1,

        /// <summary>
        /// 入站
        /// </summary>
        [Label("入站")]
        In = 2,

        /// <summary>
        /// 出站
        /// </summary>
        [Label("出站")]
        Out = 3,
    }
}
