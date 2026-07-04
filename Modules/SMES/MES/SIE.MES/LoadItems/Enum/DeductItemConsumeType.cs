using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 扣料记录消耗方式
    /// </summary>
    public enum DeductItemConsumeType
    {
        /// <summary>
        /// 拉式
        /// </summary>
        [Label("拉式")]
        Pull = 10,

        /// <summary>
        /// 推式
        /// </summary>
        [Label("推式")]
        Push = 20,
    }
}