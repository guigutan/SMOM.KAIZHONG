using System;

namespace SIE.Barcodes.Barcodes.ViewModels
{
    /// <summary>
    /// 条码打印返回结果
    /// </summary>
    [Serializable]
    public class RstBarcodePrint
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }
    }
}
