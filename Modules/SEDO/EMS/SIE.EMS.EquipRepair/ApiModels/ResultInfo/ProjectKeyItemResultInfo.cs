using System;

namespace SIE.EMS.EquipRepair.ApiModels.ResultInfo
{
    /// <summary>
    /// 项目事项返回消息
    /// </summary>
    [Serializable]
    public class ProjectKeyItemResultInfo
    {
        /// <summary>
        /// 项目事项Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 事项说明
        /// </summary>
        public string Description { get; set; }
    }
}
