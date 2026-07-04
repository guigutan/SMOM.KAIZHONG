using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
	/// 标签条码
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(PackingLabelCriteria))]
    [DisplayMember(nameof(No))]
    [Label("标签条码")]
    public partial class PackingLabel : SIE.Core.Labels.PackingLabel
    {
        #region 来源单号 AsnNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> AsnNoProperty = P<PackingLabel>.Register((System.Linq.Expressions.Expression<Func<PackingLabel, string>>)(e => (string)e.AsnNo));

        /// <summary>
        /// 来源单号
        /// </summary>
        public string AsnNo
        {
            get { return GetProperty(AsnNoProperty); }
            set { SetProperty(AsnNoProperty, value); }
        }
        #endregion

        #region 相关单号 OrderNo
        /// <summary>
        /// 相关单号
        /// </summary>
        [Label("相关单号")]
        public static readonly Property<string> OrderNoProperty = P<PackingLabel>.Register(e => e.OrderNo);

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo
        {
            get { return GetProperty(OrderNoProperty); }
            set { SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 生成日期 GenerateDate
        /// <summary>
        /// 生成日期
        /// </summary>
        [Label("生成日期")]
        public static readonly Property<DateTime> GenerateDateProperty = P<PackingLabel>.Register(e => e.GenerateDate);

        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime GenerateDate
        {
            get { return GetProperty(GenerateDateProperty); }
            set { SetProperty(GenerateDateProperty, value); }
        }
        #endregion

        #region 是否打印 IsPrinted
        /// <summary>
        /// 是否打印
        /// </summary>
        [Label("是否打印")]
        public static readonly Property<bool> IsPrintedProperty = P<PackingLabel>.Register(e => e.IsPrinted);

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrinted
        {
            get { return GetProperty(IsPrintedProperty); }
            set { SetProperty(IsPrintedProperty, value); }
        }
        #endregion

        #region 打印次数 PrintTimes
        /// <summary>
        /// 打印次数
        /// </summary>
        [Label("打印次数")]
        public static readonly Property<int> PrintTimesProperty = P<PackingLabel>.Register(e => e.PrintTimes);

        /// <summary>
        /// 打印次数
        /// </summary>
        public int PrintTimes
        {
            get { return GetProperty(PrintTimesProperty); }
            set { SetProperty(PrintTimesProperty, value); }
        }
        #endregion

        #region 是否报废 IsScrapped
        /// <summary>
        /// 是否报废
        /// </summary>
        [Label("是否报废")]
        public static readonly Property<bool> IsScrappedProperty = P<PackingLabel>.Register(e => e.IsScrapped);

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScrapped
        {
            get { return GetProperty(IsScrappedProperty); }
            set { SetProperty(IsScrappedProperty, value); }
        }
        #endregion

        #region 打印时间 PrintDate
        /// <summary>
        /// 打印时间
        /// </summary>
        [Label("打印时间")]
        public static readonly Property<DateTime?> PrintDateProperty = P<PackingLabel>.Register(e => e.PrintDate);

        /// <summary>
        /// 打印时间
        /// </summary>
        public DateTime? PrintDate
        {
            get { return GetProperty(PrintDateProperty); }
            set { SetProperty(PrintDateProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<string> SourceTypeProperty = P<PackingLabel>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public string SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 来源数据 SourceData
        /// <summary>
        /// 来源数据
        /// </summary>
        [Label("来源数据")]
        public static readonly Property<string> SourceDataProperty = P<PackingLabel>.Register((System.Linq.Expressions.Expression<Func<PackingLabel, string>>)(e => (string)e.SourceData));

        /// <summary>
        /// 来源数据
        /// </summary>
        public string SourceData
        {
            get { return GetProperty(SourceDataProperty); }
            set { SetProperty(SourceDataProperty, value); }
        }
        #endregion

        #region 来源ID SourceId
        /// <summary>
        /// 来源ID
        /// </summary>
        [Label("来源ID")]
        public static readonly Property<double?> SourceIdProperty = P<PackingLabel>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID
        /// </summary>
        public double? SourceId
        {
            get { return GetProperty(SourceIdProperty); }
            set { SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 收货日期 CollectTime
        /// <summary>
        /// 收货日期
        /// </summary>
        [Label("收货日期")]
        public static readonly Property<DateTime?> CollectTimeProperty = P<PackingLabel>.Register(e => e.CollectTime);

        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime? CollectTime
        {
            get { return GetProperty(CollectTimeProperty); }
            set { SetProperty(CollectTimeProperty, value); }
        }
        #endregion

        #region 备注 Remarks
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarksProperty = P<PackingLabel>.Register(e => e.Remarks);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get { return GetProperty(RemarksProperty); }
            set { SetProperty(RemarksProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public new static readonly IRefIdProperty ItemIdProperty =
            P<PackingLabel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public new double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public new static readonly RefEntityProperty<Item> ItemProperty =
            P<PackingLabel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public new Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        public static readonly IRefIdProperty SupplierIdProperty = P<PackingLabel>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<PackingLabel>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        public static readonly IRefIdProperty CustomerIdProperty = P<PackingLabel>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public double? CustomerId
        {
            get { return (double?)GetRefNullableId(CustomerIdProperty); }
            set { SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<PackingLabel>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 类型 PackingType
        /// <summary>
        /// 类型Id
        /// </summary>
        [Label("类型")]
        public static readonly IRefIdProperty PackingTypeIdProperty = P<PackingLabel>.RegisterRefId(e => e.PackingTypeId, ReferenceType.Normal);

        /// <summary>
        /// 类型Id
        /// </summary>
        public double? PackingTypeId
        {
            get { return (double?)GetRefNullableId(PackingTypeIdProperty); }
            set { SetRefNullableId(PackingTypeIdProperty, value); }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> PackingTypeProperty = P<PackingLabel>.RegisterRef(e => e.PackingType, PackingTypeIdProperty);

        /// <summary>
        /// 类型
        /// </summary>
        public PackingUnit PackingType
        {
            get { return GetRefEntity(PackingTypeProperty); }
            set { SetRefEntity(PackingTypeProperty, value); }
        }
        #endregion

        #region 包装单位类型 PackageUnitType
        /// <summary>
        /// 包装单位类型
        /// </summary>
        [Label("包装单位类型")]
        public static readonly Property<PackageUnitType?> PackageUnitTypeProperty = P<PackingLabel>.RegisterView(e => e.PackageUnitType, p => p.PackingType.PackageUnitType);

        /// <summary>
        /// 包装单位类型
        /// </summary>
        public PackageUnitType? PackageUnitType
        {
            get { return this.GetProperty(PackageUnitTypeProperty); }
        }
        #endregion

        #region 物料包装规则 ItemPackageRule
        /// <summary>
        /// 物料包装规则Id
        /// </summary>
        [Label("物料包装规则")]
        public static readonly IRefIdProperty ItemPackageRuleIdProperty =
            P<PackingLabel>.RegisterRefId(e => e.ItemPackageRuleId, ReferenceType.Normal);

        /// <summary>
        /// 物料包装规则Id
        /// </summary>
        public double? ItemPackageRuleId
        {
            get { return (double?)this.GetRefNullableId(ItemPackageRuleIdProperty); }
            set { this.SetRefNullableId(ItemPackageRuleIdProperty, value); }
        }

        /// <summary>
        /// 物料包装规则
        /// </summary>
        public static readonly RefEntityProperty<ItemPackageRule> ItemPackageRuleProperty =
            P<PackingLabel>.RegisterRef(e => e.ItemPackageRule, ItemPackageRuleIdProperty);

        /// <summary>
        /// 物料包装规则
        /// </summary>
        public ItemPackageRule ItemPackageRule
        {
            get { return this.GetRefEntity(ItemPackageRuleProperty); }
            set { this.SetRefEntity(ItemPackageRuleProperty, value); }
        }
        #endregion

        #region 是否序列 IsSequence
        /// <summary>
        /// 是否序列
        /// </summary>
        [Label("是否序列")]
        public static readonly Property<bool> IsSequenceProperty = P<PackingLabel>.Register(e => e.IsSequence);

        /// <summary>
        /// 是否序列
        /// </summary>
        public bool IsSequence
        {
            get { return GetProperty(IsSequenceProperty); }
            set { SetProperty(IsSequenceProperty, value); }
        }
        #endregion

        #region 启用物料扩展属性 EnableExtPro
        /// <summary>
        /// 启用物料扩展属性
        /// </summary>
        [Label("启用物料扩展属性")]
        public static readonly Property<bool> EnableExtProProperty = P<PackingLabel>.RegisterView(e => e.EnableExtPro, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 启用物料扩展属性
        /// </summary>
        public bool EnableExtPro
        {
            get { return this.GetProperty(EnableExtProProperty); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<PackingLabel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性显示名 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<PackingLabel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 生成批标志 GenerateLotFlag
        /// <summary>
        /// 生成批标志
        /// </summary>
        [Label("生成批标志")]
        [MaxLength(80)]
        public static readonly Property<string> GenerateLotFlagProperty = P<PackingLabel>.Register(e => e.GenerateLotFlag);

        /// <summary>
        /// 生成批标志
        /// </summary>
        public string GenerateLotFlag
        {
            get { return this.GetProperty(GenerateLotFlagProperty); }
            set { this.SetProperty(GenerateLotFlagProperty, value); }
        }
        #endregion
        
        #region 包装数 PackedQty
        /// <summary>
        /// 包装数
        /// </summary>
        [Label("包装数")]
        public static readonly Property<decimal> PackedQtyProperty = P<PackingLabel>.Register(e => e.PackedQty);

        /// <summary>
        /// 包装数
        /// </summary>
        public decimal PackedQty
        {
            get { return this.GetProperty(PackedQtyProperty); }
            set { this.SetProperty(PackedQtyProperty, value); }
        }
        #endregion

        #region 是否混包 IsMixPacking
        /// <summary>
        /// 是否混包
        /// </summary>
        [Label("是否混包")]
        public static readonly Property<bool> IsMixPackingProperty = P<PackingLabel>.Register(e => e.IsMixPacking);

        /// <summary>
        /// 是否混包
        /// </summary>
        public bool IsMixPacking
        {
            get { return this.GetProperty(IsMixPackingProperty); }
            set { this.SetProperty(IsMixPackingProperty, value); }
        }
        #endregion

        #region 初始项目号 InitProjectNo
        /// <summary>
        /// 初始项目号
        /// </summary>
        [Label("初始项目号")]
        public static readonly Property<string> InitProjectNoProperty = P<PackingLabel>.Register(e => e.InitProjectNo);

        /// <summary>
        /// 初始项目号
        /// </summary>
        public string InitProjectNo
        {
            get { return this.GetProperty(InitProjectNoProperty); }
            set { this.SetProperty(InitProjectNoProperty, value); }
        }
        #endregion

        #region 初始任务号 InitTaskNo
        /// <summary>
        /// 初始任务号
        /// </summary>
        [Label("初始任务号")]
        public static readonly Property<string> InitTaskNoProperty = P<PackingLabel>.Register(e => e.InitTaskNo);

        /// <summary>
        /// 初始任务号
        /// </summary>
        public string InitTaskNo
        {
            get { return this.GetProperty(InitTaskNoProperty); }
            set { this.SetProperty(InitTaskNoProperty, value); }
        }
        #endregion

        #region 数量SET SetQty
        /// <summary>
        /// 数量SET
        /// </summary>
        [Label("数量SET")]
        public static readonly Property<int> SetQtyProperty = P<PackingLabel>.Register(e => e.SetQty);

        /// <summary>
        /// 数量SET
        /// </summary>
        public int SetQty
        {
            get { return this.GetProperty(SetQtyProperty); }
            set { this.SetProperty(SetQtyProperty, value); }
        }
        #endregion

        #region 叉板数 XPlateQty
        /// <summary>
        /// 叉板数
        /// </summary>
        [Label("叉板数")]
        public static readonly Property<int> XPlateQtyProperty = P<PackingLabel>.Register(e => e.XPlateQty);

        /// <summary>
        /// 叉板数
        /// </summary>
        public int XPlateQty
        {
            get { return this.GetProperty(XPlateQtyProperty); }
            set { this.SetProperty(XPlateQtyProperty, value); }
        }
        #endregion

        #region 尾数包 IsTail
        /// <summary>
        /// 尾数包
        /// </summary>
        [Label("尾数包")]
        public static readonly Property<bool> IsTailProperty = P<PackingLabel>.Register(e => e.IsTail);

        /// <summary>
        /// 尾数包
        /// </summary>
        public bool IsTail
        {
            get { return this.GetProperty(IsTailProperty); }
            set { this.SetProperty(IsTailProperty, value); }
        }
        #endregion

        #region 包装根节点ID RootId
        /// <summary>
        /// 包装根节点ID(包装最上层条码ID)
        /// </summary>
        [Label("包装根节点ID")]
        public static readonly Property<double> RootIdProperty = P<PackingLabel>.Register(e => e.RootId);

        /// <summary>
        /// 包装根节点ID
        /// </summary>
        public double RootId
        {
            get { return this.GetProperty(RootIdProperty); }
            set { this.SetProperty(RootIdProperty, value); }
        }
        #endregion

        #region 工厂 Enterprise
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty EnterpriseIdProperty =
            P<PackingLabel>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? EnterpriseId
        {
            get { return (double?)this.GetRefNullableId(EnterpriseIdProperty); }
            set { this.SetRefNullableId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty =
            P<PackingLabel>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Enterprise
        {
            get { return this.GetRefEntity(EnterpriseProperty); }
            set { this.SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<PackingLabel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<PackingLabel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 生产状态 LabelStatus
        /// <summary>
        /// 标签状态
        /// </summary>
        [Label("生产状态")]
        public static readonly Property<LabelStatus?> LabelStatusProperty = P<PackingLabel>.Register(e => e.LabelStatus);

        /// <summary>
        /// 生产状态
        /// </summary>
        public LabelStatus? LabelStatus
        {
            get { return this.GetProperty(LabelStatusProperty); }
            set { this.SetProperty(LabelStatusProperty, value); }
        }
        #endregion

        #region 来源MES IsFromMes
        /// <summary>
        /// 来源MES
        /// </summary>
        [Label("来源MES")]
        public static readonly Property<bool?> IsFromMesProperty = P<PackingLabel>.Register(e => e.IsFromMes);

        /// <summary>
        /// 来源MES
        /// </summary>
        public bool? IsFromMes
        {
            get { return this.GetProperty(IsFromMesProperty); }
            set { this.SetProperty(IsFromMesProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PackingLabel>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<PackingLabel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料描述 ItemDescription
        /// <summary>
        /// 物料描述
        /// </summary>
        [Label("物料描述")]
        public static readonly Property<string> ItemDescriptionProperty = P<PackingLabel>.RegisterView(e => e.ItemDescription, p => p.Item.Description);

        /// <summary>
        /// 物料描述
        /// </summary>
        public string ItemDescription
        {
            get { return this.GetProperty(ItemDescriptionProperty); }
            set { this.SetProperty(ItemDescriptionProperty, value); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 物料规格
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<PackingLabel>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 物料规格
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
            set { this.SetProperty(ItemSpecificationModelProperty, value); }
        }
        #endregion

        #region 主单位 ItemUnitName
        /// <summary>
        /// 主单位
        /// </summary>
        [Label("主单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<PackingLabel>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 主单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
            set { this.SetProperty(ItemUnitNameProperty, value); }
        }
        #endregion

        #region 供应商 SupplierName
        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        public static readonly Property<string> SupplierNameProperty = P<PackingLabel>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
            set { this.SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户")]
        public static readonly Property<string> CustomerNameProperty = P<PackingLabel>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
            set { this.SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #region 包装单位 PackingTypeName
        /// <summary>
        /// 包装单位名称
        /// </summary>
        [Label("包装单位")]
        public static readonly Property<string> PackingTypeNameProperty = P<PackingLabel>.RegisterView(e => e.PackingTypeName, p => p.PackingType.Name);

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackingTypeName
        {
            get { return this.GetProperty(PackingTypeNameProperty); }
            set { this.SetProperty(PackingTypeNameProperty, value); }
        }
        #endregion

        #region 物料包装规则 ItemPackageRuleCode
        /// <summary>
        /// 物料包装规则
        /// </summary>
        [Label("物料包装规则")]
        public static readonly Property<string> ItemPackageRuleCodeProperty = P<PackingLabel>.RegisterView(e => e.ItemPackageRuleCode, p => p.ItemPackageRule.Code);

        /// <summary>
        /// 物料包装规则
        /// </summary>
        public string ItemPackageRuleCode
        {
            get { return this.GetProperty(ItemPackageRuleCodeProperty); }
            set { this.SetProperty(ItemPackageRuleCodeProperty, value); }
        }
        #endregion

        #region 物料包装规则名称 ItemPackageRuleName
        /// <summary>
        /// 物料包装规则名称
        /// </summary>
        [Label("物料包装规则名称")]
        public static readonly Property<string> ItemPackageRuleNameProperty = P<PackingLabel>.RegisterView(e => e.ItemPackageRuleName, p => p.ItemPackageRule.Name);

        /// <summary>
        /// 物料包装规则名称
        /// </summary>
        public string ItemPackageRuleName
        {
            get { return this.GetProperty(ItemPackageRuleNameProperty); }
            set { this.SetProperty(ItemPackageRuleNameProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WoNoProperty = P<PackingLabel>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion

        #region 工厂编码 EnterpriseCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> EnterpriseCodeProperty = P<PackingLabel>.RegisterView(e => e.EnterpriseCode, p => p.Enterprise.Code);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string EnterpriseCode
        {
            get { return this.GetProperty(EnterpriseCodeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 标签条码 实体配置
    /// </summary>
    internal class PackingLabelConfig : EntityConfig<PackingLabel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PACK_LABEL").MapAllProperties();
            Meta.Property(PackingLabel.RemarksProperty).ColumnMeta.HasLength(4000);
            Meta.Property(PackingLabel.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.Property(PackingLabel.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.Property(PackingLabel.PackageNoProperty).MapColumn().HasIndex(IndexTypeMeta.Indexed);
            Meta.Property(PackingLabel.LotProperty).MapColumn().HasIndex(IndexTypeMeta.Indexed);
            Meta.Property(PackingLabel.AsnNoProperty).MapColumn().HasIndex(IndexTypeMeta.Indexed);
            Meta.Property(PackingLabel.RootIdProperty).MapColumn().HasIndex(IndexTypeMeta.Indexed);
            Meta.IndexGroupOnProperties(PackingLabel.RootIdProperty, PackingLabel.PackageNoProperty);
            Meta.Property(PackingLabel.ItemIdProperty).ColumnMeta.IgnoreFK();
            Meta.Property(PackingLabel.ItemPackageRuleIdProperty).ColumnMeta.IgnoreFK();         
            Meta.EnablePhantoms();
            Meta.DisableDataSync();
        }
    }

    /// <summary>
    /// LPN信息
    /// </summary>
    public class LpnData
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 扫描条码
        /// </summary>
        public string ScanNo { get; set; }

        /// <summary>
        /// LPN编码
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 物料Id
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
        /// 规格型号
        /// </summary>
        public string SpecificationModel { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnit { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// ASN单号
        /// </summary>
        public string AsnNo { get; set; }

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
        /// 库存状态 1:未质检; 10:合格; 20:不合格;
        /// </summary>
        public int? OnhandState { get; set; }

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public double LocId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocCode { get; set; }

        /// <summary>
        /// 是否序列
        /// </summary>
        public bool IsSequence { get; set; }

        /// <summary>
        /// 是否位置跟踪
        /// </summary>
        public bool IsLocation { get; set; }
    }
}
