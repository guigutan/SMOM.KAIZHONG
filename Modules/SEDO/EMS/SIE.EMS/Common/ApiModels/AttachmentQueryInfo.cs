using System;

namespace SIE.EMS.ApiModel
{
    /// <summary>
    /// 附件查询信息
    /// </summary>
    [Serializable]
    public class AttachmentQueryInfo
    {
        /// <summary>
        /// 计划单ID  
        /// </summary>
        public double PlanId { get; set; }
    }
}
