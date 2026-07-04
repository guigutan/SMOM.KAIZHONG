using SIE.EMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// PDA首页点检统计
    /// </summary>
    [Serializable]
    public class CheckPDACountInfo
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 责任部门Id
        /// </summary>
        public double? DepartmentId { get; set; }

        /// <summary>
        /// 确认部门Id
        /// </summary>
        public double ConfirmDptId { get; set; }

        /// <summary>
        /// 点检记录状态
        /// </summary>
        public CheckExeState CheckExeState { get; set; }
    }
}
