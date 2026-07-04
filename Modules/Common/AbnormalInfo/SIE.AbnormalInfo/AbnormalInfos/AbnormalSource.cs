using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常来源
    /// </summary>
    [Serializable]
    public enum AbnormalSource
    {
        /// <summary>
        /// 预警平台
        /// </summary>
        [Label("预警平台")]
        Alert = 0,

        /// <summary>
        /// 首检过程整改
        /// </summary>
        [Label("首检过程整改")]
        FirstInspection = 1,

        /// <summary>
        /// 巡检过程整改
        /// </summary>
        [Label("巡检过程整改")]
        PatrolInspBill = 2,

        /// <summary>
        /// ESD异常整改
        /// </summary>
        [Label("ESD异常整改")]
        EsdPatrolInspTask = 3,

        /// <summary>
        /// 设备点检
        /// </summary>
        [Label("设备点检")]
        EquipCheck = 4,

        /// <summary>
        /// 设备保养
        /// </summary>
        [Label("设备保养")]
        EquipMaintain = 5,
    }
}
