using System;

namespace SIE.ERPInterface.Common.Datas.EbsData
{
    /// <summary>
    /// 任务号
    /// </summary>
    [Serializable]
    public class TaskNoData : EbsDataBase
    {

        /// <summary>
        /// 任务号Id
        /// </summary>
        public string TASK_ID { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        public string Task_Number { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Task_Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        //public SIE.WMS.Common.TaskNoMaintain? Task { get; set; }
    }

}
