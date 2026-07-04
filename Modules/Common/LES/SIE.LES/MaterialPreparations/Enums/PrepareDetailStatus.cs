using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations.Enums
{
    /// <summary>
    /// 备料明细状态
    /// </summary>
    public enum PrepareDetailStatus
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Created = 0,

        /// <summary>
        /// 待发运
        /// </summary>
        [Label("待发运")]
        ToShipping = 1,

        /// <summary>
        /// 待接收
        /// </summary>
        [Label("待接收")]
        ToReceive = 2,

        /// <summary>
        /// 部分接收
        /// </summary>
        [Label("部分接收")]
        PartReceive = 3,

        /// <summary>
        /// 已接收
        /// </summary>
        [Label("已接收")]
        HasReceived = 4,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Canceled = 5,
    }
}
