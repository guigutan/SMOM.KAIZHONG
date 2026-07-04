using SIE.Domain;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.FixtureDemands.ViewModels
{
    /// <summary>
    /// 出库明细ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("出库明细信息")]
    public class FixtureUnloadViewModel : Entity<double>
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<FixtureUnloadViewModel>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<FixtureUnloadViewModel>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库位 Location
        /// <summary>
        /// 库位Id
        /// </summary>
        public static readonly IRefIdProperty LocationIdProperty = P<FixtureUnloadViewModel>.RegisterRefId(e => e.LocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> LocationProperty = P<FixtureUnloadViewModel>.RegisterRef(e => e.Location, LocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation Location
        {
            get { return GetRefEntity(LocationProperty); }
            set { SetRefEntity(LocationProperty, value); }
        }
        #endregion

        #region 工治具台帐 FixtureAccount
        /// <summary>
        /// 工治具台帐Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<FixtureUnloadViewModel>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<FixtureUnloadViewModel>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具台帐
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 工治具需求明细 FixtureDemandDetail
        /// <summary>
        /// 工治具需求明细Id
        /// </summary>
        public static readonly IRefIdProperty FixtureDemandDetailIdProperty = P<FixtureUnloadViewModel>.RegisterRefId(e => e.FixtureDemandDetailId, ReferenceType.Normal);

        /// <summary>
        /// 工治具需求明细Id
        /// </summary>
        public double FixtureDemandDetailId
        {
            get { return (double)GetRefId(FixtureDemandDetailIdProperty); }
            set { SetRefId(FixtureDemandDetailIdProperty, value); }
        }

        /// <summary>
        /// 工治具需求明细
        /// </summary>
        public static readonly RefEntityProperty<FixtureDemandDetail> FixtureDemandDetailProperty = P<FixtureUnloadViewModel>.RegisterRef(e => e.FixtureDemandDetail, FixtureDemandDetailIdProperty);

        /// <summary>
        /// 工治具需求明细
        /// </summary>
        public FixtureDemandDetail FixtureDemandDetail
        {
            get { return GetRefEntity(FixtureDemandDetailProperty); }
            set { SetRefEntity(FixtureDemandDetailProperty, value); }
        }
        #endregion

        #region 保养任务 MaintainTask
        /// <summary>
        /// 保养任务Id
        /// </summary>
        public static readonly IRefIdProperty MaintainTaskIdProperty = P<FixtureUnloadViewModel>.RegisterRefId(e => e.MaintainTaskId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<MaintainTask> MaintainTaskProperty = P<FixtureUnloadViewModel>.RegisterRef(e => e.MaintainTask, MaintainTaskIdProperty);

        /// <summary>
        /// 保养任务
        /// </summary>
        public MaintainTask MaintainTask
        {
            get { return GetRefEntity(MaintainTaskProperty); }
            set { SetRefEntity(MaintainTaskProperty, value); }
        }
        #endregion

        #region 出库数量 UnloadQty
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        public static readonly Property<int> UnloadQtyProperty = P<FixtureUnloadViewModel>.Register(e => e.UnloadQty);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int UnloadQty
        {
            get { return GetProperty(UnloadQtyProperty); }
            set { SetProperty(UnloadQtyProperty, value); }
        }
        #endregion

        #region 保养不合格数量 NgQty
        /// <summary>
        /// 保养不合格数量
        /// </summary>
        [Label("保养不合格数量")]
        public static readonly Property<int> NgQtyProperty = P<FixtureUnloadViewModel>.Register(e => e.NgQty);

        /// <summary>
        /// 保养不合格数量
        /// </summary>
        public int NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region TurnoverToolCode
        /// <summary>
        /// 关联载具
        /// </summary>
        [Label("关联载具")]
        public static readonly Property<string> TurnoverToolCodeProperty = P<FixtureUnloadViewModel>.Register(e => e.TurnoverToolCode);

        /// <summary>
        /// 关联载具
        /// </summary>
        public string TurnoverToolCode
        {
            get { return GetProperty(TurnoverToolCodeProperty); }
            set { SetProperty(TurnoverToolCodeProperty, value); }
        }
        #endregion

        #region 出库人员 UnloadBy
        /// <summary>
        /// 出库人员Id
        /// </summary>
        public static readonly IRefIdProperty UnloadByIdProperty = P<FixtureUnloadViewModel>.RegisterRefId(e => e.UnloadById, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> UnloadByProperty = P<FixtureUnloadViewModel>.RegisterRef(e => e.UnloadBy, UnloadByIdProperty);

        /// <summary>
        /// 出库人员
        /// </summary>
        public Employee UnloadBy
        {
            get { return GetRefEntity(UnloadByProperty); }
            set { SetRefEntity(UnloadByProperty, value); }
        }
        #endregion

        #region 执行时间 UnloadDate
        /// <summary>
        /// 执行时间
        /// </summary>
        [Label("执行时间")]
        public static readonly Property<DateTime> UnloadDateProperty = P<FixtureUnloadViewModel>.Register(e => e.UnloadDate);

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime UnloadDate
        {
            get { return GetProperty(UnloadDateProperty); }
            set { SetProperty(UnloadDateProperty, value); }
        }
        #endregion

        #region 是否旧数据 IsOld
        /// <summary>
        /// 是否旧数据
        /// </summary>
        public static readonly Property<bool> IsOldProperty = P<FixtureUnloadViewModel>.Register(e => e.IsOld);

        /// <summary>
        /// 是否旧数据
        /// </summary>
        public bool IsOld
        {
            get { return GetProperty(IsOldProperty); }
            set { SetProperty(IsOldProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<FixtureUnloadViewModel>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

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
        public static readonly Property<string> WarehouseNameProperty = P<FixtureUnloadViewModel>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

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
        public static readonly Property<string> LocationCodeProperty = P<FixtureUnloadViewModel>.RegisterView(e => e.LocationCode, p => p.Location.Code);

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
        public static readonly Property<string> LocationNameProperty = P<FixtureUnloadViewModel>.RegisterView(e => e.LocationName, p => p.Location.Name);

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
        public static readonly Property<string> AccountCodeProperty = P<FixtureUnloadViewModel>.RegisterView(e => e.AccountCode, p => p.FixtureAccount.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string AccountCode
        {
            get { return this.GetProperty(AccountCodeProperty); }
            set { this.SetProperty(AccountCodeProperty, value); }
        }
        #endregion

        #region 工治具编码Id EncodeId
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码Id")]
        public static readonly Property<double> EncodeIdProperty = P<FixtureUnloadViewModel>.RegisterView(e => e.EncodeId, p => p.FixtureAccount.FixtureEncodeId);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double EncodeId
        {
            get { return this.GetProperty(EncodeIdProperty); }
            set { this.SetProperty(EncodeIdProperty, value); }
        }
        #endregion

        #region 出库人员 UnloadByName
        /// <summary>
        /// 出库人员
        /// </summary>
        [Label("出库人员")]
        public static readonly Property<string> UnloadByNameProperty = P<FixtureUnloadViewModel>.RegisterView(e => e.UnloadByName, p => p.UnloadBy.Name);

        /// <summary>
        /// 出库人员
        /// </summary>
        public string UnloadByName
        {
            get { return this.GetProperty(UnloadByNameProperty); }
            set { this.SetProperty(UnloadByNameProperty, value); }
        }
        #endregion
        #endregion
    }
}
