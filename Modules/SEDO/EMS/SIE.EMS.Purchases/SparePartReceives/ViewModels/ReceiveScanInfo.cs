using System;

namespace SIE.EMS.Purchases.SparePartReceives.ViewModels
{
    /// <summary>
    /// 备件接收扫描信息
    /// </summary>
    [Serializable]
    public class ReceiveScanInfo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否是第一个条码
        /// </summary>
        public bool IsFirstSn { get; set; }

        /// <summary>
        /// 批次信息
        /// </summary>
        public SparePartReceiveLot LotInfo { get; set; }

        /// <summary>
        /// 序列号信息
        /// </summary>
        public SparePartReceiveSn SnInfo { get; set; }
    }
}
