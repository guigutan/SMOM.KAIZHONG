using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Projects.Enums
{
    /// <summary>
    /// 工序标准参数状态
    /// </summary>
    public enum ProcessStStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        [Label("新建")]
        Created = 0,

        /// <summary>
        /// 待审核
        /// </summary>
        [Label("待审核")]
        ToExamine = 1,

        /// <summary>
        /// 已审核
        /// </summary>
        [Label("已审核")]
        Examined = 2,
    }
}
