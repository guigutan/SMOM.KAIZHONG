using System;

namespace SIE.EMS.API.APIModels
{
    /// <summary>
    /// 设备运行状态对应的前端路径
    /// </summary>
    [Serializable]
    public class RepairBillsOverviewParas
    {
        /// <summary>
        /// 报修Path
        /// </summary>
        public string ApplyRepairPath { get; set; }

        /// <summary>
        /// 待维修Path
        /// </summary>
        public string WaitRepairPath { get; set; }

        /// <summary>
        /// 维修中Path
        /// </summary>
        public string RepairingPath { get; set; }

        /// <summary>
        /// 暂停Path
        /// </summary>
        public string SuspendingPath { get; set; }

        /// <summary>
        /// 待确认Path
        /// </summary>
        public string WaitConfirmPath { get; set; }

        /// <summary>
        /// 工程评分Path
        /// </summary>
        public string WaitScorePath { get; set; }
    }
}