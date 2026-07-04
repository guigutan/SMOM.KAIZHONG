using SIE.Common.Alert;
using System;
using System.Collections.Generic;

namespace SIE.AbnormalInfo.AbnormalMonitors.Pushers
{
    /// <summary>
    /// S异常推送参数类
    /// </summary>
    [Serializable]
    public class AbnormalMonitorAlertResult : AlertResultBase
    {
        /// <summary>
        /// 异常信息键值对集合
        /// </summary>
        public List<AbnormalMonitorPusher> AlertInfoList { get; set; }
    }

    /// <summary>
    /// 异常信息键值对
    /// </summary>
    [Serializable]
    public class AbnormalMonitorPusher
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
}
