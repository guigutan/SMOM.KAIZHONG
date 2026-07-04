using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库位查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("库位查询")]
    public partial class StorageLocationCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<StorageLocationCriteria>.Register(e => e.Code);

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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<StorageLocationCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 LibraryType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<LibraryType?> LibraryTypeProperty = P<StorageLocationCriteria>.Register(e => e.LibraryType);

        /// <summary>
        /// 类型
        /// </summary>
        public LibraryType? LibraryType
        {
            get { return GetProperty(LibraryTypeProperty); }
            set { SetProperty(LibraryTypeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<StorageLocationCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<StorageLocationCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库区 Area
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<string> AreaProperty = P<StorageLocationCriteria>.Register(e => e.Area);

        /// <summary>
        /// 库区
        /// </summary>
        public string Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 是否冻结 IsFrozen
        /// <summary>
        /// 是否冻结
        /// </summary>
        [Label("是否冻结")]
        public static readonly Property<bool?> IsFrozenProperty = P<StorageLocationCriteria>.Register(e => e.IsFrozen);

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool? IsFrozen
        {
            get { return GetProperty(IsFrozenProperty); }
            set { SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        #region ERP库存组织 ErpInvOrg
        /// <summary>
        /// ERP库存组织
        /// </summary>
        [Label("ERP库存组织")]
        public static readonly Property<string> ErpInvOrgProperty = P<StorageLocationCriteria>.Register(e => e.ErpInvOrg);

        /// <summary>
        /// ERP库存组织
        /// </summary>
        public string ErpInvOrg
        {
            get { return GetProperty(ErpInvOrgProperty); }
            set { SetProperty(ErpInvOrgProperty, value); }
        }
        #endregion

        #region ERP子库 ErpSubLibrary
        /// <summary>
        /// ERP子库
        /// </summary>
        [Label("ERP子库")]
        public static readonly Property<string> ErpSubLibraryProperty = P<StorageLocationCriteria>.Register(e => e.ErpSubLibrary);

        /// <summary>
        /// ERP子库
        /// </summary>
        public string ErpSubLibrary
        {
            get { return GetProperty(ErpSubLibraryProperty); }
            set { SetProperty(ErpSubLibraryProperty, value); }
        }
        #endregion

        #region ERP库位 ErpLocation
        /// <summary>
        /// ERP库位
        /// </summary>
        [Label("ERP库位")]
        public static readonly Property<string> ErpLocationProperty = P<StorageLocationCriteria>.Register(e => e.ErpLocation);

        /// <summary>
        /// ERP库位
        /// </summary>
        public string ErpLocation
        {
            get { return GetProperty(ErpLocationProperty); }
            set { SetProperty(ErpLocationProperty, value); }
        }
        #endregion

        #region 是否立库 IsAutomatedArea
        /// <summary>
        /// 是否立库
        /// </summary>
        [Label("是否立库")]
        public static readonly Property<YesNo?> IsAutomatedAreaProperty = P<StorageLocationCriteria>.Register(e => e.IsAutomatedArea);

        /// <summary>
        /// 是否立库
        /// </summary>
        public YesNo? IsAutomatedArea
        {
            get { return this.GetProperty(IsAutomatedAreaProperty); }
            set { this.SetProperty(IsAutomatedAreaProperty, value); }
        }
        #endregion

        #region 仓库(用于多选联动查询) Warehouses
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehousesProperty = P<StorageLocationCriteria>.Register(e => e.Warehouses);

        /// <summary>
        /// 仓库(用于多选联动查询)
        /// </summary>
        public string Warehouses
        {
            get { return this.GetProperty(WarehousesProperty); }
            set { this.SetProperty(WarehousesProperty, value); }
        }
        #endregion

        #region 库区ID(用于多选联动查询) AreaIds
        /// <summary>
        /// 库区ID(用于多选联动查询)
        /// </summary>
        [Label("库区ID")]
        public static readonly Property<string> AreaIdsProperty = P<StorageLocationCriteria>.Register(e => e.AreaIds);

        /// <summary>
        /// 库区ID
        /// </summary>
        public string AreaIds
        {
            get { return this.GetProperty(AreaIdsProperty); }
            set { this.SetProperty(AreaIdsProperty, value); }
        }
        #endregion

        #region 排 RowNo
        /// <summary>
        /// 排
        /// </summary>
        [Label("排")]
        public static readonly Property<int?> RowNoProperty = P<StorageLocationCriteria>.Register(e => e.RowNo);

        /// <summary>
        /// 排
        /// </summary>
        public int? RowNo
        {
            get { return this.GetProperty(RowNoProperty); }
            set { this.SetProperty(RowNoProperty, value); }
        }
        #endregion

        #region 层 LayerNo
        /// <summary>
        /// 层
        /// </summary>
        [Label("层")]
        public static readonly Property<int?> LayerNoProperty = P<StorageLocationCriteria>.Register(e => e.LayerNo);

        /// <summary>
        /// 层
        /// </summary>
        public int? LayerNo
        {
            get { return this.GetProperty(LayerNoProperty); }
            set { this.SetProperty(LayerNoProperty, value); }
        }
        #endregion

        #region 列 ColumnNo
        /// <summary>
        /// 列
        /// </summary>
        [Label("列")]
        public static readonly Property<int?> ColumnNoProperty = P<StorageLocationCriteria>.Register(e => e.ColumnNo);

        /// <summary>
        /// 列
        /// </summary>
        public int? ColumnNo
        {
            get { return this.GetProperty(ColumnNoProperty); }
            set { this.SetProperty(ColumnNoProperty, value); }
        }
        #endregion

        #region 巷道 Routeway
        /// <summary>
        /// 巷道Id
        /// </summary>
        [Label("巷道")]
        public static readonly IRefIdProperty RoutewayIdProperty =
            P<StorageLocationCriteria>.RegisterRefId(e => e.RoutewayId, ReferenceType.Normal);

        /// <summary>
        /// 巷道Id
        /// </summary>
        public double? RoutewayId
        {
            get { return (double?)this.GetRefNullableId(RoutewayIdProperty); }
            set { this.SetRefNullableId(RoutewayIdProperty, value); }
        }

        /// <summary>
        /// 巷道
        /// </summary>
        public static readonly RefEntityProperty<Routeway> RoutewayProperty =
            P<StorageLocationCriteria>.RegisterRef(e => e.Routeway, RoutewayIdProperty);

        /// <summary>
        /// 巷道
        /// </summary>
        public Routeway Routeway
        {
            get { return this.GetRefEntity(RoutewayProperty); }
            set { this.SetRefEntity(RoutewayProperty, value); }
        }
        #endregion

        #region 逻辑分区 LogicArea
        /// <summary>
        /// 逻辑分区Id
        /// </summary>
        [Label("逻辑分区")]
        public static readonly IRefIdProperty LogicAreaIdProperty =
            P<StorageLocationCriteria>.RegisterRefId(e => e.LogicAreaId, ReferenceType.Normal);

        /// <summary>
        /// 逻辑分区Id
        /// </summary>
        public double? LogicAreaId
        {
            get { return (double?)this.GetRefNullableId(LogicAreaIdProperty); }
            set { this.SetRefNullableId(LogicAreaIdProperty, value); }
        }

        /// <summary>
        /// 逻辑分区
        /// </summary>
        public static readonly RefEntityProperty<LogicArea> LogicAreaProperty =
            P<StorageLocationCriteria>.RegisterRef(e => e.LogicArea, LogicAreaIdProperty);

        /// <summary>
        /// 逻辑分区
        /// </summary>
        public LogicArea LogicArea
        {
            get { return this.GetRefEntity(LogicAreaProperty); }
            set { this.SetRefEntity(LogicAreaProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        public static readonly Property<State?> StateProperty = P<StorageLocationCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WarehouseController>().GetStorageLocation(this);
        }
    }
}
