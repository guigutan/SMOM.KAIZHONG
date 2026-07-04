using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 周转工具型号
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("周转工具型号")]
    [DisplayMember(nameof(TurnoverToolModel.Code))]
    public partial class TurnoverToolModel : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<TurnoverToolModel>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<TurnoverToolModel>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 ToolType
        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        [Label("类型")]
        public static readonly Property<string> ToolTypeProperty = P<TurnoverToolModel>.Register(e => e.ToolType);

        /// <summary>
        /// 类型
        /// </summary>
        public string ToolType
        {
            get { return GetProperty(ToolTypeProperty); }
            set { SetProperty(ToolTypeProperty, value); }
        }
        #endregion

        #region 默认容量 DefaultCapacity
        /// <summary>
        /// 默认容量
        /// </summary>
        [MinValue(1)]
        [Label("默认容量")]
        public static readonly Property<int> DefaultCapacityProperty = P<TurnoverToolModel>.Register(e => e.DefaultCapacity);

        /// <summary>
        /// 默认容量
        /// </summary>
        public int DefaultCapacity
        {
            get { return GetProperty(DefaultCapacityProperty); }
            set { SetProperty(DefaultCapacityProperty, value); }
        }
        #endregion

        #region 专用容器 IsDedicated
        /// <summary>
        /// 专用容器
        /// </summary>
        [Label("专用容器")]
        public static readonly Property<bool> IsDedicatedProperty = P<TurnoverToolModel>.Register(e => e.IsDedicated);

        /// <summary>
        /// 专用容器
        /// </summary>
        public bool IsDedicated
        {
            get { return GetProperty(IsDedicatedProperty); }
            set { SetProperty(IsDedicatedProperty, value); }
        }
        #endregion

        #region 长度(cm) Length
        /// <summary>
        /// 长度(cm)
        /// </summary>
        [Label("长度(cm)")]
        public static readonly Property<decimal?> LengthProperty = P<TurnoverToolModel>.Register(e => e.Length);

        /// <summary>
        /// 长度(cm)
        /// </summary>
        public decimal? Length
        {
            get { return GetProperty(LengthProperty); }
            set { SetProperty(LengthProperty, value); }
        }
        #endregion

        #region 宽(cm) Width
        /// <summary>
        /// 宽(cm)
        /// </summary>
        [Label("宽(cm)")]
        public static readonly Property<decimal?> WidthProperty = P<TurnoverToolModel>.Register(e => e.Width);

        /// <summary>
        /// 宽(cm)
        /// </summary>
        public decimal? Width
        {
            get { return GetProperty(WidthProperty); }
            set { SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 高(cm) Height
        /// <summary>
        /// 高(cm)
        /// </summary>
        [Label("高(cm)")]
        public static readonly Property<decimal?> HeightProperty = P<TurnoverToolModel>.Register(e => e.Height);

        /// <summary>
        /// 高(cm)
        /// </summary>
        public decimal? Height
        {
            get { return GetProperty(HeightProperty); }
            set { SetProperty(HeightProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        public static readonly IRefIdProperty SupplierIdProperty = P<TurnoverToolModel>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<TurnoverToolModel>.RegisterRef(e => e.Supplier, SupplierIdProperty);

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
        public static readonly IRefIdProperty CustomerIdProperty = P<TurnoverToolModel>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<TurnoverToolModel>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 产品容量列表 ProductList
        /// <summary>
        /// 产品容量列表
        /// </summary>
        public static readonly ListProperty<EntityList<TurnoverToolModelInProduct>> ProductListProperty = P<TurnoverToolModel>.RegisterList(e => e.ProductList);
        /// <summary>
        /// 产品容量列表
        /// </summary>
        public EntityList<TurnoverToolModelInProduct> ProductList
        {
            get { return this.GetLazyList(ProductListProperty); }
        }
        #endregion

        #region 注册视图

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<TurnoverToolModel>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<TurnoverToolModel>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 周转工具型号 实体配置
    /// </summary>
    internal class TurnoverToolModelConfig : EntityConfig<TurnoverToolModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_TURN_TM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}