using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.Barcodes
{
    /// <summary>
    /// 条码报废后事件
    /// </summary>
    [Serializable]
    public class BarcodeScrapEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="barcodeInfos">条码集合</param>
        public BarcodeScrapEvent(List<BarcodeScrapInfo> barcodeInfos)
        {
            BarcodeInfos.AddRange(barcodeInfos);
        }

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<BarcodeScrapInfo> BarcodeInfos { get; } = new List<BarcodeScrapInfo>();
    }

    /// <summary>
    /// 拼板码报废后事件
    /// </summary>
    [Serializable]
    public class PanelManualScrapEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="barcodeInfos">条码集合</param>
        public PanelManualScrapEvent(List<BarcodeScrapInfo> barcodeInfos)
        {
            BarcodeInfos.AddRange(barcodeInfos);
        }

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<BarcodeScrapInfo> BarcodeInfos { get; } = new List<BarcodeScrapInfo>();
    }
}
