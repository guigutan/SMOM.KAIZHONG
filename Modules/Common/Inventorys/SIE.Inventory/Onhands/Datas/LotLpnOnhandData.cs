using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库存查询参数
    /// </summary>
    [Serializable]
    public class OnhandParam
    {
        /// <summary>
        /// 仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double LocId { get; set; }

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 项目
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
        /// 只匹配指定的项目号任务号
        /// </summary>
        public bool? IsAppoint { get; set; }
    }

    /// <summary>
    /// 人工拣货获取库存信息
    /// </summary>
    public class LotLpnOnhandData
    {
        /// <summary>
        /// 库存ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 库区Id
        /// </summary>
        public double AreaId { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 库位编号
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Lpn
        /// </summary>
        public string LPN { get; set; }

        /// <summary>
        /// 现有量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 可用量
        /// </summary>
        public decimal AvailableQty { get; set; }

        /// <summary>
        /// 分配量
        /// </summary>
        public decimal AllottedQty { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public string OnhandState { get; set; }

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public double LotId { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 指定批次属性01
        /// </summary>
        public DateTime? LotAtt01 { get; set; }

        /// <summary>
        /// 指定批次属性02
        /// </summary>
        public DateTime? LotAtt02 { get; set; }

        /// <summary>
        /// 指定批次属性03
        /// </summary>
        public DateTime? LotAtt03 { get; set; }

        /// <summary>
        /// 指定批次属性04
        /// </summary>
        public string LotAtt04 { get; set; }

        /// <summary>
        /// 指定批次属性05
        /// </summary>
        public decimal? LotAtt05 { get; set; }

        /// <summary>
        /// 指定批次属性06
        /// </summary>
        public decimal? LotAtt06 { get; set; }

        /// <summary>
        /// 指定批次属性07
        /// </summary>
        public bool? LotAtt07 { get; set; }

        /// <summary>
        /// 指定批次属性08
        /// </summary>
        public string LotAtt08 { get; set; }

        /// <summary>
        /// /指定批次属性09
        /// </summary>
        public string LotAtt09 { get; set; }

        /// <summary>
        /// 指定批次属性10
        /// </summary>
        public string LotAtt10 { get; set; }

        /// <summary>
        /// 指定批次属性11
        /// </summary>
        public DateTime? LotAtt11 { get; set; }

        /// <summary>
        /// 指定批次属性12
        /// </summary>
        public DateTime? LotAtt12 { get; set; }

        /// <summary>
        /// 指定LPN
        /// </summary>
        public string AppointLpn { get; set; }
    }
}
