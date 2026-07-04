using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Kit.MES.SingleLabels
{
    /// <summary>
    /// 单体条码
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("单体条码")]
    public partial class SingleLabel : DataEntity
    {
        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("条码")]
        [MaxLength(30)]
        public static readonly Property<string> SnProperty = P<SingleLabel>.Register(e => e.Sn);

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 批次标签 BatchCode
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("批次标签")]
        public static readonly Property<string> BatchCodeProperty = P<SingleLabel>.Register(e => e.BatchCode);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchCode
        {
            get { return GetProperty(BatchCodeProperty); }
            set { SetProperty(BatchCodeProperty, value); }
        }
        #endregion

        #region 打印时间 PrintDate
        /// <summary>
        /// 打印时间
        /// </summary>
        [Label("打印时间")]
        public static readonly Property<DateTime?> PrintDateProperty = P<SingleLabel>.Register(e => e.PrintDate);

        /// <summary>
        /// 打印时间
        /// </summary>
        public DateTime? PrintDate
        {
            get { return GetProperty(PrintDateProperty); }
            set { SetProperty(PrintDateProperty, value); }
        }
        #endregion

        #region 来源ID SourceId
        /// <summary>
        /// 来源ID
        /// </summary>
        [Label("来源ID")]
        public static readonly Property<string> SourceIdProperty = P<SingleLabel>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID
        /// </summary>
        public string SourceId
        {
            get { return GetProperty(SourceIdProperty); }
            set { SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 来源号 SourceNo
        /// <summary>
        /// 来源号
        /// </summary>
        [Label("来源号")]
        public static readonly Property<string> SourceNoProperty = P<SingleLabel>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源号
        /// </summary>
        public string SourceNo
        {
            get { return GetProperty(SourceNoProperty); }
            set { SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<SingleLabel>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary> 
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<SingleLabel>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 条码来源类型 SourceType
        /// <summary>
        /// 条码来源类型
        /// </summary>
        [Label("条码来源类型")]
        public static readonly Property<SingleLabelSourceType> SourceTypeProperty = P<SingleLabel>.Register(e => e.SourceType);

        /// <summary>
        /// 条码来源类型
        /// </summary>
        public SingleLabelSourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<SingleLabel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<SingleLabel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 标签状态 LabelState
        /// <summary>
        /// 标签状态
        /// </summary>
        [Label("标签状态")]
        public static readonly Property<LabelState> LabelStateProperty = P<SingleLabel>.Register(e => e.LabelState);

        /// <summary>
        /// 标签状态
        /// </summary>
        public LabelState LabelState
        {
            get { return GetProperty(LabelStateProperty); }
            set { SetProperty(LabelStateProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<SingleLabel>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double SupplierId
        {
            get { return (double)GetRefId(SupplierIdProperty); }
            set { SetRefId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<SingleLabel>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<SingleLabel>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<SingleLabel>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<SingleLabel>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<SingleLabel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 员工编码 EmployeeCode
        /// <summary>
        /// 员工编码
        /// </summary>
        [Label("员工编码")]
        public static readonly Property<string> EmployeeCodeProperty = P<SingleLabel>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 员工编码
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 单体条码 实体配置
    /// </summary>
    internal class SingleLabelConfig : EntityConfig<SingleLabel>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_SINGLE_LABEL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(SingleLabel.SnProperty).ColumnMeta.HasIndex();
        }
    }
}