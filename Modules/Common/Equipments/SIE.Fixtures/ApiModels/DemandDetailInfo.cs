using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 需求明细信息
    /// </summary>
    [Serializable]
    public class DemandDetailInfo
    {
        /// <summary>
        /// 需求单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 治具编码ID
        /// </summary>
        public double EncodeCodeId { get; set; }


        /// <summary>
        /// 治具需求ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 治具编码
        /// </summary>
        public string EncodeCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public string DemandQty { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public string UnloadQty { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 管理方式
        /// </summary>
        public string ManageMode { get; set; }

    }
}
