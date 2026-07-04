using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Datas
{
    /// <summary>
    /// 信息
    /// </summary>
    [Serializable]
    public class PdaPgInfo
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 派工
        /// </summary>
        public string PgName { get; set; }
    }
}
