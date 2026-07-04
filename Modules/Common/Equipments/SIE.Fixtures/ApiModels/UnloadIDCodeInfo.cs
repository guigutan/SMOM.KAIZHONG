using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 出库ID编码信息
    /// </summary>
    [Serializable]
    public class UnloadIDCodeInfo : ResultDataInfo
    {
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
        /// 管理方式
        /// </summary>
        public string ManageMode { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? LocationId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 可直接提交
        /// </summary>
        public bool CanSumit { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public int DemanQty { get; set; }

        /// <summary>
        /// ID类编码
        /// </summary>
        public string IdCode { get; set; }

        /// <summary>
        /// 明细ID
        /// </summary>
        public double DetailId { get; set; }
    }
}
