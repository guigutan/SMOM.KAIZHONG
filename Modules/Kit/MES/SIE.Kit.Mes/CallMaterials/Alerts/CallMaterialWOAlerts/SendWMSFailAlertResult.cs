using System;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 工单叫料发送WMS失败预警参数类
    /// </summary>
    [Serializable]
    public class SendWMSFailAlertResult : CallMaterialAlertResult
    {

        /// <summary>
        /// 配送周期
        /// </summary>
        public decimal? SendingHours { get; set; }

        /// <summary>
        /// 发送失败原因 (异常原因)
        /// </summary>
        public string FailReason { get; set; }
    }

    /// <summary>
    /// 工单叫料发送WMS失败预警参数集合类
    /// </summary>
    public class SendWMSFailAlertResultList : CallMaterialAlertResultList
    {
    }
}