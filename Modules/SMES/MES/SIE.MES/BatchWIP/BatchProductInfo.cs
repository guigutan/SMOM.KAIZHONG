using SIE.MES.WIP;
using System;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 批次产品信息
    /// </summary>
    [Serializable]
    public class BatchProductInfo : ProductInfo
    {
        /// <summary>
        /// 转入批次
        /// </summary>
        public InputBatch InputBatch { get; set; }
    }
}