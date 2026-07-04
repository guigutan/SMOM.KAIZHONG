using SIE.ObjectModel;
using System;

namespace SIE.LES.RetreatItemManage.MaterialReturns
{
    /// <summary>
    /// 退料类型
    /// </summary>
    [Serializable]
    public enum ReturnTypes
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常退料")]
        Normal = 5,

        /// <summary>
        /// 不良
        /// </summary>
        [Label("不良退料")]
        Bad = 10,
    }
}
