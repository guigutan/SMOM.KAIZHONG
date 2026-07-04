using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Inventory.TransactionProcessing
{
    /// <summary>
    /// 事务交易
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InvTransactionCriteria))]
    [Label("事务交易")]
    public partial class InvTransaction : SIE.Inventory.TransactionProcessing.BaseTransaction
    {
        #region 采购单分配ID PodistributionId
        /// <summary>
        /// 采购单分配ID
        /// </summary>
        [Label("采购单分配ID")]
        public static readonly Property<double?> PodistributionIdProperty = P<InvTransaction>.Register(e => e.PodistributionId);

        /// <summary>
        /// 采购单分配ID
        /// </summary>
        public double? PodistributionId
        {
            get { return GetProperty(PodistributionIdProperty); }
            set { SetProperty(PodistributionIdProperty, value); }
        }
        #endregion

        #region 采购单发运ID PoLinelocationId
        /// <summary>
        /// 采购单发运ID
        /// </summary>
        [Label("采购单发运ID")]
        public static readonly Property<double?> PoLinelocationIdProperty = P<InvTransaction>.Register(e => e.PoLinelocationId);

        /// <summary>
        /// 采购单发运ID
        /// </summary>
        public double? PoLinelocationId
        {
            get { return GetProperty(PoLinelocationIdProperty); }
            set { SetProperty(PoLinelocationIdProperty, value); }
        }
        #endregion
       
        #region 供应商地址ID SupplierAddress
        /// <summary>
        /// 供应商地址ID
        /// </summary>
        [Label("供应商地址ID")]
        public static readonly Property<double?> SupplierAddressProperty = P<InvTransaction>.Register(e => e.SupplierAddress);

        /// <summary>
        /// 供应商地址ID
        /// </summary>
        public double? SupplierAddress
        {
            get { return GetProperty(SupplierAddressProperty); }
            set { SetProperty(SupplierAddressProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty = P<InvTransaction>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<InvTransaction>.RegisterRef(e => e.Customer, CustomerIdProperty);

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
        public static readonly IRefIdProperty SupplierIdProperty = P<InvTransaction>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<InvTransaction>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 生产部门 Enterprise
        /// <summary>
        /// 生产部门Id
        /// </summary>
        [Label("生产部门")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<InvTransaction>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<InvTransaction>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 生产部门
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<InvTransaction>.RegisterView(e => e.CustomerName, e => e.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 物料扩展属性显示名 ItemExtPropName
        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<InvTransaction>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性显示名
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<InvTransaction>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        
    }

    /// <summary>
    /// 库存交易 实体配置
    /// </summary>
    internal class InvTransactionConfig : EntityConfig<InvTransaction>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INV_TRAN").MapAllProperties();
            Meta.Property(InvTransaction.ItemExtPropProperty).ColumnMeta.HasLength(480);
            Meta.Property(InvTransaction.ItemExtPropNameProperty).ColumnMeta.HasLength(2000);
            Meta.IndexGroupOnProperties(InvTransaction.FromWarehouseIdProperty, InvTransaction.TransactionDateProperty);
            Meta.Property(InvTransaction.BillNoProperty).MapColumn().HasIndex(IndexTypeMeta.Indexed);
            Meta.IndexGroupOnProperties(InvTransaction.ToWarehouseIdProperty, InvTransaction.ItemIdProperty);
            Meta.IndexGroupOnProperties(InvTransaction.TransactionDateProperty, InvTransaction.ItemIdProperty);
            Meta.EnablePhantoms();
        }
    }
}