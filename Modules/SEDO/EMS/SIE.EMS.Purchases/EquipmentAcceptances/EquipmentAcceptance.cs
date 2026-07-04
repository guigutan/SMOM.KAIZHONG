using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.CSM.Suppliers;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using SIE.Equipments.Configs;
using SIE.DataAuth;
using SIE.Resources.Employees;
using SIE.EMS.DataAuth;
using SIE.Common;

namespace SIE.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 设备开箱验收
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备开箱验收")]
    [ConditionQueryType(typeof(EquipmentAcceptanceCriteria))]
    [DisplayMember(nameof(AcceptanceNo))]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(NoConfig), "设备开箱验收单号配置项", "设备开箱验收单号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class EquipmentAcceptance : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty
            = P<EquipmentAcceptance>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)GetRefId(FactoryIdProperty); }
            set { SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty
            = P<EquipmentAcceptance>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty
            = P<EquipmentAcceptance>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty
            = P<EquipmentAcceptance>.RegisterRef(e => e.Department, DepartmentIdProperty);

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
        public static readonly Property<string> AcceptanceNoProperty = P<EquipmentAcceptance>.Register(e => e.AcceptanceNo);

        /// <summary>
        /// 验收单号
        /// </summary>
        public string AcceptanceNo
        {
            get { return GetProperty(AcceptanceNoProperty); }
            set { SetProperty(AcceptanceNoProperty, value); }
        }
        #endregion

        #region 接收数量 ReceiveQty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        public static readonly Property<int> ReceiveQtyProperty = P<EquipmentAcceptance>.Register(e => e.ReceiveQty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public int ReceiveQty
        {
            get { return GetProperty(ReceiveQtyProperty); }
            set { SetProperty(ReceiveQtyProperty, value); }
        }
        #endregion

        #region 合格数量 PassQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        public static readonly Property<int> PassQtyProperty = P<EquipmentAcceptance>.Register(e => e.PassQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public int PassQty
        {
            get { return GetProperty(PassQtyProperty); }
            set { SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 UnqualifiedQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<int> UnqualifiedQtyProperty = P<EquipmentAcceptance>.Register(e => e.UnqualifiedQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public int UnqualifiedQty
        {
            get { return GetProperty(UnqualifiedQtyProperty); }
            set { SetProperty(UnqualifiedQtyProperty, value); }
        }
        #endregion

        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipmentAcceptance>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)GetRefId(EquipModelIdProperty); }
            set { SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipmentAcceptance>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<EquipmentAcceptance>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        public static readonly IRefIdProperty SupplierIdProperty = P<EquipmentAcceptance>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<EquipmentAcceptance>.RegisterRef(e => e.Supplier, SupplierIdProperty);

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
        public static readonly IRefIdProperty CustomerIdProperty
            = P<EquipmentAcceptance>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty
            = P<EquipmentAcceptance>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 接收类型 ReceiveType
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<EquipmentAcceptance>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return GetProperty(ReceiveTypeProperty); }
            set { SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

        #region 设备开箱验收明细列表 DetailList
        /// <summary>
        /// 设备开箱验收明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipmentAcceptanceDetail>> DetailListProperty = P<EquipmentAcceptance>.RegisterList(e => e.DetailList);
        /// <summary>
        /// 设备开箱验收明细列表
        /// </summary>
        public EntityList<EquipmentAcceptanceDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 设备开箱验收附件列表 AttachmentList
        /// <summary>
        /// 设备开箱验收附件列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipmentAcceptanceAttachment>> AttachmentListProperty = P<EquipmentAcceptance>.RegisterList(e => e.AttachmentList);
        /// <summary>
        /// 设备开箱验收附件列表
        /// </summary>
        public EntityList<EquipmentAcceptanceAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 验收项目列表 EquipmentAcceptanceItemList
        /// <summary>
        /// 验收项目列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipmentAcceptanceItem>> EquipmentAcceptanceItemListProperty
            = P<EquipmentAcceptance>.RegisterList(e => e.EquipmentAcceptanceItemList);

        /// <summary>
        /// 验收项目列表
        /// </summary>
        public EntityList<EquipmentAcceptanceItem> EquipmentAcceptanceItemList
        {
            get { return this.GetLazyList(EquipmentAcceptanceItemListProperty); }
        }
        #endregion

        #region 视图属性
        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty
            = P<EquipmentAcceptance>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty
            = P<EquipmentAcceptance>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty
            = P<EquipmentAcceptance>.RegisterView(e => e.EquipModelName, p => p.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion
        #endregion

        #region 界面属性不映射数据库
        #region 消息框(界面属性) Message
        /// <summary>
        /// 消息框(界面属性)
        /// </summary>
        [Label("消息框")]
        public static readonly Property<string> MessageProperty = P<EquipmentAcceptance>.Register(e => e.Message);

        /// <summary>
        /// 消息框(界面属性)
        /// </summary>
        public string Message
        {
            get { return this.GetProperty(MessageProperty); }
            set { this.SetProperty(MessageProperty, value); }
        }
        #endregion

        #region 扫描框(界面属性) Sn
        /// <summary>
        /// 扫描框(界面属性)
        /// </summary>
        [Label("扫描框")]
        public static readonly Property<string> SnProperty = P<EquipmentAcceptance>.Register(e => e.Sn);

        /// <summary>
        /// 扫描框(界面属性)
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 验收状态(界面属性) AcceptanceStatus
        /// <summary>
        /// 验收状态(界面属性)
        /// </summary>
        [Label("验收状态")]
        public static readonly Property<InspectionResult?> AcceptanceStatusProperty = P<EquipmentAcceptance>.Register(e => e.AcceptanceStatus);

        /// <summary>
        /// 验收状态(界面属性)
        /// </summary>
        public InspectionResult? AcceptanceStatus
        {
            get { return GetProperty(AcceptanceStatusProperty); }
            set { SetProperty(AcceptanceStatusProperty, value); }
        }
        #endregion

        #region 备注(界面属性) Remark
        /// <summary>
        /// 备注(界面属性)
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipmentAcceptance>.Register(e => e.Remark);

        /// <summary>
        /// 备注(界面属性)
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 设备开箱验收 实体配置
    /// </summary>
    internal class EquipmentAcceptanceConfig : EntityConfig<EquipmentAcceptance>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQP_ACPT").MapAllProperties();
            Meta.Property(EquipmentAcceptance.MessageProperty).DontMapColumn();
            Meta.Property(EquipmentAcceptance.SnProperty).DontMapColumn();
            Meta.Property(EquipmentAcceptance.AcceptanceStatusProperty).DontMapColumn();
            Meta.Property(EquipmentAcceptance.RemarkProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}