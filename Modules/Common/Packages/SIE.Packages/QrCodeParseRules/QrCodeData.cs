using System;

namespace SIE.Packages.QrCodeParseRules
{
    /// <summary>
    /// 返回前端二维码解析后数据
    /// </summary>
    [Serializable]
    public class QrCodeRstData
    {
        /// <summary>
        /// 二维码KEY
        /// </summary>
        public string QrCodeKey { get; set; }

        /// <summary>
        /// 二维码字段名称
        /// </summary>
        public string QrCodeName { get; set; }

        /// <summary>
        /// 二维码解析数据
        /// </summary>
        public string QrCodeValue { get; set; }

        /// <summary>
        /// 二维码KEY值
        /// </summary>
        public int QrCodeKeyVal { get; set; }

        /// <summary>
        /// 解析栏位
        /// </summary>
        public ParseField ParseField { get; set; }
    }
}
