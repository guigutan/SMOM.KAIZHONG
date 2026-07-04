using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 分配仓库规则
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AssignWarehouseRuleCriteria))]
    [Label("分配仓库规则")]
    public partial class AssignWarehouseRule : DataEntity
    {
        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [MinValue(1)]
        [Label("优先级")]
        public static readonly Property<int> PriorityProperty = P<AssignWarehouseRule>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get { return GetProperty(PriorityProperty); }
            set { SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 单据类型 OrderType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("单据类型")]
        public static readonly Property<OrderType> OrderTypeProperty = P<AssignWarehouseRule>.Register(e => e.OrderType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public OrderType OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 基本分类 ItemType
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<ItemType> ItemTypeProperty = P<AssignWarehouseRule>.Register(e => e.ItemType);

        /// <summary>
        /// 基本分类
        /// </summary>
        public ItemType ItemType
        {
            get { return GetProperty(ItemTypeProperty); }
            set { SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        #region 库存类别 ItemCategory
        /// <summary>
        /// 库存类别Id
        /// </summary>
        [Label("库存类别")]
        public static readonly IRefIdProperty ItemCategoryIdProperty = P<AssignWarehouseRule>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 库存类别Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)GetRefNullableId(ItemCategoryIdProperty); }
            set { SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 库存类别
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty = P<AssignWarehouseRule>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 库存分类
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return GetRefEntity(ItemCategoryProperty); }
            set { SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 匹配仓库  Warehouse
        /// <summary>
        /// 匹配仓库Id
        /// </summary>
        [Label("匹配仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssignWarehouseRule>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 匹配仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 匹配仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssignWarehouseRule>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 匹配仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty = P<AssignWarehouseRule>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<AssignWarehouseRule>.RegisterRef(e => e.Customer, CustomerIdProperty);

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
        public static readonly IRefIdProperty SupplierIdProperty = P<AssignWarehouseRule>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<AssignWarehouseRule>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 部门 Enterprise
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<AssignWarehouseRule>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? EnterpriseId
        {
            get { return (double?)GetRefNullableId(EnterpriseIdProperty); }
            set { SetRefNullableId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<AssignWarehouseRule>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<AssignWarehouseRule>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<AssignWarehouseRule>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 分配仓库规则 实体配置
    /// </summary>
    internal class AssignWarehouseRuleConfig : EntityConfig<AssignWarehouseRule>
    {
        /// <summary>
        /// 增加验证逻辑
        /// </summary>
        /// <param name="rules">验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);          
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ASSIGN_WH_RULE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}