using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations.ApiModels
{
    /// <summary>
    /// 导出选项
    /// </summary>
    public enum MpExportInfo
    {
        /// <summary>
        /// 当前页
        /// </summary>
        Current = 0,

        /// <summary>
        /// 选中行
        /// </summary>
        Selected = 1,

        /// <summary>
        /// 查询结果
        /// </summary>
        All = 2,
    }
}
