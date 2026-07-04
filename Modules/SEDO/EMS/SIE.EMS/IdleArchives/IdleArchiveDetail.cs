using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.IdleArchives
{
    /// <summary>
    /// 设备清单
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("设备清单")]
    public partial class IdleArchiveDetail : DataEntity
    {
        #region 闲置/封存期限 Deadline
        /// <summary>
        /// 闲置/封存期限
        /// </summary>
        [Label("闲置/封存期限")]
        public static readonly Property<DateTime?> DeadlineProperty = P<IdleArchiveDetail>.Register(e => e.Deadline);

        /// <summary>
        /// 闲置/封存期限
        /// </summary>
        public DateTime? Deadline
        {
            get { return GetProperty(DeadlineProperty); }
            set { SetProperty(DeadlineProperty, value); }
        }
        #endregion

        #region 位置 Location
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> LocationProperty = P<IdleArchiveDetail>.Register(e => e.Location);

        /// <summary>
        /// 位置
        /// </summary>
        public string Location
        {
            get { return GetProperty(LocationProperty); }
            set { SetProperty(LocationProperty, value); }
        }
        #endregion

        #region 保管人 Keeper
        /// <summary>
        /// 保管人Id
        /// </summary>
        [Label("保管人")]
        public static readonly IRefIdProperty KeeperIdProperty = P<IdleArchiveDetail>.RegisterRefId(e => e.KeeperId, ReferenceType.Normal);

        /// <summary>
        /// 保管人Id
        /// </summary>
        public double? KeeperId
        {
            get { return (double?)GetRefNullableId(KeeperIdProperty); }
            set { SetRefNullableId(KeeperIdProperty, value); }
        }

        /// <summary>
        /// 保管人
        /// </summary>
        public static readonly RefEntityProperty<Employee> KeeperProperty = P<IdleArchiveDetail>.RegisterRef(e => e.Keeper, KeeperIdProperty);

        /// <summary>
        /// 保管人
        /// </summary>
        public Employee Keeper
        {
            get { return GetRefEntity(KeeperProperty); }
            set { SetRefEntity(KeeperProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty = P<IdleArchiveDetail>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ResourceProperty = P<IdleArchiveDetail>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<IdleArchiveDetail>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<IdleArchiveDetail>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<IdleArchiveDetail>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)GetRefNullableId(WorkshopIdProperty); }
            set { SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<IdleArchiveDetail>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 闲置与封存 IdleArchive
        /// <summary>
        /// 闲置与封存Id
        /// </summary>
        [Label("闲置与封存")]
        public static readonly IRefIdProperty IdleArchiveIdProperty = P<IdleArchiveDetail>.RegisterRefId(e => e.IdleArchiveId, ReferenceType.Parent);

        /// <summary>
        /// 闲置与封存Id
        /// </summary>
        public double IdleArchiveId
        {
            get { return (double)GetRefId(IdleArchiveIdProperty); }
            set { SetRefId(IdleArchiveIdProperty, value); }
        }

        /// <summary>
        /// 闲置与封存
        /// </summary>
        public static readonly RefEntityProperty<IdleArchive> IdleArchiveProperty = P<IdleArchiveDetail>.RegisterRef(e => e.IdleArchive, IdleArchiveIdProperty);

        /// <summary>
        /// 闲置与封存
        /// </summary>
        public IdleArchive IdleArchive
        {
            get { return GetRefEntity(IdleArchiveProperty); }
            set { SetRefEntity(IdleArchiveProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<IdleArchiveDetail>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<IdleArchiveDetail>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备型号编码 ModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<IdleArchiveDetail>.RegisterView(e => e.ModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 设备型号名称 ModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> ModelNameProperty = P<IdleArchiveDetail>.RegisterView(e => e.ModelName, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 设备类型 EquipTypeCode
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeCodeProperty = P<IdleArchiveDetail>.RegisterView(e => e.EquipTypeCode, p => p.EquipAccount.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
        }
        #endregion

        #region 设备类型名称 EquipTypeName
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeNameProperty = P<IdleArchiveDetail>.RegisterView(e => e.EquipTypeName, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion


        #region 技术规格 Specifications
        /// <summary>
        /// 技术规格
        /// </summary>
        [Label("技术规格")]
        public static readonly Property<string> SpecificationsProperty = P<IdleArchiveDetail>.RegisterView(e => e.Specifications, p => p.EquipAccount.EquipModel.Specifications);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string Specifications
        {
            get { return this.GetProperty(SpecificationsProperty); }
        }
        #endregion


        #region 固定资产编码 FixedAssetsAccountCode
        /// <summary>
        /// 固定资产编码
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> FixedAssetsAccountCodeProperty
            = P<IdleArchiveDetail>.RegisterView(e => e.FixedAssetsAccountCode, p => p.EquipAccount.FixedAssetsAccount.Code);

        /// <summary>
        /// 固定资产编码
        /// </summary>
        public string FixedAssetsAccountCode
        {
            get { return this.GetProperty(FixedAssetsAccountCodeProperty); }
        }
        #endregion

        #region 固定资产名称 FixedAssetsAccountName
        /// <summary>
        /// 固定资产名称
        /// </summary>
        [Label("固定资产名称")]
        public static readonly Property<string> FixedAssetsAccountNameProperty
            = P<IdleArchiveDetail>.RegisterView(e => e.FixedAssetsAccountName, p => p.EquipAccount.FixedAssetsAccount.Name);

        /// <summary>
        /// 固定资产名称
        /// </summary>
        public string FixedAssetsAccountName
        {
            get { return this.GetProperty(FixedAssetsAccountNameProperty); }
        }
        #endregion


        #region 父级工厂Id FactoryId
        /// <summary>
        /// 父级工厂Id
        /// </summary>
        [Label("父级工厂")]
        public static readonly Property<double> FactoryIdProperty = P<IdleArchiveDetail>.RegisterView(e => e.FactoryId,p=>p.IdleArchive.FactoryId);

        /// <summary>
        /// 父级工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
            set { this.SetProperty(FactoryIdProperty, value); }
        }
        #endregion



        #region 业务类型 IdleArchiveType
        /// <summary>
        /// 业务类型
        /// </summary>
        [Label("业务类型")]
        public static readonly Property<IdleArchiveType> IdleArchiveTypeProperty = P<IdleArchiveDetail>.RegisterView(e => e.IdleArchiveType, p => p.IdleArchive.IdleArchiveType);

        /// <summary>
        /// 业务类型
        /// </summary>
        public IdleArchiveType IdleArchiveType
        {
            get { return this.GetProperty(IdleArchiveTypeProperty); }
            set { this.SetProperty(IdleArchiveTypeProperty, value); }
        }
        #endregion


        #region 管理部门 DepartmentId
        /// <summary>
        /// 管理部门
        /// </summary>
        [Label("管理部门")]
        public static readonly Property<double> DepartmentIdProperty = P<IdleArchiveDetail>.RegisterView(e => e.DepartmentId, p => p.IdleArchive.DepartmentId);

        /// <summary>
        /// 管理部门
        /// </summary>
        public double DepartmentId
        {
            get { return this.GetProperty(DepartmentIdProperty); }
            set { this.SetProperty(DepartmentIdProperty, value); }
        }
        #endregion

        #region 使用部门 UseDepartmentId
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<double> UseDepartmentIdProperty = P<IdleArchiveDetail>.RegisterView(e => e.UseDepartmentId, p => p.IdleArchive.UseDepartmentId);

        /// <summary>
        /// 使用部门
        /// </summary>
        public double UseDepartmentId
        {
            get { return this.GetProperty(UseDepartmentIdProperty); }
        }
        #endregion

        #region 设备类别 TypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> TypeCategoryProperty = P<IdleArchiveDetail>.RegisterView(e => e.TypeCategory, p => p.IdleArchive.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory
        {
            get { return this.GetProperty(TypeCategoryProperty); }
        }
        #endregion

        #region 固定资产 IsAsset
        /// <summary>
        /// 固定资产
        /// </summary>
        [Label("固定资产")]
        public static readonly Property<bool> IsAssetProperty = P<IdleArchiveDetail>.RegisterView(e => e.IsAsset, p => p.IdleArchive.IsAsset);

        /// <summary>
        /// 固定资产
        /// </summary>
        public bool IsAsset
        {
            get { return this.GetProperty(IsAssetProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备清单 实体配置
    /// </summary>
    internal class IdleArchiveDetailConfig : EntityConfig<IdleArchiveDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_IDLE_ARCH_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}