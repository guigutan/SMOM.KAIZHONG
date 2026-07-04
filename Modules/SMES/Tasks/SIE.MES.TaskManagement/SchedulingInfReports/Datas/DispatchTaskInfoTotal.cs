using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfReports.Datas
{
    /// <summary>
    /// 排程级别的汇总信息（派工任务）
    /// </summary>
    public class DispatchTaskInfoTotal
    {
        /// <summary>
        /// 产线编码
        /// </summary>
        public string MachineCode { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string MachineName { get; set; }


        /// <summary>
        /// 是否已完全导入
        /// </summary>
        public YesNo IsImport { get; set; }      
        
        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty { get; set; }

        /// <summary>
        /// 已下发数量[任务单数量]
        /// </summary>
        public decimal YesGenerateTaskQty { get; set; }

        /// <summary>
        /// 未下发数量[MES排程导入中间表数量]
        /// </summary>
        public decimal NoGenerateTaskQty { get; set; }


        /// <summary>
        /// 工序已排程数量
        /// </summary>
        public decimal SchedulingQty { get; set; }
        /// <summary>
        /// 工序待排程数量
        /// </summary>
        public decimal WaitSchedulingQty { get; set; }
        /// <summary>
        /// 任务单状态
        /// </summary>
        public SIE.MES.TaskManagement.Dispatchs.DispatchTaskStatus? TaskStatus { get; set; }      

        /// <summary>
        /// 工序任务单生成数量
        /// </summary>
        public decimal ImportQty { get; set; }
        /// <summary>
        /// 是否排程退回 **取第一个**
        /// </summary>
        public YesNo IsSchedulingInfReturn { get; set; }
        /// <summary>
        /// 排程退回原因 **取第一个**
        /// </summary>
        public string SchedulingInfReturnReason { get; set; }

        /// <summary>
        /// 排程导入时间 **取第一个**
        /// </summary>
        public DateTime? ImportTime { get; set; }
        /// <summary>
        /// 检验是否通过
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 标准产能
        /// </summary>
        public decimal? StandardCapacity { get; set; }
        /// <summary>
        /// 是否已经下发
        /// </summary>
        public YesNo? IsGenerateTask { get; set; }

    }
}
