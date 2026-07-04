using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.Enums
{
    /// <summary>
    /// 生产状态
    /// </summary>
    public enum ProduceStatus
    {
        /// <summary>
        /// 未排产
        /// </summary>
        [Label("未排产")]
        UnProduct = 0,

        /// <summary>
        /// 待生产
        /// </summary>
        [Label("待生产")]
        ToProduct = 1,

        /// <summary>
        /// 生产中
        /// </summary>
        [Label("生产中")]
        Producting = 2,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Complete = 3,
    }
}
