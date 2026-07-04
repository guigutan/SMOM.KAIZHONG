using SIE.Domain;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.FixtureDemands
{
    /// <summary>
	/// 出库明细
	/// </summary>
	[ChildEntity, Serializable]
    [Label("出库明细")]
    public partial class FixtureUnload : DataEntity
    {
        #region 出库数量 UnloadQty
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        public static readonly Property<int> UnloadQtyProperty
            = P<FixtureUnload>.Register(e => e.UnloadQty);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int UnloadQty
        {
            get { return GetProperty(UnloadQtyProperty); }
            set { SetProperty(UnloadQtyProperty, value); }
        }
        #endregion

        #region TurnoverToolCode
        /// <summary>
        /// 关联载具
        /// </summary>
        [Label("关联载具")]
        public static readonly Property<string> TurnoverToolCodeProperty
            = P<FixtureUnload>.Register(e => e.TurnoverToolCode);

        /// <summary>
        /// 关联载具
        /// </summary>
        public string TurnoverToolCode
        {
            get { return GetProperty(TurnoverToolCodeProperty); }
            set { SetProperty(TurnoverToolCodeProperty, value); }
        }
        #endregion

        #region 执行时间 UnloadDate
        /// <summary>
        /// 执行时间
        /// </summary>
        [Label("执行时间")]
        public static readonly Property<DateTime> UnloadDateProperty
            = P<FixtureUnload>.Register(e => e.UnloadDate);

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime UnloadDate
        {
            get { return GetProperty(UnloadDateProperty); }
            set { SetProperty(UnloadDateProperty, value); }
        }
        #endregion

        #region 领用状态 State
        /// <summary>
        /// 领用状态
        /// </summary>
        [Label("领用状态")]
        public static readonly Property<ReceiveState> StateProperty
            = P<FixtureUnload>.Register(e => e.State);

        /// <summary>
        /// 领用状态
        /// </summary>
        public ReceiveState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工治具台账 FixtureAccount
        /// <summary>
        /// 工治具台账Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty
            = P<FixtureUnload>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

        /// <summary>
        /// 工治具台账Id
        /// </summary>
        public double FixtureAccountId
        {
            get { return (double)GetRefId(FixtureAccountIdProperty); }
            set { SetRefId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// 工治具台账
        /// </summary>
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty
            = P<FixtureUnload>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具台账
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 出库人员 UnloadBy
        /// <summary>
        /// 出库人员Id
        /// </summary>
        public static readonly IRefIdProperty UnloadByIdProperty
            = P<FixtureUnload>.RegisterRefId(e => e.UnloadById, ReferenceType.Normal);

        /// <summary>
        /// 出库人员Id
        /// </summary>
        public double UnloadById
        {
            get { return (double)GetRefId(UnloadByIdProperty); }
            set { SetRefId(UnloadByIdProperty, value); }
        }

        /// <summary>
        /// 出库人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> UnloadByProperty
            = P<FixtureUnload>.RegisterRef(e => e.UnloadBy, UnloadByIdProperty);

        /// <summary>
        /// 出库人员
        /// </summary>
        public Employee UnloadBy
        {
            get { return GetRefEntity(UnloadByProperty); }
            set { SetRefEntity(UnloadByProperty, value); }
        }
        #endregion

        #region 库位 Location
        /// <summary>
        /// 库位Id
        /// </summary>
        public static readonly IRefIdProperty LocationIdProperty
            = P<FixtureUnload>.RegisterRefId(e => e.LocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId
        {
            get { return (double)GetRefId(LocationIdProperty); }
            set { SetRefId(LocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> LocationProperty
            = P<FixtureUnload>.RegisterRef(e => e.Location, LocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation Location
        {
            get { return GetRefEntity(LocationProperty); }
            set { SetRefEntity(LocationProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty
            = P<FixtureUnload>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty
            = P<FixtureUnload>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 出库执行情况 FixtureDemand
        /// <summary>
        /// 出库执行情况Id
        /// </summary>
        public static readonly IRefIdProperty FixtureDemandIdProperty
            = P<FixtureUnload>.RegisterRefId(e => e.FixtureDemandId, ReferenceType.Parent);

        /// <summary>
        /// 出库执行情况Id
        /// </summary>
        public double FixtureDemandId
        {
            get { return (double)GetRefId(FixtureDemandIdProperty); }
            set { SetRefId(FixtureDemandIdProperty, value); }
        }

        /// <summary>
        /// 出库执行情况
        /// </summary>
        public static readonly RefEntityProperty<FixtureDemand> FixtureDemandProperty
            = P<FixtureUnload>.RegisterRef(e => e.FixtureDemand, FixtureDemandIdProperty);

        /// <summary>
        /// 出库执行情况
        /// </summary>
        public FixtureDemand FixtureDemand
        {
            get { return GetRefEntity(FixtureDemandProperty); }
            set { SetRefEntity(FixtureDemandProperty, value); }
        }
        #endregion

        #region 保养任务 MaintainTask
        /// <summary>
        /// 保养任务Id
        /// </summary>
        public static readonly IRefIdProperty MaintainTaskIdProperty
            = P<FixtureUnload>.RegisterRefId(e => e.MaintainTaskId, ReferenceType.Normal);

        /// <summary>
        /// 保养任务Id
        /// </summary>
        public double? MaintainTaskId
        {
            get { return (double?)GetRefNullableId(MaintainTaskIdProperty); }
            set { SetRefNullableId(MaintainTaskIdProperty, value); }
        }

        /// <summary>
        /// 保养任务
        /// </summary>
        public static readonly RefEntityProperty<MaintainTask> MaintainTaskProperty
            = P<FixtureUnload>.RegisterRef(e => e.MaintainTask, MaintainTaskIdProperty);

        /// <summary>
        /// 保养任务
        /// </summary>
        public MaintainTask MaintainTask
        {
            get { return GetRefEntity(MaintainTaskProperty); }
            set { SetRefEntity(MaintainTaskProperty, value); }
        }
        #endregion

        #region 归还数量 ReturnQty
        /// <summary>
        /// 归还数量
        /// </summary>
        [Label("归还数量")]
        public static readonly Property<int> ReturnQtyProperty
            = P<FixtureUnload>.Register(e => e.ReturnQty);

        /// <summary>
        /// 归还数量
        /// </summary>
        public int ReturnQty
        {
            get { return GetProperty(ReturnQtyProperty); }
            set { SetProperty(ReturnQtyProperty, value); }
        }
        #endregion

        #region 保养不合格数量 NgQty
        /// <summary>
        /// 保养不合格数量
        /// </summary>
        [Label("保养不合格数量")]
        public static readonly Property<int> NgQtyProperty
            = P<FixtureUnload>.Register(e => e.NgQty);

        /// <summary>
        /// 保养不合格数量
        /// </summary>
        public int NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty
            = P<FixtureUnload>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

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
        public static readonly Property<string> WarehouseNameProperty
            = P<FixtureUnload>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

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
        public static readonly Property<string> LocationCodeProperty
            = P<FixtureUnload>.RegisterView(e => e.LocationCode, p => p.Location.Code);

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
        public static readonly Property<string> LocationNameProperty
            = P<FixtureUnload>.RegisterView(e => e.LocationName, p => p.Location.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName
        {
            get { return this.GetProperty(LocationNameProperty); }
            set { this.SetProperty(LocationNameProperty, value); }
        }
        #endregion

        #region 工治具编码Id EncodeId
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码Id")]
        public static readonly Property<double> EncodeIdProperty
            = P<FixtureUnload>.RegisterView(e => e.EncodeId, p => p.FixtureAccount.FixtureEncodeId);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double EncodeId
        {
            get { return this.GetProperty(EncodeIdProperty); }
            set { this.SetProperty(EncodeIdProperty, value); }
        }
        #endregion

        #region 工治具ID AccountCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> AccountCodeProperty
            = P<FixtureUnload>.RegisterView(e => e.AccountCode, p => p.FixtureAccount.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string AccountCode
        {
            get { return this.GetProperty(AccountCodeProperty); }
            set { this.SetProperty(AccountCodeProperty, value); }
        }
        #endregion

        #region 出库人员 UnloadByName
        /// <summary>
        /// 出库人员
        /// </summary>
        [Label("出库人员")]
        public static readonly Property<string> UnloadByNameProperty
            = P<FixtureUnload>.RegisterView(e => e.UnloadByName, p => p.UnloadBy.Name);

        /// <summary>
        /// 出库人员
        /// </summary>
        public string UnloadByName
        {
            get { return this.GetProperty(UnloadByNameProperty); }
            set { this.SetProperty(UnloadByNameProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty
            = P<FixtureUnload>.RegisterView(e => e.FixtureType, p => p.FixtureAccount.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
        }
        #endregion

        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty
            = P<FixtureUnload>.RegisterView(e => e.EncodeCode, p => p.FixtureAccount.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty
            = P<FixtureUnload>.RegisterView(e => e.ModelName, p => p.FixtureAccount.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 关联保养单号 TaskNo
        /// <summary>
        /// 关联保养单号
        /// </summary>
        [Label("关联保养单号")]
        public static readonly Property<string> TaskNoProperty
            = P<FixtureUnload>.RegisterView(e => e.TaskNo, p => p.MaintainTask.No);

        /// <summary>
        /// 关联保养单号
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
        }
        #endregion

        #region 保养状态 MaintainTaskState
        /// <summary>
        /// 保养状态
        /// </summary>
        [Label("保养状态")]
        public static readonly Property<MaintainState?> MaintainTaskStateProperty = P<FixtureUnload>.RegisterView(e => e.MaintainTaskState, p => p.MaintainTask.State);

        /// <summary>
        /// 保养状态
        /// </summary>
        public MaintainState? MaintainTaskState
        {
            get { return this.GetProperty(MaintainTaskStateProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 出库明细 实体配置
    /// </summary>
    internal class FixtureUnloadConfig : EntityConfig<FixtureUnload>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIX_UNLOAD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
