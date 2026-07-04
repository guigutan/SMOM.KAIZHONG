using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.Enums
{
    /// <summary>
    /// 设计状态
    /// </summary>
    public enum DesignStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        [Label("新建")]
        Create = 0,

        /// <summary>
        /// 设计中
        /// </summary>
        [Label("设计中")]
        DesignIng = 1,

        /// <summary>
        /// 待审核
        /// </summary>
        [Label("待审核")]
        ToExamine = 2,

        /// <summary>
        /// 设计完成
        /// </summary>
        [Label("设计完成")]
        Complete = 3,
    }
}
