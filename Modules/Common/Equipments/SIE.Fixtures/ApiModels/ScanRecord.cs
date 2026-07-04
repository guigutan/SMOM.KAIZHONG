using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 扫描记录
    /// </summary>
    [Serializable]
    public class ScanRecord
    {
        /// <summary>
        ///工治具ID/编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// RFID
        /// </summary>
        public string RFID { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 载具
        /// </summary>
        public string TurnoverToolCode { get; set; }

        /// <summary>
        /// 本次出库数
        /// </summary>
        public string Qty { get; set; }
    }
}
