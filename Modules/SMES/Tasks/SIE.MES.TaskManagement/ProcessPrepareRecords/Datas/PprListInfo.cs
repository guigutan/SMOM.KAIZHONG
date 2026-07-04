using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.ProcessPrepareRecords.Datas
{
    /// <summary>
    /// 产前准备任务列表
    /// </summary>
    [Serializable]
    public class PprListInfo
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }
        
        /// <summary>
        /// 派工单号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public string WipResource { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId { get; set; }

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
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public string PlanBeginTime { get; set; }
    }
}
