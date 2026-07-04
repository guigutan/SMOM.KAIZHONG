using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// 站台
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StationCriteria))]
    [Label("站台")]
    [DisplayMember(nameof(Code))]
    //[DataAuth.EntityDataAuth(typeof(WarehouseEmployee), nameof(WarehouseId))]
    public partial class Station : DataEntity, IStateEntity
    {
        #region 站台代码 Code
        /// <summary>
        /// 站台代码，规则：站台类型名：排_层_列_库区_仓库
        /// </summary>
        [MaxLength(80)]
        [Label("编码")]
        [Required, NotDuplicate]
        public static readonly Property<string> CodeProperty = P<Station>.Register(e => e.Code);

        /// <summary>
        /// 站台代码，规则：站台类型名：排_层_列_库区_仓库
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
        [MaxLength(80)]
        [Label("名称")]
        [Required, NotDuplicate]
        public static readonly Property<string> NameProperty = P<Station>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 是否对接生产线 IsLinkProduction
        /// <summary>
        /// 是否对接生产线，默认为N
        /// </summary>
        [Label("是否对接生产线")]
        public static readonly Property<bool> IsLinkProductionProperty = P<Station>.Register(e => e.IsLinkProduction);

        /// <summary>
        /// 是否对接生产线，默认为N
        /// </summary>
        public bool IsLinkProduction
        {
            get { return GetProperty(IsLinkProductionProperty); }
            set { SetProperty(IsLinkProductionProperty, value); }
        }
        #endregion

        #region 是否被锁 IsLock
        /// <summary>
        /// 该站台是否被锁，Y表示被锁、N表示未被锁。存在双向出入库站台互锁业务
        /// </summary>
        [Label("是否被锁")]
        public static readonly Property<bool> IsLockProperty = P<Station>.Register(e => e.IsLock);

        /// <summary>
        /// 该站台是否被锁，Y表示被锁、N表示未被锁。存在双向出入库站台互锁业务
        /// </summary>
        public bool IsLock
        {
            get { return GetProperty(IsLockProperty); }
            set { SetProperty(IsLockProperty, value); }
        }
        #endregion

        #region 所在的物理楼层 Floor
        /// <summary>
        /// 所在的物理楼层
        /// </summary>
        [Label("所在楼层")]
        [MinValue(1)]
        public static readonly Property<int?> FloorProperty = P<Station>.Register(e => e.Floor);

        /// <summary>
        /// 所在的物理楼层
        /// </summary>
        public int? Floor
        {
            get { return GetProperty(FloorProperty); }
            set { SetProperty(FloorProperty, value); }
        }
        #endregion

        #region 功能描述 FunctionDescription
        /// <summary>
        /// 功能描述
        /// </summary>
        [MaxLength(240)]
        [Label("功能描述")]
        public static readonly Property<string> FunctionDescriptionProperty = P<Station>.Register(e => e.FunctionDescription);

        /// <summary>
        /// 功能描述
        /// </summary>
        public string FunctionDescription
        {
            get { return GetProperty(FunctionDescriptionProperty); }
            set { SetProperty(FunctionDescriptionProperty, value); }
        }
        #endregion

        #region 位置描述 LocationDescription
        /// <summary>
        /// 位置描述
        /// </summary>
        [MaxLength(240)]
        [Label("位置描述")]
        public static readonly Property<string> LocationDescriptionProperty = P<Station>.Register(e => e.LocationDescription);

        /// <summary>
        /// 位置描述
        /// </summary>
        public string LocationDescription
        {
            get { return GetProperty(LocationDescriptionProperty); }
            set { SetProperty(LocationDescriptionProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 站台优先级,用于同侧有多个入库站台时优先哪个站台先接货,值越高,优先级越高,默认为0
        /// </summary>
        [Label("优先级")]
        [MinValue(1)]
        public static readonly Property<int?> PriorityProperty = P<Station>.Register(e => e.Priority);

        /// <summary>
        /// 站台优先级,用于同侧有多个入库站台时优先哪个站台先接货,值越高,优先级越高,默认为0
        /// </summary>
        public int? Priority
        {
            get { return GetProperty(PriorityProperty); }
            set { SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 搬运设备排编号 DeviceLineNo
        /// <summary>
        /// 巷道内对应搬运设备（如堆垛机、子母车、AGV等）的排编号，如果设备不用排层列直接用数字编号(如村田堆垛机对楼层站台管理)，那么就用该字段，层、列值就为零
        /// </summary>
        [Label("搬运设备排编号")]
        [MinValue(2)]
        public static readonly Property<int?> DeviceLineNoProperty = P<Station>.Register(e => e.DeviceLineNo);

        /// <summary>
        /// 巷道内对应搬运设备（如堆垛机、子母车、AGV等）的排编号，如果设备不用排层列直接用数字编号(如村田堆垛机对楼层站台管理)，那么就用该字段，层、列值就为零
        /// </summary>
        public int? DeviceLineNo
        {
            get { return GetProperty(DeviceLineNoProperty); }
            set { SetProperty(DeviceLineNoProperty, value); }
        }
        #endregion

        #region 搬运设备层编号 DeviceLayerNo
        /// <summary>
        /// 对应搬运设备（如堆垛机）的层编号
        /// </summary>
        [Label("搬运设备层编号")]
        [MinValue(1)]
        public static readonly Property<int?> DeviceLayerNoProperty = P<Station>.Register(e => e.DeviceLayerNo);

        /// <summary>
        /// 对应搬运设备（如堆垛机）的层编号
        /// </summary>
        public int? DeviceLayerNo
        {
            get { return GetProperty(DeviceLayerNoProperty); }
            set { SetProperty(DeviceLayerNoProperty, value); }
        }
        #endregion

        #region 搬运设备列编号 DeviceColumnNo
        /// <summary>
        /// 对应搬运设备（如堆垛机）的列编号
        /// </summary>
        [Label("搬运设备列编号")]
        [MinValue(1)]
        public static readonly Property<int?> DeviceColumnNoProperty = P<Station>.Register(e => e.DeviceColumnNo);

        /// <summary>
        /// 对应搬运设备（如堆垛机）的列编号
        /// </summary>
        public int? DeviceColumnNo
        {
            get { return GetProperty(DeviceColumnNoProperty); }
            set { SetProperty(DeviceColumnNoProperty, value); }
        }
        #endregion

        #region 工位序列号 SerialNo
        /// <summary>
        /// 工位序列号,用于每个拣选工位排序编号,默认为0
        /// </summary>
        [Label("工位序列号")]
        [MinValue(1)]
        public static readonly Property<int?> SerialNoProperty = P<Station>.Register(e => e.SerialNo);

        /// <summary>
        /// 工位序列号,用于每个拣选工位排序编号,默认为0
        /// </summary>
        public int? SerialNo
        {
            get { return GetProperty(SerialNoProperty); }
            set { SetProperty(SerialNoProperty, value); }
        }
        #endregion

        #region 站台OPC编号 OpcSerialNo
        /// <summary>
        /// 站台OPC编号，可监听垛是否就绪
        /// </summary>
        [Label("站台OPC编号")]
        [MinValue(1)]
        public static readonly Property<int?> OpcSerialNoProperty = P<Station>.Register(e => e.OpcSerialNo);

        /// <summary>
        /// 站台OPC编号，可监听垛是否就绪
        /// </summary>
        public int? OpcSerialNo
        {
            get { return GetProperty(OpcSerialNoProperty); }
            set { SetProperty(OpcSerialNoProperty, value); }
        }
        #endregion

        #region 条码枪OPC编号 OpcScannerNo
        /// <summary>
        /// 如果该站台处存在条码枪，那么该条码枪设备的OPC编号值，不存在不能配置要为空
        /// </summary>
        [Label("母托盘OPC编号")]
        [MinValue(1)]
        public static readonly Property<int?> OpcScannerNoProperty = P<Station>.Register(e => e.OpcScannerNo);

        /// <summary>
        /// 如果该站台处存在条码枪，那么该条码枪设备的OPC编号值，不存在不能配置要为空
        /// </summary>
        public int? OpcScannerNo
        {
            get { return GetProperty(OpcScannerNoProperty); }
            set { SetProperty(OpcScannerNoProperty, value); }
        }
        #endregion

        #region 子托盘OPC编号 OpcSubTrayScannerNo
        /// <summary>
        /// 如果该站台处存在子托盘条码枪，那么该扫描子托盘条码枪设备的OPC编号值，不存在不能配置要为空
        /// </summary>
        [Label("子托盘OPC编号")]
        [MinValue(1)]
        public static readonly Property<int?> OpcSubTrayScannerNoProperty = P<Station>.Register(e => e.OpcSubTrayScannerNo);

        /// <summary>
        /// 如果该站台处存在子托盘条码枪，那么该扫描子托盘条码枪设备的OPC编号值，不存在不能配置要为空
        /// </summary>
        public int? OpcSubTrayScannerNo
        {
            get { return GetProperty(OpcSubTrayScannerNoProperty); }
            set { SetProperty(OpcSubTrayScannerNoProperty, value); }
        }
        #endregion

        #region 搬运设备站台编号 DeviceDockNo
        /// <summary>
        /// 对应搬运设备（如RGV、AGV、环穿等，不含堆垛机）站台编号
        /// </summary>
        [Label("搬运设备站台编号")]
        [MinValue(1)]
        public static readonly Property<int?> DeviceDockNoProperty = P<Station>.Register(e => e.DeviceDockNo);

        /// <summary>
        /// 对应搬运设备（如RGV、AGV、环穿等，不含堆垛机）站台编号
        /// </summary>
        public int? DeviceDockNo
        {
            get { return GetProperty(DeviceDockNoProperty); }
            set { SetProperty(DeviceDockNoProperty, value); }
        }
        #endregion

        #region 是否需要写指令ID IsNeedWriteInstructId
        /// <summary>
        /// 是否需要写指令ID
        /// </summary>
        [Label("是否需要写指令ID")]
        public static readonly Property<bool> IsNeedWriteInstructIdProperty = P<Station>.Register(e => e.IsNeedWriteInstructId);

        /// <summary>
        /// 是否需要写指令ID
        /// </summary>
        public bool IsNeedWriteInstructId
        {
            get { return GetProperty(IsNeedWriteInstructIdProperty); }
            set { SetProperty(IsNeedWriteInstructIdProperty, value); }
        }
        #endregion

        #region 在出库口处是否需要写条码 IsNeedWriteBarcode
        /// <summary>
        /// 在出库口处是否需要写条码
        /// </summary>
        [Label("在出库口处是否需要写条码")]
        public static readonly Property<bool> IsNeedWriteBarcodeProperty = P<Station>.Register(e => e.IsNeedWriteBarcode);

        /// <summary>
        /// 在出库口处是否需要写条码
        /// </summary>
        public bool IsNeedWriteBarcode
        {
            get { return GetProperty(IsNeedWriteBarcodeProperty); }
            set { SetProperty(IsNeedWriteBarcodeProperty, value); }
        }
        #endregion

        #region 备选出库地址 BackupExitAddress
        /// <summary>
        /// 备选出库地址，多个值用符号“|”隔开
        /// </summary>
        [MaxLength(512)]
        [Label("备选出库地址")]
        public static readonly Property<string> BackupExitAddressProperty = P<Station>.Register(e => e.BackupExitAddress);

        /// <summary>
        /// 备选出库地址，多个值用符号“|”隔开
        /// </summary>
        public string BackupExitAddress
        {
            get { return GetProperty(BackupExitAddressProperty); }
            set { SetProperty(BackupExitAddressProperty, value); }
        }
        #endregion

        #region 关联站台 RelatedStation
        /// <summary>
        /// 关联站台，配置一个站台
        /// </summary>
        [MaxLength(64)]
        [Label("关联站台")]
        public static readonly Property<string> RelatedStationProperty = P<Station>.Register(e => e.RelatedStation);

        /// <summary>
        /// 关联站台，配置一个站台
        /// </summary>
        public string RelatedStation
        {
            get { return GetProperty(RelatedStationProperty); }
            set { SetProperty(RelatedStationProperty, value); }
        }
        #endregion

        #region 出入库模式的关联站台 InOutRelatedStation
        /// <summary>
        /// 出入库模式的关联站台，多个值用符号“|”隔开
        /// </summary>
        [MaxLength(512)]
        [Label("出入库模式的关联站台")]
        public static readonly Property<string> InOutRelatedStationProperty = P<Station>.Register(e => e.InOutRelatedStation);

        /// <summary>
        /// 出入库模式的关联站台，多个值用符号“|”隔开
        /// </summary>
        public string InOutRelatedStation
        {
            get { return GetProperty(InOutRelatedStationProperty); }
            set { SetProperty(InOutRelatedStationProperty, value); }
        }
        #endregion

        #region 托盘类型 TrayType
        /// <summary>
        /// 托盘类型，如1100、1400，只配置出入库站台、拣选站台、拆码垛机站台等存在业务的站台，无业务的站台如楼层站台不需要配置
        /// </summary>
        [MaxLength(32)]
        [Label("托盘类型")]
        public static readonly Property<string> TrayTypeProperty = P<Station>.Register(e => e.TrayType);

        /// <summary>
        /// 托盘类型，如1100、1400，只配置出入库站台、拣选站台、拆码垛机站台等存在业务的站台，无业务的站台如楼层站台不需要配置
        /// </summary>
        public string TrayType
        {
            get { return GetProperty(TrayTypeProperty); }
            set { SetProperty(TrayTypeProperty, value); }
        }
        #endregion

        #region 备注说明 Note
        /// <summary>
        /// 备注说明
        /// </summary>
        [MaxLength(128)]
        [Label("备注说明")]
        public static readonly Property<string> NoteProperty = P<Station>.Register(e => e.Note);

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Note
        {
            get { return GetProperty(NoteProperty); }
            set { SetProperty(NoteProperty, value); }
        }
        #endregion

        #region 站台类型 StationType
        /// <summary>
        /// 站台类型
        /// </summary>
        [Label("站台类型")]
        public static readonly Property<StationType> StationTypeProperty = P<Station>.Register(e => e.StationType);

        /// <summary>
        /// 站台类型
        /// </summary>
        public StationType StationType
        {
            get { return GetProperty(StationTypeProperty); }
            set { SetProperty(StationTypeProperty, value); }
        }
        #endregion

        #region LED屏幕基础数据 Led
        /// <summary>
        /// LED屏幕基础数据Id
        /// </summary>
        [Label("LED屏幕基础数据")]
        public static readonly IRefIdProperty LedIdProperty =
            P<Station>.RegisterRefId(e => e.LedId, ReferenceType.Normal);

        /// <summary>
        /// LED屏幕基础数据Id
        /// </summary>
        public double? LedId
        {
            get { return (double?)this.GetRefNullableId(LedIdProperty); }
            set { this.SetRefNullableId(LedIdProperty, value); }
        }

        /// <summary>
        /// LED屏幕基础数据
        /// </summary>
        public static readonly RefEntityProperty<LED> LedProperty =
            P<Station>.RegisterRef(e => e.Led, LedIdProperty);

        /// <summary>
        /// LED屏幕基础数据
        /// </summary>
        public LED Led
        {
            get { return this.GetRefEntity(LedProperty); }
            set { this.SetRefEntity(LedProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<Station>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<Station>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 巷道 Routeway
        /// <summary>
        /// 巷道Id
        /// </summary>
        [Label("巷道编码")]
        public static readonly IRefIdProperty RoutewayIdProperty =
            P<Station>.RegisterRefId(e => e.RoutewayId, ReferenceType.Normal);

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
            P<Station>.RegisterRef(e => e.Routeway, RoutewayIdProperty);

        /// <summary>
        /// 巷道
        /// </summary>
        public Routeway Routeway
        {
            get { return this.GetRefEntity(RoutewayProperty); }
            set { this.SetRefEntity(RoutewayProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Station>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 站台 实体配置
    /// </summary>
    internal class StationConfig : EntityConfig<Station>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WCS_STATION").MapAllProperties();
            Meta.Property(Station.CodeProperty).ColumnMeta.HasLength(160);
            Meta.Property(Station.NameProperty).ColumnMeta.HasLength(160);
            Meta.Property(Station.FunctionDescriptionProperty).ColumnMeta.HasLength(480);
            Meta.Property(Station.LocationDescriptionProperty).ColumnMeta.HasLength(480);
            Meta.Property(Station.BackupExitAddressProperty).ColumnMeta.HasLength(1024);
            Meta.Property(Station.RelatedStationProperty).ColumnMeta.HasLength(128);
            Meta.Property(Station.InOutRelatedStationProperty).ColumnMeta.HasLength(1024);
            Meta.Property(Station.TrayTypeProperty).ColumnMeta.HasLength(64);
            Meta.Property(Station.NoteProperty).ColumnMeta.HasLength(256);
            Meta.EnablePhantoms();
            Meta.DisableDataSync();
        }
    }
}