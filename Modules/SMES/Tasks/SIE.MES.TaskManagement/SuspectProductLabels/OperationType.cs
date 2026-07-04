using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 作业类型
    /// </summary>
    public enum LabelType
    {
        /// <summary>
        /// 工序标签
        /// </summary>
        [Label("工序标签")]
        WipBatch = 0,
        /// <summary>
        /// 耐压SN
        /// </summary>
        [Label("耐压SN")]
        WipSn = 1,
    }
}
