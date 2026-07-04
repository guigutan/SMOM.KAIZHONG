using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ReworkLayoutVersions
{
    /// <summary>
    /// 返工信息状态
    /// </summary>
    public enum ReworkInfoRecordState
    {
        /// <summary>
        /// 新增
        /// </summary>
        [Label("新增")]
        Create = 1,

        /// <summary>
        /// 修改
        /// </summary>
        [Label("修改")]
        Edit = 2,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close = 3
    }
}
