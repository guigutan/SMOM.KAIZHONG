using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 叫料单配送超时预警参数类
    /// </summary>
    [Serializable]
    public class DeliveryDelayAlertResult : CallMaterialAlertResult
    {

        /// <summary>
        /// 工位编号
        /// </summary>
        [Label("工位编码")]
        public string StationCode { get; set; }

        /// <summary>
        /// 叫料单号
        /// </summary>
        [Label("叫料单号")]
        public string CallMaterialBillNo { get; set; }
    }

    /// <summary>
    /// 叫料单配送超时预警参数集合类
    /// </summary>
    public class DeliveryDelayAlertResultList : CallMaterialAlertResultList
    {
    }
}