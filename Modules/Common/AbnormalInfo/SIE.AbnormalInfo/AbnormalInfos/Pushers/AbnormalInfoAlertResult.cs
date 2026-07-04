using SIE.Common.Alert;
using System;
using System.Collections.Generic;

namespace SIE.AbnormalInfo.AbnormalInfos.Pushers
{
    /// <summary>
    /// SPC异常推送参数类
    /// </summary>
    [Serializable]
    public class AbnormalInfoAlertResult : AlertResultBase
    {
        /// <summary>
        /// 异常信息键值对集合
        /// </summary>
        public List<AbnormalInfoPusher> AlertInfoList { get; set; }
    }

    /// <summary>
    /// 异常信息键值对
    /// </summary>
    [Serializable]
    public class AbnormalInfoPusher
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
