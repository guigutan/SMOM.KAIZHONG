using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 采购订单明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("采购订单明细")]
    [DisplayMember(nameof(LineNo))]
    public partial class PurchaseOrderItem : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<PurchaseOrderItem>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 状态 Status
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<PurchaseOrderStatus> StatusProperty = P<PurchaseOrderItem>.Register(e => e.Status);

        /// <summary>
        /// 状态
        /// </summary>
        public PurchaseOrderStatus Status
        {
            get { return this.GetProperty(StatusProperty); }
            set { this.SetProperty(StatusProperty, value); }
        }
        #endregion

        #region 采购数量 Qty
        /// <summary>
        /// 采购数量
        /// </summary>
        [Label("采购数量")]
        [MinValue(1)]
        public static readonly Property<int> QtyProperty = P<PurchaseOrderItem>.Register(e => e.Qty);

        /// <summary>
        /// 采购数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 单价(含税) Price
        /// <summary>
        /// 单价(含税)
        /// </summary>
        [Label("单价(含税)")]
        [MinValue(0.01)]
        public static readonly Property<decimal> PriceProperty = P<PurchaseOrderItem>.Register(e => e.Price);

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
        [MinValue(0)]
        [MaxValue(100)]
        public static readonly Property<decimal> TaxRateProperty = P<PurchaseOrderItem>.Register(e => e.TaxRate);

        /// <summary>
        /// 税率(%)
        /// </summary>
        public decimal TaxRate
        {
            get { return this.GetProperty(TaxRateProperty); }
            set { this.SetProperty(TaxRateProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        [Required]
        public static readonly Property<DateTime> DeliveryDateProperty = P<PurchaseOrderItem>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate
        {
            get { return GetProperty(DeliveryDateProperty); }
            set { SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 接收数量 ReciveQty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        public static readonly Property<decimal> ReciveQtyProperty = P<PurchaseOrderItem>.Register(e => e.ReciveQty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public decimal ReciveQty
        {
            get { return GetProperty(ReciveQtyProperty); }
            set { SetProperty(ReciveQtyProperty, value); }
        }
        #endregion


        #region 累计接收数 TotalReciveQty
        /// <summary>
        /// 累计接收数
        /// </summary>
        [Label("累计接收数")]
        public static readonly Property<decimal> TotalReciveQtyProperty = P<PurchaseOrderItem>.Register(e => e.TotalReciveQty);

        /// <summary>
        /// 累计接收数
        /// </summary>
        public decimal TotalReciveQty
        {
            get { return this.GetProperty(TotalReciveQtyProperty); }
            set { this.SetProperty(TotalReciveQtyProperty, value); }
        }
        #endregion

        #region 累计验收数 TotalAcceptanceQty
        /// <summary>
        /// 累计验收数
        /// </summary>
        [Label("累计验收数")]
        public static readonly Property<decimal> TotalAcceptanceQtyProperty = P<PurchaseOrderItem>.Register(e => e.TotalAcceptanceQty);

        /// <summary>
        /// 累计验收数
        /// </summary>
        public decimal TotalAcceptanceQty
        {
            get { return this.GetProperty(TotalAcceptanceQtyProperty); }
            set { this.SetProperty(TotalAcceptanceQtyProperty, value); }
        }
        #endregion


        #region 验收数量 AcceptanceQty
        /// <summary>
        /// 验收数量
        /// </summary>
        [Label("验收数量")]
        public static readonly Property<decimal> AcceptanceQtyProperty = P<PurchaseOrderItem>.Register(e => e.AcceptanceQty);

        /// <summary>
        /// 验收数量
        /// </summary>
        public decimal AcceptanceQty
        {
            get { return GetProperty(AcceptanceQtyProperty); }
            set { SetProperty(AcceptanceQtyProperty, value); }
        }
        #endregion

        #region 拒收数量 RejectQty
        /// <summary>
        /// 拒收数量
        /// </summary>
        [Label("拒收数量")]
        public static readonly Property<decimal> RejectQtyProperty = P<PurchaseOrderItem>.Register(e => e.RejectQty);

        /// <summary>
        /// 拒收数量
        /// </summary>
        public decimal RejectQty
        {
            get { return GetProperty(RejectQtyProperty); }
            set { SetProperty(RejectQtyProperty, value); }
        }
        #endregion

        #region 入库数量 InboundQty
        /// <summary>
        /// 入库数量
        /// </summary>
        [Label("入库数量")]
        public static readonly Property<decimal> InboundQtyProperty = P<PurchaseOrderItem>.Register(e => e.InboundQty);

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal InboundQty
        {
            get { return GetProperty(InboundQtyProperty); }
            set { SetProperty(InboundQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<PurchaseOrderItem>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 采购申请明细 PurchaseRequisitionItem
        /// <summary>
        /// 采购申请明细Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseRequisitionItemIdProperty = P<PurchaseOrderItem>.RegisterRefId(e => e.PurchaseRequisitionItemId, ReferenceType.Normal);

        /// <summary>
        /// 采购申请明细Id
        /// </summary>
        public double PurchaseRequisitionItemId
        {
            get { return (double)GetRefId(PurchaseRequisitionItemIdProperty); }
            set { SetRefId(PurchaseRequisitionItemIdProperty, value); }
        }

        /// <summary>
        /// 采购申请明细
        /// </summary>
        public static readonly RefEntityProperty<PurchaseRequisitionItem> PurchaseRequisitionItemProperty = P<PurchaseOrderItem>.RegisterRef(e => e.PurchaseRequisitionItem, PurchaseRequisitionItemIdProperty);

        /// <summary>
        /// 采购申请明细
        /// </summary>
        public PurchaseRequisitionItem PurchaseRequisitionItem
        {
            get { return GetRefEntity(PurchaseRequisitionItemProperty); }
            set { SetRefEntity(PurchaseRequisitionItemProperty, value); }
        }
        #endregion

        #region 采购订单 PurchaseOrder
        /// <summary>
        /// 采购订单Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseOrderIdProperty = P<PurchaseOrderItem>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Parent);

        /// <summary>
        /// 采购订单Id
        /// </summary>
        public double PurchaseOrderId
        {
            get { return (double)GetRefId(PurchaseOrderIdProperty); }
            set { SetRefId(PurchaseOrderIdProperty, value); }
        }

        /// <summary>
        /// 采购订单
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrder> PurchaseOrderProperty = P<PurchaseOrderItem>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购订单
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return GetRefEntity(PurchaseOrderProperty); }
            set { SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 总价 Amount
        /// <summary>
        /// 总价
        /// </summary>
        [Label("总价")]
        public static readonly Property<decimal> AmountProperty = P<PurchaseOrderItem>.RegisterReadOnly(
            e => e.Amount, e => e.GetAmount(), QtyProperty);
        /// <summary>
        /// 总价
        /// </summary>

        public decimal Amount
        {
            get { return this.GetProperty(AmountProperty); }
        }
        private decimal GetAmount()
        {
            return Qty * Price;
        }
        #endregion

        #region 单价(不含税) PriceNoTax
        /// <summary>
        /// 单价(不含税)
        /// </summary>
        [Label("单价(不含税)")]
        public static readonly Property<decimal> PriceNoTaxProperty = P<PurchaseOrderItem>.RegisterReadOnly(
            e => e.PriceNoTax, e => e.GetPriceNoTax(), PriceProperty);
        /// <summary>
        /// 单价(不含税)
        /// </summary>

        public decimal PriceNoTax
        {
            get { return this.GetProperty(PriceNoTaxProperty); }
        }
        private decimal GetPriceNoTax()
        {
            return Math.Round(Price * (1 - (TaxRate / 100)), 2);
        }
        #endregion

        #region 采购描述 Description
        /// <summary>
        /// 采购描述
        /// </summary>
        [Label("采购描述")]
        public static readonly Property<string> DescriptionProperty = P<PurchaseOrderItem>.RegisterView(e => e.Description, p => p.PurchaseRequisitionItem.Description);

        /// <summary>
        /// 采购描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<PurchaseOrderItem>.RegisterView(e => e.UnitName, p => p.PurchaseRequisitionItem.ItemUnit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion


        #region 单位Id UintId
        /// <summary>
        /// UintId
        /// </summary>
        [Label("单位Id")]
        public static readonly Property<double> UintIdProperty = P<PurchaseOrderItem>.RegisterView(e => e.UintId, p => p.PurchaseRequisitionItem.ItemUnitId);

        /// <summary>
        /// UintId
        /// </summary>
        public double UintId
        {
            get { return this.GetProperty(UintIdProperty); }
        }
        #endregion


        #region 采购对象编码 ObjectCode
        /// <summary>
        /// 采购对象编码
        /// </summary>
        [Label("采购对象编码")]
        public static readonly Property<string> ObjectCodeProperty = P<PurchaseOrderItem>.RegisterView(e => e.ObjectCode, p => p.PurchaseRequisitionItem.ObjectCode);

        /// <summary>
        /// 采购对象编码
        /// </summary>
        public string ObjectCode
        {
            get { return this.GetProperty(ObjectCodeProperty); }
        }
        #endregion

        #region 采购对象描述 ObjectName
        /// <summary>
        /// 采购对象描述
        /// </summary>
        [Label("采购对象描述")]
        public static readonly Property<string> ObjectNameProperty = P<PurchaseOrderItem>.RegisterView(e => e.ObjectName, p => p.PurchaseRequisitionItem.Description);

        /// <summary>
        /// 采购对象描述
        /// </summary>
        public string ObjectName
        {
            get { return this.GetProperty(ObjectNameProperty); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<PurchaseOrderItem>.RegisterView(e => e.Specification, p => p.PurchaseRequisitionItem.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
        }
        #endregion

        #region 项目编码 ProjectCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectCodeProperty = P<PurchaseOrderItem>.RegisterView(e => e.ProjectCode, p => p.PurchaseRequisitionItem.PurchaseRequisition.Project.Code);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode
        {
            get { return this.GetProperty(ProjectCodeProperty); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<PurchaseOrderItem>.RegisterView(e => e.ProjectName, p => p.PurchaseRequisitionItem.PurchaseRequisition.Project.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
        }
        #endregion

        #region 项目事项 KeyItem
        /// <summary>
        /// 项目事项
        /// </summary>
        [Label("项目事项")]
        public static readonly Property<string> KeyItemProperty = P<PurchaseOrderItem>.RegisterView(e => e.KeyItem, p => p.PurchaseRequisitionItem.ProjectKeyItem.Description);

        /// <summary>
        /// 项目事项
        /// </summary>
        public string KeyItem
        {
            get { return this.GetProperty(KeyItemProperty); }
        }
        #endregion

        #region 工厂id FactoryId
        /// <summary>
        /// 工厂id
        /// </summary>
        [Label("工厂id")]
        public static readonly Property<double> FactoryIdProperty = P<PurchaseOrderItem>.RegisterView(e => e.FactoryId, p => p.PurchaseOrder.FactoryId);

        /// <summary>
        /// 工厂id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
        }
        #endregion

        #region 部门id DepartmentId
        /// <summary>
        /// 部门id
        /// </summary>
        [Label("部门id")]
        public static readonly Property<double> DepartmentIdProperty = P<PurchaseOrderItem>.RegisterView(e => e.DepartmentId, p => p.PurchaseOrder.DepartmentId);

        /// <summary>
        /// 部门id
        /// </summary>
        public double DepartmentId
        {
            get { return this.GetProperty(DepartmentIdProperty); }
        }
        #endregion

        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType> PurchaseObjectTypeProperty = P<PurchaseOrderItem>.RegisterView(e => e.PurchaseObjectType, p => p.PurchaseOrder.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType PurchaseObjectType
        {
            get { return this.GetProperty(PurchaseObjectTypeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 采购订单明细 实体配置
    /// </summary>
    internal class PurchaseOrderItemConfig : EntityConfig<PurchaseOrderItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PO_ITEM").MapAllProperties();
            Meta.Property(PurchaseOrderItem.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}