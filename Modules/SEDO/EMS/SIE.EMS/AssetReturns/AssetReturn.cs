using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;

namespace SIE.EMS.AssetReturns
{
    /// <summary>
    /// 资产归还
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AssetReturnCriteria))]
    [EntityWithConfig(typeof(NoConfig), "资产归还单号生成规则配置项", "资产归还单号生成规则")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [Label("资产归还")]
    public partial class AssetReturn : DataEntity
    {
        #region 归还单号 ReturnNo
        /// <summary>
        /// 归还单号
        /// </summary>
        [Label("归还单号")]
        public static readonly Property<string> ReturnNoProperty = P<AssetReturn>.Register(e => e.ReturnNo);

        /// <summary>
        /// 归还单号
        /// </summary>
        public string ReturnNo
        {
            get { return GetProperty(ReturnNoProperty); }
            set { SetProperty(ReturnNoProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<AssetReturn>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<AssetReturn>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<AssetReturn>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 借用单号 AssetRequisition
        /// <summary>
        /// 借用单号Id
        /// </summary>
        [Label("借用单号")]
        public static readonly IRefIdProperty AssetRequisitionIdProperty = P<AssetReturn>.RegisterRefId(e => e.AssetRequisitionId, ReferenceType.Normal);

        /// <summary>
        /// 借用单号Id
        /// </summary>
        public double AssetRequisitionId
        {
            get { return (double)GetRefId(AssetRequisitionIdProperty); }
            set { SetRefId(AssetRequisitionIdProperty, value); }
        }

        /// <summary>
        /// 借用单号
        /// </summary>
        public static readonly RefEntityProperty<AssetRequisition> AssetRequisitionProperty = P<AssetReturn>.RegisterRef(e => e.AssetRequisition, AssetRequisitionIdProperty);

        /// <summary>
        /// 借用单号
        /// </summary>
        public AssetRequisition AssetRequisition
        {
            get { return GetRefEntity(AssetRequisitionProperty); }
            set { SetRefEntity(AssetRequisitionProperty, value); }
        }
        #endregion

        #region 归还部门 ApplyDepartment
        /// <summary>
        /// 归还部门Id
        /// </summary>
        [Label("归还部门")]
        public static readonly IRefIdProperty ApplyDepartmentIdProperty = P<AssetReturn>.RegisterRefId(e => e.ApplyDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 归还部门Id
        /// </summary>
        public double ApplyDepartmentId
        {
            get { return (double)GetRefId(ApplyDepartmentIdProperty); }
            set { SetRefId(ApplyDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 归还部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ApplyDepartmentProperty = P<AssetReturn>.RegisterRef(e => e.ApplyDepartment, ApplyDepartmentIdProperty);

        /// <summary>
        /// 归还部门
        /// </summary>
        public Enterprise ApplyDepartment
        {
            get { return GetRefEntity(ApplyDepartmentProperty); }
            set { SetRefEntity(ApplyDepartmentProperty, value); }
        }
        #endregion

        #region 接收部门 LendingDepartment
        /// <summary>
        /// 接收部门Id
        /// </summary>
        [Label("接收部门")]
        public static readonly IRefIdProperty LendingDepartmentIdProperty =
            P<AssetReturn>.RegisterRefId(e => e.LendingDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 接收部门Id
        /// </summary>
        public double? LendingDepartmentId
        {
            get { return (double?)this.GetRefNullableId(LendingDepartmentIdProperty); }
            set { this.SetRefNullableId(LendingDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 接收部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> LendingDepartmentProperty =
            P<AssetReturn>.RegisterRef(e => e.LendingDepartment, LendingDepartmentIdProperty);

        /// <summary>
        /// 接收部门
        /// </summary>
        public Enterprise LendingDepartment
        {
            get { return this.GetRefEntity(LendingDepartmentProperty); }
            set { this.SetRefEntity(LendingDepartmentProperty, value); }
        }
        #endregion

        #region 接收仓库 Warehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssetReturn>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 接收仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetReturn>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 接收人 Employee
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<AssetReturn>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<AssetReturn>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 归还日期 ApplyDate
        /// <summary>
        /// 归还日期
        /// </summary>
        [Label("归还日期")]
        public static readonly Property<DateTime> ApplyDateProperty = P<AssetReturn>.Register(e => e.ApplyDate);

        /// <summary>
        /// 归还日期
        /// </summary>
        public DateTime ApplyDate
        {
            get { return GetProperty(ApplyDateProperty); }
            set { SetProperty(ApplyDateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<AssetReturn>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 外部领用信息字段

        #region 联系人 ContactPerson
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactPersonProperty = P<AssetReturn>.Register(e => e.ContactPerson);

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson
        {
            get { return GetProperty(ContactPersonProperty); }
            set { SetProperty(ContactPersonProperty, value); }
        }
        #endregion

        #region 联系方式 ContactInformation
        /// <summary>
        /// 联系方式
        /// </summary>
        [Label("联系方式")]
        public static readonly Property<string> ContactInformationProperty = P<AssetReturn>.Register(e => e.ContactInformation);

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInformation
        {
            get { return GetProperty(ContactInformationProperty); }
            set { SetProperty(ContactInformationProperty, value); }
        }
        #endregion

        #region 联系地址 Address
        /// <summary>
        /// 联系地址
        /// </summary>
        [Label("联系地址")]
        public static readonly Property<string> AddressProperty = P<AssetReturn>.Register(e => e.Address);

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address
        {
            get { return GetProperty(AddressProperty); }
            set { SetProperty(AddressProperty, value); }
        }
        #endregion

        #region 快递/出厂单号 TrackNo
        /// <summary>
        /// 快递/出厂单号
        /// </summary>
        [Label("快递/出厂单号")]
        public static readonly Property<string> TrackNoProperty = P<AssetReturn>.Register(e => e.TrackNo);

        /// <summary>
        /// 快递/出厂单号
        /// </summary>
        public string TrackNo
        {
            get { return GetProperty(TrackNoProperty); }
            set { SetProperty(TrackNoProperty, value); }
        }
        #endregion

        #endregion

        #region 设备明细 AssetReturnEquipmentList
        /// <summary>
        /// 设备明细
        /// </summary>
        public static readonly ListProperty<EntityList<AssetReturnEquipment>> AssetReturnEquipmentListProperty = P<AssetReturn>.RegisterList(e => e.AssetReturnEquipmentList);
        /// <summary>
        /// 设备明细
        /// </summary>
        public EntityList<AssetReturnEquipment> AssetReturnEquipmentList
        {
            get { return this.GetLazyList(AssetReturnEquipmentListProperty); }
        }
        #endregion

        #region 工治具明细 AssetReturnFixtureList
        /// <summary>
        /// 工治具明细
        /// </summary>
        public static readonly ListProperty<EntityList<AssetReturnFixture>> AssetReturnFixtureListProperty = P<AssetReturn>.RegisterList(e => e.AssetReturnFixtureList);
        /// <summary>
        /// 工治具明细
        /// </summary>
        public EntityList<AssetReturnFixture> AssetReturnFixtureList
        {
            get { return this.GetLazyList(AssetReturnFixtureListProperty); }
        }
        #endregion

        #region 附件 AssetReturnAttachmentList
        /// <summary>
        /// 附件
        /// </summary>
        public static readonly ListProperty<EntityList<AssetReturnAttachment>> AssetReturnAttachmentListProperty = P<AssetReturn>.RegisterList(e => e.AssetReturnAttachmentList);
        /// <summary>
        /// 附件
        /// </summary>
        public EntityList<AssetReturnAttachment> AssetReturnAttachmentList
        {
            get { return this.GetLazyList(AssetReturnAttachmentListProperty); }
        }
        #endregion

        #region 视图属性

        #region 归还对象 AssetObject
        /// <summary>
        /// 归还对象
        /// </summary>
        [Label("归还对象")]
        public static readonly Property<AssetObject> AssetObjectProperty = P<AssetReturn>.RegisterView(e => e.AssetObject, p => p.AssetRequisition.AssetObject);

        /// <summary>
        /// 归还对象
        /// </summary>
        public AssetObject AssetObject
        {
            get { return this.GetProperty(AssetObjectProperty); }
        }
        #endregion

        #region 外部归还 External
        /// <summary>
        /// 外部归还
        /// </summary>
        [Label("外部归还")]
        public static readonly Property<bool> ExternalProperty = P<AssetReturn>.RegisterView(e => e.External, p => p.AssetRequisition.External);

        /// <summary>
        /// 外部归还
        /// </summary>
        public bool External
        {
            get { return this.GetProperty(ExternalProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 资产归还 实体配置
    /// </summary>
    internal class AssetReturnConfig : EntityConfig<AssetReturn>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ASET_RETURN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}