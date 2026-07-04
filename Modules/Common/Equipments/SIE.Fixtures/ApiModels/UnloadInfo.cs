using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 出库明细信息
    /// </summary>
    [Serializable]
    public class UnloadInfo
    {
        /// <summary>
        /// ID编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 载具
        /// </summary>
        public string TurnoverToolCode { get; set; }

        /// <summary>
        /// 出库数
        /// </summary>
        public int UnloadQty { get; set; }
    }
}