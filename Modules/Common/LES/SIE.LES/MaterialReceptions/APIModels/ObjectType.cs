using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.MaterialReceptions.APIModels
{
    /// <summary>
    /// 扫描对象类型
    /// </summary>
   [Serializable]
    public enum ObjectType
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
       ItemCode=1,

        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        Lot = 2,

        /// <summary>
        /// SN序列号
        /// </summary>
        [Label("物料标签序列号")]
        SN = 3,
    }
}
