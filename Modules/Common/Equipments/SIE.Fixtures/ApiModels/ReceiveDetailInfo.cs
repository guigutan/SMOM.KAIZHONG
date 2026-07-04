using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 工治具领用清单信息
    /// </summary>
    [Serializable]
    public class ReceiveDetailInfo
    {
        /// <summary>
        /// 是否选中:0不选中，1选中
        /// </summary>
        public int IsSelect { get; set; }

        /// <summary>
        /// 需求单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 载具编号
        /// </summary>
        public string ToolCode { get; set; }

        /// <summary>
        /// ID编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 治具编码
        /// </summary>
        public string EncodeCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// RFID
        /// </summary>
        public string RFID { get; set; }

        /// <summary>
        /// 保养状态
        /// </summary>
        public string MaintainState { get; set; }

    }
}
