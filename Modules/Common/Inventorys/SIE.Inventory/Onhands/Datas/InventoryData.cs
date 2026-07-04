using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库存查询API交互信息
    /// </summary>
    public class InventoryData
    {
        /// <summary>
        /// 库存Id
        /// </summary>
        public double LotLpnOnhandId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料规格
        /// </summary>
        public string ItemSpecificationModel { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnitName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// LPN
        /// </summary>
        private string lpn;

        /// <summary>
        /// Lpn
        /// </summary>
        public string Lpn
        {
            get { return lpn.IsNullOrEmpty() ? "*" : lpn; }
            set { lpn = value; }
        }

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性展示名
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 现有数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal AvailableQty { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public string OnhandState { get; set; }

        /// <summary>
        /// 分配数量
        /// </summary>
        public decimal AllottedQty { get; set; }
    }

    /// <summary>
    /// 基础维度数据
    /// </summary>
    public class InventoryTypeData
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string SpecificationModel { get; set; }
    }
}
