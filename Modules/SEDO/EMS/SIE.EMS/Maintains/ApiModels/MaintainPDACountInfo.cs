using SIE.EMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养统计信息
    /// </summary>
    [Serializable]
    public class MaintainPDACountInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 保养状态
        /// </summary>
        public MaintExeState MaintExeState { get; set; }

        /// <summary>
        /// 责任部门Id
        /// </summary>
        public double? DepartmentId { get; set; }

        /// <summary>
        /// 确认部门Id
        /// </summary>
        public double ConfirmDptId { get; set; }
    }
}
