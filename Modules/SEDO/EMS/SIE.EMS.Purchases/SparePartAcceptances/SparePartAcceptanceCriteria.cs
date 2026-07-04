using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备件验收查询实体")]
    public partial class SparePartAcceptanceCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<SparePartAcceptanceCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<SparePartAcceptanceCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<SparePartAcceptanceCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<SparePartAcceptanceCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 验收单号 AcceptanceNo
        /// <summary>
        /// 验收单号
        /// </summary>
        [Label("验收单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> AcceptanceNoProperty = P<SparePartAcceptanceCriteria>.Register(e => e.AcceptanceNo);

        /// <summary>
        /// 验收单号
        /// </summary>
        public string AcceptanceNo
        {
            get { return GetProperty(AcceptanceNoProperty); }
            set { SetProperty(AcceptanceNoProperty, value); }
        }
        #endregion

        #region 接收单号 ReceiveNo
        /// <summary>
        /// 接收单号
        /// </summary>
        [Label("接收单号")]
        public static readonly Property<string> ReceiveNoProperty = P<SparePartAcceptanceCriteria>.Register(e => e.ReceiveNo);

        /// <summary>
        /// 接收单号
        /// </summary>
        public string ReceiveNo
        {
            get { return GetProperty(ReceiveNoProperty); }
            set { SetProperty(ReceiveNoProperty, value); }
        }
        #endregion

        #region 接收类型 ReceiveType
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<ReceiveType?> ReceiveTypeProperty = P<SparePartAcceptanceCriteria>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType? ReceiveType
        {
            get { return GetProperty(ReceiveTypeProperty); }
            set { SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartAcceptanceCriteria>.Register(e => e.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<SparePartAcceptanceCriteria>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 备件管控方式 ControlMethod
        /// <summary>
        /// 备件管控方式
        /// </summary>
        [Label("备件管控方式")]
        public static readonly Property<ControlMethod?> ControlMethodProperty = P<SparePartAcceptanceCriteria>.Register(e => e.ControlMethod);

        /// <summary>
        /// 备件管控方式
        /// </summary>
        public ControlMethod? ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { this.SetProperty(ControlMethodProperty, value); }
        }
        #endregion


        #region 采购订单号 PurchaseOrderNo
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<SparePartAcceptanceCriteria>.Register(e => e.PurchaseOrderNo);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
            set { this.SetProperty(PurchaseOrderNoProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<SparePartAcceptanceCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<SparePartAcceptanceCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<SparePartAcceptanceCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<SparePartAcceptanceCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SparePartAcceptanceController>().CriteriaSparePartAcceptances(this);
        }
    }
}
