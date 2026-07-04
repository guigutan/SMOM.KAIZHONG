using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 开机准备获取任务单
    /// </summary>
    [Serializable]
    public class Pda_PreStartupSetupTaskInfo
    {
        /// <summary>
        /// 任务单Id
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 派工单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 产线(机台号)
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// 产线名称(机台名称)
        /// </summary>
        public string ResourceName { get; set; }

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
    }
}
