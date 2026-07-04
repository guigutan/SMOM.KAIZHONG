using SIE.Core.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.Reports
{
    /// <summary>
    /// 项目号工单信息
    /// </summary>
    [Serializable]
    public class ProjectWoInfo
    {
        /// <summary>
        /// 项目号Id
        /// </summary>
        public double ProjectMaintainId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState State { get; set; }
    }
}
