using SIE.Inventory.Onhands;

namespace SIE.Inventory
{
    /// <summary>
    /// 包装库存数据
    /// </summary>
    public class PackOnhandDataBase
    {
        /// <summary>
        /// 库存Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 最上级包装号
        /// </summary>
        public string PackageNo { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string LocCode { get; set; }

        /// <summary>
        /// 是否立库
        /// </summary>
        public bool IsAutomatedArea { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double LocId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 库区ID
        /// </summary>
        public double AreaId { get; set; }

        /// <summary>
        /// 批次ID
        /// </summary>
        public double? LotId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public string OnhandState { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState OnhandStateValue { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public string OnhandStateName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 分配数量
        /// </summary>
        public decimal AllotQty { get; set; }

        /// <summary>
        /// 冻结数量
        /// </summary>
        public decimal FreezingQty { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnitName { get; set; }

        /// <summary>
        /// 包装是否已被拣货
        /// </summary>
        public bool IsPicked { get; set; }

        /// <summary>
        /// 序列号管理
        /// </summary>
        public bool IsSerialNumber { get; set; }        

        /// <summary>
        /// 是否位置跟踪
        /// </summary>
        public bool IsLocation { get; set; }

    }
}
