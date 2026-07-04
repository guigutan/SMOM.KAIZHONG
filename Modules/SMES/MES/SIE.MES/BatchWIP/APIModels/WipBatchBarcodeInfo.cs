using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BatchWIP.APIModels
{
    /// <summary>
    /// 批次条码信息
    /// </summary>
    [Serializable]
    public class WipBatchBarcodeInfo
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 批次条码
        /// </summary>
        public string BatchBarcode { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public string CurrenProcess { get; set; }
    }

    /// <summary>
    /// 批次过站条码信息
    /// </summary>
    [Serializable]
    public class WipBatchMoveBarcodeInfo : WipBatchBarcodeInfo
    {

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal? CompleteNum { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal? ScrapNum { get; set; }
    }

}
