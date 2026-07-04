using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 打印数据
    /// </summary>
    [Serializable]
    public class PrintDataCommon
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }
    }
}
