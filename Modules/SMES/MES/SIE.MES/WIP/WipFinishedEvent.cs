using SIE.MES.BatchWIP;
using SIE.MES.WIP.Runtime;
using System;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 在制品完成
    /// </summary>
    [Serializable]
    public class WipFinishedEvent
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="product">生产采集运行时产品</param>
        /// <param name="barcode">单体条码</param>
        /// <param name="collectionDate">采集时间</param>
        /// <param name="OutputBatch"></param>
        public WipFinishedEvent(product product, string barcode, DateTime collectionDate, OutputBatch OutputBatch)  {
            this.Product = product;
            this.Barcode = barcode;
            this.CollectionDate = collectionDate;
            this.OutputBatch = OutputBatch;
        }

        /// <summary>
        /// 生产采集运行时产品
        /// </summary>
        public product Product { get; set; }

        /// <summary>
        /// 单体条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionDate { get; set; }

        /// <summary>
        /// 转出批次列表
        /// </summary>
        public OutputBatch OutputBatch { get; set; }

    }
}
