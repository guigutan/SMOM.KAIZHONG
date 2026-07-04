using System;

namespace SIE.EMS.EquipRepair.ApiModels.ResultInfo
{
    /// <summary>
    /// 项目返回消息
    /// </summary>
    [Serializable]
    public class ProjectResultInfo
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
    }
}
