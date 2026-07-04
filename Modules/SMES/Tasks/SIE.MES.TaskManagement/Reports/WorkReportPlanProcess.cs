using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkReportPlanProcess
    {
        /// <summary>
        /// 
        /// </summary>
        public double WorkReportPlanId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNeedCheck { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? ProcsessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
