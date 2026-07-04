using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 销售订单导入实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("销售订单导入实体")]
    public class SaleOrderReachViewModel : ViewModel
    {
        #region 销售订单编号 Code
        /// <summary>
        /// 销售订单编号
        /// </summary>
        [Label("销售订单编号")]
        public static readonly Property<string> CodeProperty = P<SaleOrderReachViewModel>.Register(e => e.Code);
        /// <summary>
        /// 销售订单编号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Required]
        [MaxLength(80)]
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<SaleOrderReachViewModel>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 销售人员  Employee
        /// <summary>
        /// 销售人员
        /// </summary>
        [Label("销售人员")]
        public static readonly Property<string> EmployeeProperty = P<SaleOrderReachViewModel>.Register(e => e.Employee);

        /// <summary>
        /// 销售人员
        /// </summary>
        public string Employee
        {
            get { return GetProperty(EmployeeProperty); }
            set { SetProperty(EmployeeProperty, value); }
        }
        #endregion

        #region 客户  Customer
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户")]
        public static readonly Property<string> CustomerProperty = P<SaleOrderReachViewModel>.Register(e => e.Customer);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string Customer
        {
            get { return GetProperty(CustomerProperty); }
            set { SetProperty(CustomerProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<SaleOrderReachViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [MaxLength(240)]
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<SaleOrderReachViewModel>.Register(e => e.ItemName);
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 版本号 Version
        /// <summary>
        /// 版本号
        /// </summary>
        [Required]
        [MaxLength(80)]
        [Label("版本号")]
        public static readonly Property<string> VersionProperty = P<SaleOrderReachViewModel>.Register(e => e.Version);

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get { return GetProperty(VersionProperty); }
            set { SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 行业类型 IndustryType
        /// <summary>
        /// 行业类型
        /// </summary>
        [Required]
        [Label("行业类型")]
        public static readonly Property<string> IndustryTypeProperty = P<SaleOrderReachViewModel>.Register(e => e.IndustryType);

        /// <summary>
        /// 行业类型
        /// </summary>
        public string IndustryType
        {
            get { return GetProperty(IndustryTypeProperty); }
            set { SetProperty(IndustryTypeProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Required]
        [Label("订单类型")]
        public static readonly Property<string> OrderTypeProperty = P<SaleOrderReachViewModel>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 产品类型 ProductType
        /// <summary>
        /// 产品类型
        /// </summary>
        [Required]
        [Label("产品类型")]
        public static readonly Property<string> ProductTypeProperty = P<SaleOrderReachViewModel>.Register(e => e.ProductType);

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType
        {
            get { return GetProperty(ProductTypeProperty); }
            set { SetProperty(ProductTypeProperty, value); }
        }
        #endregion

        #region 产品等级 ProductLevel
        /// <summary>
        /// 产品等级
        /// </summary>
        [Required]
        [Label("产品等级")]
        public static readonly Property<string> ProductLevelProperty = P<SaleOrderReachViewModel>.Register(e => e.ProductLevel);

        /// <summary>
        /// 产品等级
        /// </summary>
        public string ProductLevel
        {
            get { return GetProperty(ProductLevelProperty); }
            set { SetProperty(ProductLevelProperty, value); }
        }
        #endregion

        #region 特殊工艺 SpecialProcess
        /// <summary>
        /// 特殊工艺
        /// </summary>
        [Required]
        [Label("特殊工艺")]
        public static readonly Property<string> SpecialProcessProperty = P<SaleOrderReachViewModel>.Register(e => e.SpecialProcess);

        /// <summary>
        /// 特殊工艺
        /// </summary>
        public string SpecialProcess
        {
            get { return GetProperty(SpecialProcessProperty); }
            set { SetProperty(SpecialProcessProperty, value); }
        }
        #endregion

        #region 是否新单 IsNew
        /// <summary>
        /// 是否新单
        /// </summary>
        [Label("是否新单")]
        public static readonly Property<String> IsNewProperty = P<SaleOrderReachViewModel>.Register(e => e.IsNew);

        /// <summary>
        /// 是否新单
        /// </summary>
        public String IsNew
        {
            get { return GetProperty(IsNewProperty); }
            set { SetProperty(IsNewProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<SaleOrderReachViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<SaleOrderReachViewModel>.Register(e => e.Unit);

        /// <summary>
        /// Unit
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 库存组织代号 EnterpriseCode
        /// <summary>
        /// 库存组织代号
        /// </summary>
        [Label("库存组织代号")]
        public static readonly Property<string> EnterpriseCodeProperty = P<SaleOrderReachViewModel>.Register(e => e.EnterpriseCode);

        /// <summary>
        /// 库存组织代号
        /// </summary>
        public string EnterpriseCode
        {
            get { return this.GetProperty(EnterpriseCodeProperty); }
            set { SetProperty(EnterpriseCodeProperty, value); }
        }
        #endregion

        #region 库存组织名称 EnterpriseName
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [Label("库存组织名称")]
        public static readonly Property<string> EnterpriseNameProperty = P<SaleOrderReachViewModel>.Register(e => e.EnterpriseName);

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string EnterpriseName
        {
            get { return this.GetProperty(EnterpriseNameProperty); }
            set { SetProperty(EnterpriseNameProperty, value); }
        }
        #endregion

        #region MI完成时间 MiDateTime
        /// <summary>
        /// 完成时间
        /// </summary>
        [Label("MI完成时间")]
        public static readonly Property<DateTime> MiDateTimeProperty = P<SaleOrderReachViewModel>.Register(e => e.MiDateTime);

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime MiDateTime
        {
            get { return GetProperty(MiDateTimeProperty); }
            set { SetProperty(MiDateTimeProperty, value); }
        }
        #endregion

        #region 面积M2 Area
        /// <summary>
        /// 面积M2
        /// </summary>
        [Label("面积M2")]
        public static readonly Property<decimal> AreaProperty = P<SaleOrderReachViewModel>.Register(e => e.Area);

        /// <summary>
        /// 面积M2
        /// </summary>
        public decimal Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 大板尺寸 PlateSize
        /// <summary>
        /// 大板尺寸
        /// </summary>
        [Label("大板尺寸")]
        public static readonly Property<string> PlateSizeProperty = P<SaleOrderReachViewModel>.Register(e => e.PlateSize);

        /// <summary>
        /// 大板尺寸
        /// </summary>
        public string PlateSize
        {
            get { return GetProperty(PlateSizeProperty); }
            set { SetProperty(PlateSizeProperty, value); }
        }
        #endregion

        #region 开料PNL数 MaterialPnl
        /// <summary>
        /// 开料PNL数
        /// </summary>
        [Label("开料PNL数")]
        public static readonly Property<decimal> MaterialPnlProperty = P<SaleOrderReachViewModel>.Register(e => e.MaterialPnl);

        /// <summary>
        /// 开料PNL数
        /// </summary>
        public decimal MaterialPnl
        {
            get { return GetProperty(MaterialPnlProperty); }
            set { SetProperty(MaterialPnlProperty, value); }
        }
        #endregion

        #region SET/PNL数 SetPnl
        /// <summary>
        /// SET/PNL数
        /// </summary>
        [Label("SET/PNL数")]
        public static readonly Property<decimal> SetPnlProperty = P<SaleOrderReachViewModel>.Register(e => e.SetPnl);

        /// <summary>
        /// SET/PNL数
        /// </summary>
        public decimal SetPnl
        {
            get { return GetProperty(SetPnlProperty); }
            set { SetProperty(SetPnlProperty, value); }
        }
        #endregion

        #region PCS/PNL数 PcsPnl
        /// <summary>
        /// PCS/PNL数
        /// </summary>
        [Label("PCS/PNL数")]
        public static readonly Property<decimal> PcsPnlProperty = P<SaleOrderReachViewModel>.Register(e => e.PcsPnl);

        /// <summary>
        /// PCS/PNL数
        /// </summary>
        public decimal PcsPnl
        {
            get { return GetProperty(PcsPnlProperty); }
            set { SetProperty(PcsPnlProperty, value); }
        }
        #endregion

        #region 客户交期 RequireDelivery
        /// <summary>
        /// 客户交期
        /// </summary>
        [Label("客户交期")]
        public static readonly Property<DateTime> RequireDeliveryProperty = P<SaleOrderReachViewModel>.Register(e => e.RequireDelivery);

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery
        {
            get { return GetProperty(RequireDeliveryProperty); }
            set { SetProperty(RequireDeliveryProperty, value); }
        }
        #endregion

        #region 承诺交期 PromiseDelivery
        /// <summary>
        /// 承诺交期
        /// </summary>
        [Label("承诺交期")]
        public static readonly Property<DateTime?> PromiseDeliveryProperty = P<SaleOrderReachViewModel>.Register(e => e.PromiseDelivery);

        /// <summary>
        /// 承诺交期
        /// </summary>
        public DateTime? PromiseDelivery
        {
            get { return GetProperty(PromiseDeliveryProperty); }
            set { SetProperty(PromiseDeliveryProperty, value); }
        }
        #endregion

        #region 订单行挂起 IsHangUp
        /// <summary>
        /// 订单行挂起
        /// </summary>
        [Label("订单行挂起")]
        public static readonly Property<String> IsHangUpProperty = P<SaleOrderReachViewModel>.Register(e => e.IsHangUp);

        /// <summary>
        /// 订单行挂起
        /// </summary>
        public String IsHangUp
        {
            get { return GetProperty(IsHangUpProperty); }
            set { SetProperty(IsHangUpProperty, value); }
        }
        #endregion

        #region 行状态 LineState
        /// <summary>
        /// 行状态
        /// </summary>
        [Label("行状态")]
        public static readonly Property<String> LineStateProperty = P<SaleOrderReachViewModel>.Register(e => e.LineState);

        /// <summary>
        /// 行状态
        /// </summary>
        public String LineState
        {
            get { return GetProperty(LineStateProperty); }
            set { SetProperty(LineStateProperty, value); }
        }
        #endregion
    }
}
