using SIE.Barcodes;
using System;

namespace SIE.MES.WIP.Models
{
    /// <summary>
    /// 条码打印信息
    /// </summary>
    [Serializable]
    public class BarcodePrintInfo : PrinterInfo
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        public string TemplateType { get; set; }

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer { get; set; }
    }
}
