using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 历史订单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(HistoryOrderViewModelCriteria))]
    [Label("历史订单")]
    public class HistoryOrderViewModel : ViewModel
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<HistoryOrderViewModel>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> DepartmentProperty = P<HistoryOrderViewModel>.Register(e => e.Department);

        /// <summary>
        /// 部门
        /// </summary>
        public string Department
        {
            get { return this.GetProperty(DepartmentProperty); }
            set { this.SetProperty(DepartmentProperty, value); }
        }
        #endregion

        #region 采购订单号 OrderNo
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> OrderNoProperty = P<HistoryOrderViewModel>.Register(e => e.OrderNo);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string OrderNo
        {
            get { return GetProperty(OrderNoProperty); }
            set { SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<HistoryOrderViewModel>.Register(e => e.SupplierCode);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
            set { this.SetProperty(SupplierCodeProperty, value); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<HistoryOrderViewModel>.Register(e => e.SupplierName);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
            set { this.SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<HistoryOrderViewModel>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 采购对象编码 ObjectCode
        /// <summary>
        /// 采购对象编码
        /// </summary>
        [Label("采购对象编码")]
        public static readonly Property<string> ObjectCodeProperty = P<HistoryOrderViewModel>.Register(e => e.ObjectCode);

        /// <summary>
        /// 采购对象编码
        /// </summary>
        public string ObjectCode
        {
            get { return this.GetProperty(ObjectCodeProperty); }
            set { this.SetProperty(ObjectCodeProperty, value); }
        }
        #endregion

        #region 采购描述 Description
        /// <summary>
        /// 采购描述
        /// </summary>
        [Label("采购描述")]
        public static readonly Property<string> DescriptionProperty = P<HistoryOrderViewModel>.Register(e => e.Description);

        /// <summary>
        /// 采购描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
            set { this.SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<HistoryOrderViewModel>.Register(e => e.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
            set { this.SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 采购数量 Qty
        /// <summary>
        /// 采购数量
        /// </summary>
        [Label("采购数量")]
        public static readonly Property<int> QtyProperty = P<HistoryOrderViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 采购数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<HistoryOrderViewModel>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 单价(含税) Price
        /// <summary>
        /// 单价(含税)
        /// </summary>
        [Label("单价(含税)")]
        public static readonly Property<decimal> PriceProperty = P<HistoryOrderViewModel>.Register(e => e.Price);

        /// <summary>
        /// 单价(含税)
        /// </summary>
        public decimal Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }
        #endregion

        #region 税率(%) TaxRate
        /// <summary>
        /// 税率(%)
        /// </summary>
        [Label("税率(%)")]
        public static readonly Property<decimal> TaxRateProperty = P<HistoryOrderViewModel>.Register(e => e.TaxRate);

        /// <summary>
        /// 税率(%)
        /// </summary>
        public decimal TaxRate
        {
            get { return this.GetProperty(TaxRateProperty); }
            set { this.SetProperty(TaxRateProperty, value); }
        }
        #endregion

        #region 单价(不含税) PriceNoTax
        /// <summary>
        /// 单价(不含税)
        /// </summary>
        [Label("单价(不含税)")]
        public static readonly Property<decimal> PriceNoTaxProperty = P<HistoryOrderViewModel>.Register(e => e.PriceNoTax);

        /// <summary>
        /// 单价(不含税)
        /// </summary>
        public decimal PriceNoTax
        {
            get { return this.GetProperty(PriceNoTaxProperty); }
            set { this.SetProperty(PriceNoTaxProperty, value); }
        }
        #endregion

        #region 总价 Amount
        /// <summary>
        /// 总价
        /// </summary>
        [Label("总价")]
        public static readonly Property<decimal> AmountProperty = P<HistoryOrderViewModel>.Register(e => e.Amount);

        /// <summary>
        /// 总价
        /// </summary>
        public decimal Amount
        {
            get { return this.GetProperty(AmountProperty); }
            set { this.SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<HistoryOrderViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 采购时间 PurchaseDate
        /// <summary>
        /// 采购时间
        /// </summary>
        [Label("采购时间")]
        public static readonly Property<DateTime> PurchaseDateProperty = P<HistoryOrderViewModel>.Register(e => e.PurchaseDate);

        /// <summary>
        /// 采购时间
        /// </summary>
        public DateTime PurchaseDate
        {
            get { return this.GetProperty(PurchaseDateProperty); }
            set { this.SetProperty(PurchaseDateProperty, value); }
        }
        #endregion
    }
}
