using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.FixtureRecords
{
    /// <summary>
	/// 工治具出入库记录
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureRecordCriteria))]
    [Label("工治具出入库记录")]
    public partial class FixtureRecord : DataEntity
    {
        #region 任务编号 Code
        /// <summary>
        /// 任务编号
        /// </summary>
        [Label("任务编号")]
        public static readonly Property<string> CodeProperty = P<FixtureRecord>.Register(e => e.Code);

        /// <summary>
        /// 任务编号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> QtyProperty = P<FixtureRecord>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 任务执行时间 ComplyDate
        /// <summary>
        /// 任务执行时间
        /// </summary>
        [Label("任务执行时间")]
        public static readonly Property<DateTime?> ComplyDateProperty = P<FixtureRecord>.Register(e => e.ComplyDate);

        /// <summary>
        /// 任务执行时间
        /// </summary>
        public DateTime? ComplyDate
        {
            get { return GetProperty(ComplyDateProperty); }
            set { SetProperty(ComplyDateProperty, value); }
        }
        #endregion

        #region 单据创建时间 ApplyDate
        /// <summary>
        /// 单据创建时间
        /// </summary>
        [Label("单据创建时间")]
        public static readonly Property<DateTime> ApplyDateProperty = P<FixtureRecord>.Register(e => e.ApplyDate);

        /// <summary>
        /// 单据创建时间
        /// </summary>
        public DateTime ApplyDate
        {
            get { return GetProperty(ApplyDateProperty); }
            set { SetProperty(ApplyDateProperty, value); }
        }
        #endregion

        #region 任务创建人 ApplyBy
        /// <summary>
        /// 任务创建人Id
        /// </summary>
        [Label("任务创建人")]
        public static readonly IRefIdProperty ApplyByIdProperty = P<FixtureRecord>.RegisterRefId(e => e.ApplyById, ReferenceType.Normal);

        /// <summary>
        /// 任务创建人Id
        /// </summary>
        public double ApplyById
        {
            get { return (double)GetRefId(ApplyByIdProperty); }
            set { SetRefId(ApplyByIdProperty, value); }
        }

        /// <summary>
        /// 任务创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApplyByProperty = P<FixtureRecord>.RegisterRef(e => e.ApplyBy, ApplyByIdProperty);

        /// <summary>
        /// 任务创建人
        /// </summary>
        public Employee ApplyBy
        {
            get { return GetRefEntity(ApplyByProperty); }
            set { SetRefEntity(ApplyByProperty, value); }
        }
        #endregion

        #region 类型 RecordType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<RecordType> RecordTypeProperty = P<FixtureRecord>.Register(e => e.RecordType);

        /// <summary>
        /// 类型
        /// </summary>
        public RecordType RecordType
        {
            get { return GetProperty(RecordTypeProperty); }
            set { SetProperty(RecordTypeProperty, value); }
        }
        #endregion

        #region 工治具台帐 FixtureAccount
        /// <summary>
        /// 工治具台帐Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<FixtureRecord>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

        /// <summary>
        /// 工治具台帐Id
        /// </summary>
        public double FixtureAccountId
        {
            get { return (double)GetRefId(FixtureAccountIdProperty); }
            set { SetRefId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// 工治具台帐
        /// </summary>
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<FixtureRecord>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具台帐
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 任务执行人 ComplyBy
        /// <summary>
        /// 任务执行人Id
        /// </summary>
        [Label("任务执行人")]
        public static readonly IRefIdProperty ComplyByIdProperty = P<FixtureRecord>.RegisterRefId(e => e.ComplyById, ReferenceType.Normal);

        /// <summary>
        /// 任务执行人Id
        /// </summary>
        public double? ComplyById
        {
            get { return (double?)GetRefNullableId(ComplyByIdProperty); }
            set { SetRefNullableId(ComplyByIdProperty, value); }
        }

        /// <summary>
        /// 任务执行人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ComplyByProperty = P<FixtureRecord>.RegisterRef(e => e.ComplyBy, ComplyByIdProperty);

        /// <summary>
        /// 任务执行人
        /// </summary>
        public Employee ComplyBy
        {
            get { return GetRefEntity(ComplyByProperty); }
            set { SetRefEntity(ComplyByProperty, value); }
        }
        #endregion

        #region 业务类型 BusinessType
        /// <summary>
        /// 业务类型
        /// </summary>
        [Label("业务类型")]
        public static readonly Property<BusinessType> BusinessTypeProperty = P<FixtureRecord>.Register(e => e.BusinessType);

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType BusinessType
        {
            get { return GetProperty(BusinessTypeProperty); }
            set { SetProperty(BusinessTypeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty FixtureWarehouseIdProperty = P<FixtureRecord>.RegisterRefId(e => e.FixtureWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? FixtureWarehouseId
        {
            get { return (double?)GetRefNullableId(FixtureWarehouseIdProperty); }
            set { SetRefNullableId(FixtureWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> FixtureWarehouseProperty = P<FixtureRecord>.RegisterRef(e => e.Warehouse, FixtureWarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(FixtureWarehouseProperty); }
            set { SetRefEntity(FixtureWarehouseProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        public static readonly IRefIdProperty FixtureStorageLocationIdProperty = P<FixtureRecord>.RegisterRefId(e => e.FixtureStorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? FixtureStorageLocationId
        {
            get { return (double?)GetRefNullableId(FixtureStorageLocationIdProperty); }
            set { SetRefNullableId(FixtureStorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> FixtureStorageLocationProperty = P<FixtureRecord>.RegisterRef(e => e.StorageLocation, FixtureStorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(FixtureStorageLocationProperty); }
            set { SetRefEntity(FixtureStorageLocationProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<FixtureRecord>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { this.SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<FixtureRecord>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 库位编码 LocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> LocationCodeProperty = P<FixtureRecord>.RegisterView(e => e.LocationCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocationCode
        {
            get { return this.GetProperty(LocationCodeProperty); }
            set { this.SetProperty(LocationCodeProperty, value); }
        }
        #endregion

        #region 库位名称 LocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> LocationNameProperty = P<FixtureRecord>.RegisterView(e => e.LocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName
        {
            get { return this.GetProperty(LocationNameProperty); }
            set { this.SetProperty(LocationNameProperty, value); }
        }
        #endregion

        #region 工治具ID AccountCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> AccountCodeProperty = P<FixtureRecord>.RegisterView(e => e.AccountCode, p => p.FixtureAccount.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string AccountCode
        {
            get { return this.GetProperty(AccountCodeProperty); }
            set { this.SetProperty(AccountCodeProperty, value); }
        }
        #endregion

        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<FixtureRecord>.RegisterView(e => e.EncodeCode, p => p.FixtureAccount.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
        }
        #endregion

        #region 工治具型号编码 ModelCode
        /// <summary>
        /// 工治具型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureRecord>.RegisterView(e => e.ModelCode, p => p.FixtureAccount.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 工治具型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 工治具型号名称 ModelName
        /// <summary>
        /// 工治具型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureRecord>.RegisterView(e => e.ModelName, p => p.FixtureAccount.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 工治具型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty = P<FixtureRecord>.RegisterView(e => e.FixtureType, p => p.FixtureAccount.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 治具出入库记录 实体配置
    /// </summary>
    internal class FixtureRecordConfig : EntityConfig<FixtureRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_FIXTURE_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
