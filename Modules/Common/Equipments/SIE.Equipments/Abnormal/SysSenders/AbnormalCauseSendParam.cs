using SIE.Common.Sender;
using System.Collections.Generic;

namespace SIE.Equipments.Abnormal.SysSenders
{
    /// <summary>
    /// 异常停线发送参数
    /// </summary>
    public class AbnormalCauseSendParam : ISendParam
    {
        /// <summary>
        /// 停线产线集合
        /// </summary>
        public List<double> LineIdList { get; set; }

        /// <summary>
        /// 停线设备集合
        /// </summary>
        public List<double> EquipAccountIdList { get; set; }

        /// <summary>
        /// 预警配置ID
        /// </summary>
        public double? AlerterId { get; set; }

        /// <summary>
        /// 预警管理ID
        /// </summary>
        public double? AlertManageId { get; set; }
    }
}
