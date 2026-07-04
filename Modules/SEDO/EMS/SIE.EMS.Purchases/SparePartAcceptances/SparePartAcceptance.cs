using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.CSM.Suppliers;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SparePartAcceptanceCriteria))]
    [Label("备件验收")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(NoConfig), "备件验收单号配置项", "备件验收单号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class SparePartAcceptance : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<SparePartAcceptance>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<SparePartAcceptance>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<SparePartAcceptance>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<SparePartAcceptance>.RegisterRef(e => e.Department, DepartmentIdProperty);

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
        public static readonly Property<string> AcceptanceNoProperty = P<SparePartAcceptance>.Register(e => e.AcceptanceNo);

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
        [MinValue(0)]
        public static readonly Property<int> ReceiveQtyProperty = P<SparePartAcceptance>.Register(e => e.ReceiveQty);

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
        [MinValue(0)]
        public static readonly Property<int> PassQtyProperty = P<SparePartAcceptance>.Register(e => e.PassQty);

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
        [MinValue(0)]
        public static readonly Property<int> UnqualifiedQtyProperty = P<SparePartAcceptance>.Register(e => e.UnqualifiedQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public int UnqualifiedQty
        {
            get { return GetProperty(UnqualifiedQtyProperty); }
            set { SetProperty(UnqualifiedQtyProperty, value); }
        }
        #endregion

        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件基础数据")]
        public static readonly IRefIdProperty SparePartIdProperty = P<SparePartAcceptance>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<SparePartAcceptance>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件接收 SparePartReceive
        /// <summary>
        /// 备件接收Id
        /// </summary>
        [Label("备件接收")]
        public static readonly IRefIdProperty SparePartReceiveIdProperty = P<SparePartAcceptance>.RegisterRefId(e => e.SparePartReceiveId, ReferenceType.Normal);

        /// <summary>
        /// 备件接收Id
        /// </summary>
        public double SparePartReceiveId
        {
            get { return (double)GetRefId(SparePartReceiveIdProperty); }
            set { SetRefId(SparePartReceiveIdProperty, value); }
        }

        /// <summary>
        /// 备件接收
        /// </summary>
        public static readonly RefEntityProperty<SparePartReceive> SparePartReceiveProperty = P<SparePartAcceptance>.RegisterRef(e => e.SparePartReceive, SparePartReceiveIdProperty);

        /// <summary>
        /// 备件接收
        /// </summary>
        public SparePartReceive SparePartReceive
        {
            get { return GetRefEntity(SparePartReceiveProperty); }
            set { SetRefEntity(SparePartReceiveProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<SparePartAcceptance>.Register(e => e.ApprovalStatus);

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
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<SparePartAcceptance>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<SparePartAcceptance>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 备件验收明细列表 DetailList
        /// <summary>
        /// 备件验收明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartAcceptanceDetail>> DetailListProperty = P<SparePartAcceptance>.RegisterList(e => e.DetailList);
        /// <summary>
        /// 备件验收明细列表
        /// </summary>
        public EntityList<SparePartAcceptanceDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 备件验收项目列表 AcceptanceItemList
        /// <summary>
        /// 备件验收项目列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartAcceptanceItem>> AcceptanceItemListProperty = P<SparePartAcceptance>.RegisterList(e => e.AcceptanceItemList);
        /// <summary>
        /// 备件验收项目列表
        /// </summary>
        public EntityList<SparePartAcceptanceItem> AcceptanceItemList
        {
            get { return this.GetLazyList(AcceptanceItemListProperty); }
        }
        #endregion

        #region 验收附件列表 AttachmentList
        /// <summary>
        /// 验收附件列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartAcceptanceAttachment>> AttachmentListProperty = P<SparePartAcceptance>.RegisterList(e => e.AttachmentList);
        /// <summary>
        /// 验收附件列表
        /// </summary>
        public EntityList<SparePartAcceptanceAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 视图属性
        #region 接收类型 ReceiveType
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<SparePartAcceptance>.RegisterView(e => e.ReceiveType, p => p.SparePartReceive.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return this.GetProperty(ReceiveTypeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<SparePartAcceptance>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<SparePartAcceptance>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<SparePartAcceptance>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<SparePartAcceptance>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 备件验收 实体配置
    /// </summary>
    internal class SparePartAcceptanceConfig : EntityConfig<SparePartAcceptance>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_ACPT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}