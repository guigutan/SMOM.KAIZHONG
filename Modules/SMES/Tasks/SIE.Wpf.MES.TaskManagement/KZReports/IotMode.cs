using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.TaskManagement.KZReports
{
    /// <summary>
    /// IOT数采模式
    /// </summary>
    public enum IotMode
    {
        /// <summary>
        /// 正常 (冲压)
        /// </summary>
        [Label("正常")]
        Normal = 0,
        /// <summary>
        /// 押出
        /// </summary>
        [Label("押出")]
        Extrusion = 1,
        /// <summary>
        /// 共模
        /// </summary>
        [Label("共模")]
        CommonMode = 2,

        /// <summary>
        /// 多工位
        /// </summary>
        [Label("多工位")]
        MultiStation = 3,

        /// <summary>
        /// 过程数采
        /// </summary>
        [Label("过程数采")]
        Process = 4
        
    }
}
