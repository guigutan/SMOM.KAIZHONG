using SIE.Common.Alert;
using System;
using System.Collections.Generic;

namespace SIE.Equipments.Abnormal.SysSenders
{
    /// <summary>
    /// 异常停线结果
    /// </summary>
    [Serializable]
    public class AbnormalCauseResult : AlertResultBase
    {
        /// <summary>
        /// 停线产线集合
        /// </summary>
        public List<double> LineIdList { get; set; }

        /// <summary>
        /// 停线设备集合
        /// </summary>
        public List<double> EquipAccountIdList { get; set; }

    }
}
