using System;

namespace SIE.EventMessages.MES.AgvTasks.Datas
{
    /// <summary>
    /// Agv任务数据
    /// </summary>
    public class AgvTaskData
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 起始仓库Id
        /// </summary>
        public double StartWhId { get; set; } 

        /// <summary>
        /// 起始库位Id
        /// </summary>
        public double StartLocationId { get; set; }

        /// <summary>
        /// 终止仓库
        /// </summary>
        public double EndWhId { get; set; }

        /// <summary>
        /// 终止库位
        /// </summary>
        public double EndLocationId { get; set; }

        /// <summary>
        /// 呼叫时间
        /// </summary>
        public DateTime CallTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime FinishTime { get; set; }
    }
}
