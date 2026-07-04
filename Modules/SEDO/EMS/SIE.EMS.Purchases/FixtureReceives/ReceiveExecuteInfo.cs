using System;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 设备接收扫描信息
    /// </summary>
    [Serializable]
    public class ReceiveExecuteInfo
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
        /// 序列号信息
        /// </summary>
        public FixtureReceiveSn SnInfo { get; set; }

        /// <summary>
        /// 是否是第一个条码
        /// </summary>
        public bool IsFirstSn { get; set; }
    }
}
