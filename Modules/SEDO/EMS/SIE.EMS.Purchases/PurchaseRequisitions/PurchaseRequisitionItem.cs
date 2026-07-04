using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Projects;
using SIE.Equipments.Enums;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购申请明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("采购申请明细")]
    [DisplayMember(nameof(PurchaseRequisitionNoLine))]
    public partial class PurchaseRequisitionItem : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<PurchaseRequisitionItem>.Register(e => e.LineNo);

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
        public static readonly Property<string> ObjectCodeProperty = P<PurchaseRequisitionItem>.Register(e => e.ObjectCode);

        /// <summary>
        /// 采购对象编码
        /// </summary>
        public string ObjectCode
        {
            get { return this.GetProperty(ObjectCodeProperty); }
            set { this.SetProperty(ObjectCodeProperty, value); }
        }
        #endregion

        #region 采购对象编码 ObjectCodeInfo
        /// <summary>
        /// 采购对象编码Id
        /// </summary>
        [Label("采购对象编码")]
        public static readonly IRefIdProperty ObjectCodeInfoIdProperty =
            P<PurchaseRequisitionItem>.RegisterRefId(e => e.ObjectCodeInfoId, ReferenceType.Normal);

        /// <summary>
        /// 采购对象编码Id
        /// </summary>
        public string ObjectCodeInfoId
        {
            get { return (string)this.GetRefId(ObjectCodeInfoIdProperty); }
            set { this.SetRefId(ObjectCodeInfoIdProperty, value); }
        }

        /// <summary>
        /// 采购对象编码
        /// </summary>
        public static readonly RefEntityProperty<ObjectCodeInfo> ObjectCodeInfoProperty =
            P<PurchaseRequisitionItem>.RegisterRef(e => e.ObjectCodeInfo, ObjectCodeInfoIdProperty);

        /// <summary>
        /// 采购对象编码
        /// </summary>
        public ObjectCodeInfo ObjectCodeInfo
        {
            get { return this.GetRefEntity(ObjectCodeInfoProperty); }
            set { this.SetRefEntity(ObjectCodeInfoProperty, value); }
        }
        #endregion

        #region 采购对象描述 Description
        /// <summary>
        /// 采购对象描述
        /// </summary>
        [MaxLength(200)]
        [Label("采购对象描述")]
        public static readonly Property<string> DescriptionProperty = P<PurchaseRequisitionItem>.Register(e => e.Description);

        /// <summary>
        /// 采购对象描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<PurchaseRequisitionItem>.Register(e => e.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
            set { this.SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 需求数量 Qty
        /// <summary>
        /// 需求数量
        /// </summary>
        [Label("需求数量")]
        [MinValue(1)]
        public static readonly Property<int> QtyProperty = P<PurchaseRequisitionItem>.Register(e => e.Qty);

        /// <summary>
        /// 需求数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 参考单价 Price
        /// <summary>
        /// 参考单价
        /// </summary>
        [Label("参考单价")]
        [MinValue(0.01)]
        public static readonly Property<decimal?> PriceProperty = P<PurchaseRequisitionItem>.Register(e => e.Price);

        /// <summary>
        /// 参考单价
        /// </summary>
        public decimal? Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }
        #endregion

        #region 需求日期 DemandDate
        /// <summary>
        /// 需求日期
        /// </summary>
        [Label("需求日期")]
        [Required]
        public static readonly Property<DateTime> DemandDateProperty = P<PurchaseRequisitionItem>.Register(e => e.DemandDate);

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime DemandDate
        {
            get { return GetProperty(DemandDateProperty); }
            set { SetProperty(DemandDateProperty, value); }
        }
        #endregion

        #region 已采购数量 PurchasedQty
        /// <summary>
        /// 已采购数量
        /// </summary>
        [Label("已采购数量")]
        public static readonly Property<decimal> PurchasedQtyProperty = P<PurchaseRequisitionItem>.Register(e => e.PurchasedQty);

        /// <summary>
        /// 已采购数量
        /// </summary>
        public decimal PurchasedQty
        {
            get { return GetProperty(PurchasedQtyProperty); }
            set { SetProperty(PurchasedQtyProperty, value); }
        }
        #endregion

        #region 已采购金额 PurchasePrice
        /// <summary>
        /// 已采购金额
        /// </summary>
        [Label("已采购金额")]
        public static readonly Property<decimal> PurchasePriceProperty = P<PurchaseRequisitionItem>.Register(e => e.PurchasePrice);

        /// <summary>
        /// 已采购金额
        /// </summary>
        public decimal PurchasePrice
        {
            get { return GetProperty(PurchasePriceProperty); }
            set { SetProperty(PurchasePriceProperty, value); }
        }
        #endregion

        #region 建议供应商 Supplier
        /// <summary>
        /// 建议供应商Id
        /// </summary>
        [Label("建议供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<PurchaseRequisitionItem>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 建议供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 建议供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<PurchaseRequisitionItem>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 建议供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 建议供应商名称 SupplierName
        /// <summary>
        /// 建议供应商名称
        /// </summary>
        [Label("建议供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<PurchaseRequisitionItem>.Register(e => e.SupplierName);

        /// <summary>
        /// 建议供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
            set { this.SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 单位 ItemUnit
        /// <summary>
        /// 单位Id
        /// </summary>
        public static readonly IRefIdProperty ItemUnitIdProperty = P<PurchaseRequisitionItem>.RegisterRefId(e => e.ItemUnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? ItemUnitId
        {
            get { return (double?)GetRefNullableId(ItemUnitIdProperty); }
            set { SetRefNullableId(ItemUnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> ItemUnitProperty = P<PurchaseRequisitionItem>.RegisterRef(e => e.ItemUnit, ItemUnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit ItemUnit
        {
            get { return GetRefEntity(ItemUnitProperty); }
            set { SetRefEntity(ItemUnitProperty, value); }
        }
        #endregion

        #region 项目关键事项 ProjectKeyItem
        /// <summary>
        /// 项目关键事项Id
        /// </summary>
        public static readonly IRefIdProperty ProjectKeyItemIdProperty = P<PurchaseRequisitionItem>.RegisterRefId(e => e.ProjectKeyItemId, ReferenceType.Normal);

        /// <summary>
        /// 项目关键事项Id
        /// </summary>
        public double? ProjectKeyItemId
        {
            get { return (double?)GetRefNullableId(ProjectKeyItemIdProperty); }
            set { SetRefNullableId(ProjectKeyItemIdProperty, value); }
        }

        /// <summary>
        /// 项目关键事项
        /// </summary>
        public static readonly RefEntityProperty<ProjectKeyItem> ProjectKeyItemProperty = P<PurchaseRequisitionItem>.RegisterRef(e => e.ProjectKeyItem, ProjectKeyItemIdProperty);

        /// <summary>
        /// 项目关键事项
        /// </summary>
        public ProjectKeyItem ProjectKeyItem
        {
            get { return GetRefEntity(ProjectKeyItemProperty); }
            set { SetRefEntity(ProjectKeyItemProperty, value); }
        }
        #endregion

        #region 采购申请 PurchaseRequisition
        /// <summary>
        /// 采购申请Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseRequisitionIdProperty = P<PurchaseRequisitionItem>.RegisterRefId(e => e.PurchaseRequisitionId, ReferenceType.Parent);

        /// <summary>
        /// 采购申请Id
        /// </summary>
        public double PurchaseRequisitionId
        {
            get { return (double)GetRefId(PurchaseRequisitionIdProperty); }
            set { SetRefId(PurchaseRequisitionIdProperty, value); }
        }

        /// <summary>
        /// 采购申请
        /// </summary>
        public static readonly RefEntityProperty<PurchaseRequisition> PurchaseRequisitionProperty = P<PurchaseRequisitionItem>.RegisterRef(e => e.PurchaseRequisition, PurchaseRequisitionIdProperty);

        /// <summary>
        /// 采购申请
        /// </summary>
        public PurchaseRequisition PurchaseRequisition
        {
            get { return GetRefEntity(PurchaseRequisitionProperty); }
            set { SetRefEntity(PurchaseRequisitionProperty, value); }
        }
        #endregion

        #region 采购申请单+行号 PurchaseRequisitionNoLine
        /// <summary>
        /// 采购申请单+行号
        /// </summary>
        [Label("采购申请单+行号")]
        public static readonly Property<string> PurchaseRequisitionNoLineProperty = P<PurchaseRequisitionItem>.Register(e => e.PurchaseRequisitionNoLine);

        /// <summary>
        /// 采购申请单+行号
        /// </summary>
        public string PurchaseRequisitionNoLine
        {
            get { return this.GetProperty(PurchaseRequisitionNoLineProperty); }
            set { this.SetProperty(PurchaseRequisitionNoLineProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 单号 PurchaseRequisitionNo
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> PurchaseRequisitionNoProperty = P<PurchaseRequisitionItem>.RegisterView(e => e.PurchaseRequisitionNo, p => p.PurchaseRequisition.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string PurchaseRequisitionNo
        {
            get { return this.GetProperty(PurchaseRequisitionNoProperty); }
        }
        #endregion

        #region 项目编码 ProjectCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectCodeProperty = P<PurchaseRequisitionItem>.RegisterView(e => e.ProjectCode, p => p.PurchaseRequisition.Project.Code);

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
        public static readonly Property<string> ProjectNameProperty = P<PurchaseRequisitionItem>.RegisterView(e => e.ProjectName, p => p.PurchaseRequisition.Project.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<PurchaseRequisitionItem>.RegisterView(e => e.UnitName, p => p.ItemUnit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 关键事项说明 KeyItemDescription
        /// <summary>
        /// 关键事项说明
        /// </summary>
        [Label("关键事项说明")]
        public static readonly Property<string> KeyItemDescriptionProperty = P<PurchaseRequisitionItem>.RegisterView(e => e.KeyItemDescription, p => p.ProjectKeyItem.Description);

        /// <summary>
        /// 关键事项说明
        /// </summary>
        public string KeyItemDescription
        {
            get { return this.GetProperty(KeyItemDescriptionProperty); }
        }
        #endregion

        #region 参考总金额 TotalAmount
        /// <summary>
        /// 参考总金额
        /// </summary>
        [Label("参考总金额")]
        public static readonly Property<decimal?> TotalAmountProperty = P<PurchaseRequisitionItem>.RegisterReadOnly(
            e => e.TotalAmount, e => e.GetTotalAmount(), QtyProperty);
        /// <summary>
        /// 参考总金额
        /// </summary>

        public decimal? TotalAmount
        {
            get { return this.GetProperty(TotalAmountProperty); }
        }
        private decimal? GetTotalAmount()
        {
            if (Price == null)
            {
                return null;
            }
            return Qty * Price.Value;
        }
        #endregion

        #region 项目id(界面属性) ProjectId
        /// <summary>
        /// 项目id
        /// </summary>
        [Label("项目id")]
        public static readonly Property<double> ProjectIdProperty = P<PurchaseRequisitionItem>.Register(e => e.ProjectId);

        /// <summary>
        /// 项目id
        /// </summary>
        public double ProjectId
        {
            get { return this.GetProperty(ProjectIdProperty); }
            set { this.SetProperty(ProjectIdProperty, value); }
        }
        #endregion

        #region 采购类型(界面属性) PurchaseType
        /// <summary>
        /// 采购类型
        /// </summary>
        [Label("采购类型")]
        public static readonly Property<PurchaseType> PurchaseTypeProperty = P<PurchaseRequisitionItem>.Register(e => e.PurchaseType);

        /// <summary>
        /// 采购类型
        /// </summary>
        public PurchaseType PurchaseType
        {
            get { return GetProperty(PurchaseTypeProperty); }
            set { SetProperty(PurchaseTypeProperty, value); }
        }
        #endregion

        #region 采购对象(界面属性) PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType> PurchaseObjectTypeProperty = P<PurchaseRequisitionItem>.Register(e => e.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType PurchaseObjectType
        {
            get { return GetProperty(PurchaseObjectTypeProperty); }
            set { SetProperty(PurchaseObjectTypeProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 采购申请明细 实体配置
    /// </summary>
    internal class PurchaseRequisitionItemConfig : EntityConfig<PurchaseRequisitionItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PR_ITEM").MapAllProperties();
            Meta.Property(PurchaseRequisitionItem.ProjectIdProperty).DontMapColumn();
            Meta.Property(PurchaseRequisitionItem.PurchaseTypeProperty).DontMapColumn();
            Meta.Property(PurchaseRequisitionItem.PurchaseObjectTypeProperty).DontMapColumn();
            Meta.Property(PurchaseRequisitionItem.ObjectCodeInfoIdProperty).DontMapColumn();
            Meta.Property(PurchaseRequisitionItem.DescriptionProperty).ColumnMeta.HasLength(400);
            Meta.EnablePhantoms();
        }
    }
}