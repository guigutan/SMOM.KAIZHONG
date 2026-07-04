using System;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// 发运单数据
    /// </summary>
    [Serializable]
    public class SoData
    {
        /// <summary>
        /// 发运单ID
        /// </summary>
        public double ShippingOrderId { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string ShippingOrderNo { get; set; }

        /// <summary>
        /// 单据状态
        /// </summary>
        public string OrderState { get; set; }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public string ShippingWarehouseName { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime ShippingDate { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustName { get; set; }

        /// <summary>
        /// 生产部门
        /// </summary>
        public string ProdDeptName { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int PriorityType { get; set; }
    }

    /// <summary>
    /// 发运单明细数据
    /// </summary>
    [Serializable]
    public class SoDetailData
    {
        /// <summary>
        /// 发运单ID
        /// </summary>
        public double ShippingOrderId { get; set; }

        /// <summary>
        /// 是否部分发货
        /// </summary>
        public bool IsPartShip { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string ShippingOrderNo { get; set; }

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 相关单号行号
        /// </summary>
        public string OrderLineNo { get; set; }

        /// <summary>
        /// 发运单明细ID
        /// </summary>
        public double SoDetailId { get; set; }

        /// <summary>
        /// 发运单明细行号
        /// </summary>
        public string SoDetailLineNo { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

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
        public string ItemSpec { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool ItemEnableExtendProperty { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        public decimal? ShippingQty { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// 生产部门
        /// </summary>
        public string ProdDept { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? ShippingDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateByName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal ExceptQty { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 分子
        /// </summary>
        public decimal Numerator
        {
            get;
            set;
        }

        /// <summary>
        /// 分母
        /// </summary>
        public decimal Denominator
        {
            get;
            set;
        }

        /// <summary>
        /// 转换率
        /// </summary>
        public decimal ConvertFigre
        {
            get;
            set;
        }

        /// <summary>
        /// 辅助单位
        /// </summary>
        public double SecondUnitId { get; set; }

        /// <summary>
        /// 辅助单位
        /// </summary>
        public string SecondUnitName { get; set; }

        /// <summary>
        /// 辅助精度
        /// </summary>
        public int SecondPrecision { get; set; } = 3;

        public int MainPrecision { get; set; } = 3;
        
        public int MainTrade { get; set; }

        public int SecondTrade { get; set; }

        /// <summary>
        /// 委外BOMId
        /// </summary>
        public double? PurOrderDtlBomId { get; set; }
    }
}
