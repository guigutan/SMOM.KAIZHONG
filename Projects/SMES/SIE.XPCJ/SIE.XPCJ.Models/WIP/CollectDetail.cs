using SIE.XPCJ.Models.Enums;
using System;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 采集记录明细
    /// </summary>
    [Serializable]
    public class CollectDetail
    {
        /// <summary>
        /// 采集条码
        /// </summary>

        public string Barcode { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>

        public string BarcodeType { get; set; }

        /// <summary>
        /// 采集结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public string CollectDate { get; set; }
    }




}

