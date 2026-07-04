using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    [Serializable]
    public class Pda_AssemblyGetTaskInfo
    {
        /// <summary>
        /// 工序BomId
        /// </summary>
        public virtual double ProcessBomId { get; set; }

        /// <summary>
        /// 派工任务单Id
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string OldItemCode { get; set; }

    }
}
