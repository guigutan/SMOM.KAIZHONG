using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Commom;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using SIE.Warehouses.Enums;
using System;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 发货计划
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DeliveryPlanCriteria))]
    [Label("发货计划")]
    public partial class DeliveryPlan : DataEntity
    {
        #region 需创单数量 CreateQty
        /// <summary>
        /// 需创单数量
        /// </summary>
        [Label("需创单数量")]
        public static readonly Property<decimal> CreateQtyProperty = P<DeliveryPlan>.Register(e => e.CreateQty);

        /// <summary>
        /// 需创单数量
        /// </summary>
        public decimal CreateQty
        {
            get { return GetProperty(CreateQtyProperty); }
            set { SetProperty(CreateQtyProperty, value); }
        }
        #endregion

        #region 需求数 RequireQty
        /// <summary>
        /// 需求数
        /// </summary>
        [Label("需求数")]
        public static readonly Property<decimal> RequireQtyProperty = P<DeliveryPlan>.Register(e => e.RequireQty);

        /// <summary>
        /// 需求数
        /// </summary>
        public decimal RequireQty
        {
            get { return GetProperty(RequireQtyProperty); }
            set { SetProperty(RequireQtyProperty, value); }
        }
        #endregion

        #region 未建单数 NoCreateQty
        /// <summary>
        /// 未建单数
        /// </summary>
        [Label("未建单数")]
        public static readonly Property<decimal> NoCreateQtyProperty = P<DeliveryPlan>.Register(e => e.NoCreateQty);

        /// <summary>
        /// 未建单数
        /// </summary>
        public decimal NoCreateQty
        {
            get { return GetProperty(NoCreateQtyProperty); }
            set { SetProperty(NoCreateQtyProperty, value); }
        }
        #endregion

        #region 发货数 DeliveryQty
        /// <summary>
        /// 发货数
        /// </summary>
        [Label("发货数")]
        public static readonly Property<decimal> DeliveryQtyProperty = P<DeliveryPlan>.Register(e => e.DeliveryQty);

        /// <summary>
        /// 发货数
        /// </summary>
        public decimal DeliveryQty
        {
            get { return GetProperty(DeliveryQtyProperty); }
            set { SetProperty(DeliveryQtyProperty, value); }
        }
        #endregion

        #region 取消数 CancelQty
        /// <summary>
        /// 取消数
        /// </summary>
        [Label("取消数")]
        public static readonly Property<decimal> CancelQtyProperty = P<DeliveryPlan>.Register(e => e.CancelQty);

        /// <summary>
        /// 取消数
        /// </summary>
        public decimal CancelQty
        {
            get { return GetProperty(CancelQtyProperty); }
            set { SetProperty(CancelQtyProperty, value); }
        }
        #endregion

        #region 发货时间 DeliveryDate
        /// <summary>
        /// 发货时间
        /// </summary>
        [Required]
        [Label("发货时间")]
        public static readonly Property<DateTime?> DeliveryDateProperty = P<DeliveryPlan>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return GetProperty(DeliveryDateProperty); }
            set { SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 相关单号 OrderNo
        /// <summary>
        /// 相关单号
        /// </summary>
        [Label("相关单号")]
        public static readonly Property<string> OrderNoProperty = P<DeliveryPlan>.Register(e => e.OrderNo);

        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo
        {
            get { return GetProperty(OrderNoProperty); }
            set { SetProperty(OrderNoProperty, value); }
        }
        #endregion
       

        #region 货主编码 StorerCode
        /// <summary>
        /// 货主编码
        /// </summary>
        [Label("货主编码")]
        public static readonly Property<string> StorerCodeProperty = P<DeliveryPlan>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 指定项目号 ProjectNo
        /// <summary>
        /// 指定项目号
        /// </summary>
        [Label("指定项目号")]
        public static readonly Property<string> ProjectNoProperty = P<DeliveryPlan>.Register(e => e.ProjectNo);

        /// <summary>
        /// 指定项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 指定任务号 TaskNo
        /// <summary>
        /// 指定任务号
        /// </summary>
        [Label("指定任务号")]
        public static readonly Property<string> TaskNoProperty = P<DeliveryPlan>.Register(e => e.TaskNo);

        /// <summary>
        /// 指定任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 指定批次号 LotCode
        /// <summary>
        /// 指定批次号
        /// </summary>
        [Label("指定批次号")]
        public static readonly Property<string> LotCodeProperty = P<DeliveryPlan>.Register(e => e.LotCode);

        /// <summary>
        /// 指定批次号
        /// </summary>
        public string LotCode
        {
            get { return GetProperty(LotCodeProperty); }
            set { SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 指定生产批次 ProductBatch
        /// <summary>
        /// 指定生产批次
        /// </summary>
        [Label("指定生产批次")]
        public static readonly Property<string> ProductBatchProperty = P<DeliveryPlan>.Register(e => e.ProductBatch);

        /// <summary>
        /// 指定生产批次
        /// </summary>
        public string ProductBatch
        {
            get { return GetProperty(ProductBatchProperty); }
            set { SetProperty(ProductBatchProperty, value); }
        }
        #endregion

        #region 是否合并 IsMergeIssued
        /// <summary>
        /// 是否合并
        /// </summary>
        [Label("是否合并")]
        public static readonly Property<bool> IsMergeIssuedProperty = P<DeliveryPlan>.Register(e => e.IsMergeIssued);

        /// <summary>
        /// 是否合并
        /// </summary>
        public bool IsMergeIssued
        {
            get { return this.GetProperty(IsMergeIssuedProperty); }
            set { this.SetProperty(IsMergeIssuedProperty, value); }
        }
        #endregion

        #region 计划单号 No
        /// <summary>
        /// 计划单号
        /// </summary>
        [Label("计划单号")]
        public static readonly Property<string> NoProperty = P<DeliveryPlan>.Register(e => e.No);

        /// <summary>
        /// 计划单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 计划明细行号 LineNo
        /// <summary>
        /// 计划明细行号
        /// </summary>
        [MinValue(1)]
        [Label("计划明细行号")]
        public static readonly Property<int> LineNoProperty = P<DeliveryPlan>.Register(e => e.LineNo);

        /// <summary>
        /// 计划明细行号
        /// </summary>
        public int LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<DeliveryPlan>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
            set { SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性名称 ItemExtPropName
        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        [Label("物料扩展属性名称")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<DeliveryPlan>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 状态 DeliveryState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<DeliveryState> StateProperty = P<DeliveryPlan>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public DeliveryState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 单据类型 OrderType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("单据类型")]
        public static readonly Property<OrderType> OrderTypeProperty = P<DeliveryPlan>.Register(e => e.OrderType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public OrderType OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<DeliveryPlan>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<DeliveryPlan>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 生产部门 Enterprise
        /// <summary>
        /// 生产部门Id
        /// </summary>
        [Label("生产部门")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<DeliveryPlan>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 生产部门Id
        /// </summary>
        public double? EnterpriseId
        {
            get { return (double?)GetRefNullableId(EnterpriseIdProperty); }
            set { SetRefNullableId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 生产部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<DeliveryPlan>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 生产部门
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty = P<DeliveryPlan>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)GetRefNullableId(CustomerIdProperty); }
            set { SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<DeliveryPlan>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<DeliveryPlan>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<DeliveryPlan>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 发货仓库 Warehouse
        /// <summary>
        /// 发货仓库Id
        /// </summary>
        [Label("发货仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<DeliveryPlan>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发货仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<DeliveryPlan>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发货仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 目标仓库 TargetWarehouse
        /// <summary>
        /// 目标仓库Id
        /// </summary>
        [Label("目标仓库")]
        public static readonly IRefIdProperty TargetWarehouseIdProperty = P<DeliveryPlan>.RegisterRefId(e => e.TargetWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 目标仓库Id
        /// </summary>
        public double? TargetWarehouseId
        {
            get { return (double?)GetRefNullableId(TargetWarehouseIdProperty); }
            set { SetRefNullableId(TargetWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 目标仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> TargetWarehouseProperty = P<DeliveryPlan>.RegisterRef(e => e.TargetWarehouse, TargetWarehouseIdProperty);

        /// <summary>
        /// 目标仓库
        /// </summary>
        public Warehouse TargetWarehouse
        {
            get { return GetRefEntity(TargetWarehouseProperty); }
            set { SetRefEntity(TargetWarehouseProperty, value); }
        }
        #endregion

        #region 来源 DeliverySourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<DeliverySourceType> SourceTypeProperty = P<DeliveryPlan>.Register(e => e.SourceType);

        /// <summary>
        /// 
        /// </summary>
        public DeliverySourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 已打包Unit数量 PackagedUnitQty
        /// <summary>
        /// 已打包Unit数量
        /// </summary>
        [Label("已打包Unit数量")]
        public static readonly Property<decimal> PackagedUnitQtyProperty = P<DeliveryPlan>.Register(e => e.PackagedUnitQty);

        /// <summary>
        /// 已打包Unit数量
        /// </summary>
        public decimal PackagedUnitQty
        {
            get { return this.GetProperty(PackagedUnitQtyProperty); }
            set { this.SetProperty(PackagedUnitQtyProperty, value); }
        }
        #endregion

        #region 相关订单行号 OrderLineNo
        /// <summary>
        /// 相关订单行号
        /// </summary>
        [Label("相关订单行号")]
        public static readonly Property<string> OrderLineNoProperty = P<DeliveryPlan>.Register(e => e.OrderLineNo);

        /// <summary>
        /// 相关订单行号
        /// </summary>
        public string OrderLineNo
        {
            get { return GetProperty(OrderLineNoProperty); }
            set { SetProperty(OrderLineNoProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<DeliveryPlan>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        [Label("生产资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<DeliveryPlan>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region  齐套情况
        /// <summary>
        /// 
        /// </summary>
        [Label("齐套情况")]
        public static readonly Property<KittingType?> KittingTypeProperty = P<DeliveryPlan>.Register(e => e.KittingType);

        /// <summary>
        /// 齐套情况
        /// </summary>
        public KittingType? KittingType
        {
            get { return GetProperty(KittingTypeProperty); }
            set { SetProperty(KittingTypeProperty, value); }
        }
        #endregion

        #region 视图属性栏位
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<DeliveryPlan>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 生产资源名称 ResourceName
        /// <summary>
        /// 生产资源名称
        /// </summary>
        [Label("生产资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<DeliveryPlan>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 生产资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<DeliveryPlan>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<DeliveryPlan>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<DeliveryPlan>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }
        #endregion

        #region 是否启用扩展属性 ItemEnableExtendProp
        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        [Label("是否启用扩展属性")]
        public static readonly Property<bool> ItemEnableExtendPropProperty = P<DeliveryPlan>.RegisterView(e => e.ItemEnableExtendProp, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        public bool ItemEnableExtendProp
        {
            get { return this.GetProperty(ItemEnableExtendPropProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<DeliveryPlan>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseCodeProperty = P<DeliveryPlan>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 单位
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion
        #endregion

        #region 只读属性
        #region 未打包Unit数 NotPackageUnitQty
        /// <summary>
        /// 未打包Unit数
        /// </summary>
        [Label("未打包Unit数")]
        public static readonly Property<decimal> NotPackageUnitQtyProperty
            = P<DeliveryPlan>.RegisterReadOnly(e => e.NotPackageUnitQty,
                e => e.GetNotPackageUnitQty(), PackagedUnitQtyProperty, RequireQtyProperty);

        /// <summary>
        /// 未打包Unit数
        /// </summary>

        public decimal NotPackageUnitQty
        {
            get { return this.GetProperty(NotPackageUnitQtyProperty); }
        }

        private decimal GetNotPackageUnitQty()
        {
            if (PackagedUnitQty <= RequireQty)
            {
                return RequireQty - PackagedUnitQty;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 出货计划行号 DeliveryPlanLine
        /// <summary>
        /// 出货计划行号
        /// </summary>
        [Label("出货计划行号")]
        public static readonly Property<string> DeliveryPlanLineProperty = P<DeliveryPlan>.RegisterReadOnly(
            e => e.DeliveryPlanLine, e => e.GetDeliveryPlanLine(), NoProperty, LineNoProperty);
        /// <summary>
        /// 出货计划行号
        /// </summary>

        public string DeliveryPlanLine
        {
            get { return this.GetProperty(DeliveryPlanLineProperty); }
        }
        private string GetDeliveryPlanLine()
        {
            return string.Format("{0}-{1}", No, LineNo);
        }
        #endregion


        #endregion

        #region ERP相关信息

        #region ERP单据Id ErpOrderId
        /// <summary>
        /// ERP单据Id
        /// </summary>
        [Label("ERP单据Id")]
        public static readonly Property<double?> ErpOrderIdProperty = P<DeliveryPlan>.Register(e => e.ErpOrderId);

        /// <summary>
        /// ERP单据Id
        /// </summary>
        public double? ErpOrderId
        {
            get { return this.GetProperty(ErpOrderIdProperty); }
            set { this.SetProperty(ErpOrderIdProperty, value); }
        }
        #endregion

        #region Erp库存组织名称 ErpOrganizationName
        /// <summary>
        /// Erp库存组织名称
        /// </summary>
        [Label("Erp库存组织名称")]
        public static readonly Property<string> ErpOrganizationNameProperty = P<DeliveryPlan>.Register(e => e.ErpOrganizationName);

        /// <summary>
        /// Erp库存组织名称
        /// </summary>
        public string ErpOrganizationName
        {
            get { return this.GetProperty(ErpOrganizationNameProperty); }
            set { this.SetProperty(ErpOrganizationNameProperty, value); }
        }
        #endregion

        #region Erp业务实体名词 ErpOrgName
        /// <summary>
        /// Erp业务实体名词
        /// </summary>
        [Label("Erp业务实体名词")]
        public static readonly Property<string> ErpOrgNameProperty = P<DeliveryPlan>.Register(e => e.ErpOrgName);

        /// <summary>
        /// Erp业务实体名词
        /// </summary>
        public string ErpOrgName
        {
            get { return this.GetProperty(ErpOrgNameProperty); }
            set { this.SetProperty(ErpOrgNameProperty, value); }
        }
        #endregion

        #region Erp明细行Id或主键值 ErpDetailId
        /// <summary>
        /// Erp明细行Id或主键值
        /// </summary>
        [Label("Erp明细行Id或主键值")]
        public static readonly Property<string> ErpDetailIdProperty = P<DeliveryPlan>.Register(e => e.ErpDetailId);

        /// <summary>
        /// Erp明细行Id或主键值
        /// </summary>
        public string ErpDetailId
        {
            get { return this.GetProperty(ErpDetailIdProperty); }
            set { this.SetProperty(ErpDetailIdProperty, value); }
        }
        #endregion

        #region Erp工单号 ErpWoNo
        /// <summary>
        /// Erp工单号
        /// </summary>
        [Label("Erp工单号")]
        public static readonly Property<string> ErpWoNoProperty = P<DeliveryPlan>.Register(e => e.ErpWoNo);

        /// <summary>
        /// Erp工单号
        /// </summary>
        public string ErpWoNo
        {
            get { return this.GetProperty(ErpWoNoProperty); }
            set { this.SetProperty(ErpWoNoProperty, value); }
        }
        #endregion

        #region 预计最早发货时间 ScheduleShipDate
        /// <summary>
        /// 预计最早发货时间
        /// </summary>
        [Label("预计最早发货时间")]
        public static readonly Property<string> ScheduleShipDateProperty = P<DeliveryPlan>.Register(e => e.ScheduleShipDate);

        /// <summary>
        /// 预计最早发货时间
        /// </summary>
        public string ScheduleShipDate
        {
            get { return this.GetProperty(ScheduleShipDateProperty); }
            set { this.SetProperty(ScheduleShipDateProperty, value); }
        }
        #endregion


        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<DeliveryPlan>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 调拨模式 AllotModel
        /// <summary>
        /// 调拨模式
        /// </summary>
        [Label("调拨模式")]
        public static readonly Property<AllotModel?> AllotModelProperty = P<DeliveryPlan>.Register(e => e.AllotModel);

        /// <summary>
        /// 调拨模式
        /// </summary>
        public AllotModel? AllotModel
        {
            get { return GetProperty(AllotModelProperty); }
            set { SetProperty(AllotModelProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 发货计划 实体配置
    /// </summary>
    internal class DeliveryPlanConfig : EntityConfig<DeliveryPlan>
    {
        /// <summary>
        /// 增加验证逻辑
        /// </summary>
        /// <param name="rules">验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    DeliveryPlan.NoProperty,
                    DeliveryPlan.LineNoProperty,
                    DeliveryPlan.OrderTypeProperty,
                    DeliveryPlan.SourceTypeProperty,
                },
                MessageBuilder = o =>
                {
                    return "已经存在相同的计划明细行号".L10N();
                }
            }, new RuleMeta { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DELIVERY_PLAN").MapAllProperties();
            Meta.Property(DeliveryPlan.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.Property(DeliveryPlan.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.Property(DeliveryPlan.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}