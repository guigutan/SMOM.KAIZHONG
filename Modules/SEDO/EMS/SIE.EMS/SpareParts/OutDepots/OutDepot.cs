using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Criterias;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Text;

namespace SIE.EMS.SpareParts.OutDepots
{
    /// <summary>
    /// 备件出库单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(OutDepotCriteria))]
    [EntityWithConfig(typeof(NoConfig), "备件出库单单号生成规则配置项", "备件出库单单号生成规则")]
    [Label("备件出库单")]
    public class OutDepot : DataEntity
    {
        #region 备件出库单号 No
        /// <summary>
        /// 备件出库单号
        /// </summary>
        [Label("备件出库单号")]
        [Required]
        public static readonly Property<string> NoProperty = P<OutDepot>.Register(e => e.No);

        /// <summary>
        /// 备件出库单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 出库类型 OutDepotType
        /// <summary>
        /// 出库类型
        /// </summary>
        [Label("出库类型")]
        public static readonly Property<OutDepotType> OutDepotTypeProperty = P<OutDepot>.Register(e => e.OutDepotType);

        /// <summary>
        /// 出库类型
        /// </summary>
        public OutDepotType OutDepotType
        {
            get { return this.GetProperty(OutDepotTypeProperty); }
            set { this.SetProperty(OutDepotTypeProperty, value); }
        }
        #endregion

        #region 状态 OutDepotState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        [Required]
        public static readonly Property<OutDepotState> OutDepotStateProperty = P<OutDepot>.Register(e => e.OutDepotState);

        /// <summary>
        /// 状态
        /// </summary>
        public OutDepotState OutDepotState
        {
            get { return this.GetProperty(OutDepotStateProperty); }
            set { this.SetProperty(OutDepotStateProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        [Required]
        public static readonly Property<QualityStatus?> QualityStatusProperty = P<OutDepot>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public QualityStatus? QualityStatus
        {
            get { return this.GetProperty(QualityStatusProperty); }
            set { this.SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 申请单号 ReleDoc
        /// <summary>
        /// 申请单号
        /// </summary>
        [Label("申请单号")]
        public static readonly Property<string> ReleDocProperty = P<OutDepot>.Register(e => e.ReleDoc);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string ReleDoc
        {
            get { return this.GetProperty(ReleDocProperty); }
            set { this.SetProperty(ReleDocProperty, value); }
        }
        #endregion

        #region 来源单号 SourceNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceNoProperty = P<OutDepot>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion

        //2022-06-15 张俊杰的需求 领用部门改成非必输，添加出库单那里。要加个检验部门不能为空
        #region 领用部门 GetDepartment
        /// <summary>
        /// 领用部门Id
        /// </summary>
        [Label("领用部门")]
        public static readonly IRefIdProperty GetDepartmentIdProperty =
            P<OutDepot>.RegisterRefId(e => e.GetDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 领用部门Id
        /// </summary>
        public double? GetDepartmentId
        {
            get { return (double?)this.GetRefNullableId(GetDepartmentIdProperty); }
            set { this.SetRefNullableId(GetDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 领用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> GetDepartmentProperty =
            P<OutDepot>.RegisterRef(e => e.GetDepartment, GetDepartmentIdProperty);

        /// <summary>
        /// 领用部门
        /// </summary>
        public Enterprise GetDepartment
        {
            get { return this.GetRefEntity(GetDepartmentProperty); }
            set { this.SetRefEntity(GetDepartmentProperty, value); }
        }
        #endregion

        #region 出库日期 OutDepotDate
        /// <summary>
        /// 出库日期
        /// </summary>
        [Label("出库日期")]
        public static readonly Property<DateTime?> OutDepotDateProperty = P<OutDepot>.Register(e => e.OutDepotDate);

        /// <summary>
        /// 出库日期
        /// </summary>
        public DateTime? OutDepotDate
        {
            get { return this.GetProperty(OutDepotDateProperty); }
            set { this.SetProperty(OutDepotDateProperty, value); }
        }
        #endregion

        #region 出库仓库 Warehouse
        /// <summary>
        /// 出库仓库Id
        /// </summary>
        [Label("出库仓库")]
        [Required]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<OutDepot>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 出库仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 出库仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<OutDepot>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<OutDepot>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<OutDepot>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<OutDepot>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double)this.GetRefId(EquipModelIdProperty); }
            set { this.SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<OutDepot>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 出库明细 PartOutDepotDetailList
        /// <summary>
        /// 出库明细
        /// </summary>
        [Label("出库明细")]
        public static readonly ListProperty<EntityList<PartOutDepotDetail>> PartOutDepotDetailListProperty = P<OutDepot>.RegisterList(e => e.PartOutDepotDetailList);
        /// <summary>
        /// 出库明细
        /// </summary>
        public EntityList<PartOutDepotDetail> PartOutDepotDetailList
        {
            get { return this.GetLazyList(PartOutDepotDetailListProperty); }
        }
        #endregion

        #region 申请单明细 OutDepotDetailList
        /// <summary>
        /// 申请单明细
        /// </summary>
        [Label("申请单明细")]
        public static readonly ListProperty<EntityList<OutDepotDetail>> OutDepotDetailListProperty = P<OutDepot>.RegisterList(e => e.OutDepotDetailList);

        /// <summary>
        /// 申请单明细
        /// </summary>
        public EntityList<OutDepotDetail> OutDepotDetailList
        {
            get { return this.GetLazyList(OutDepotDetailListProperty); }
        }
        #endregion

        #region 供应商信息 SupplierInfoList
        /// <summary>
        /// 供应商信息
        /// </summary>
        [Label("供应商信息")]
        public static readonly ListProperty<EntityList<SupplierInfo>> SupplierInfoListProperty = P<OutDepot>.RegisterList(e => e.SupplierInfoList);

        /// <summary>
        /// 供应商信息
        /// </summary>
        public EntityList<SupplierInfo> SupplierInfoList
        {
            get { return this.GetLazyList(SupplierInfoListProperty); }
        }
        #endregion

        #region 是否不是申请单推送 IsAppComeHere
        /// <summary>
        /// 是否不是申请单推送
        /// </summary>
        [Label("是否不是申请单推送")]
        public static readonly Property<YesNo> IsAppComeHereProperty = P<OutDepot>.Register(e => e.IsAppComeHere);

        /// <summary>
        /// 是否不是申请单推送
        /// </summary>
        public YesNo IsAppComeHere
        {
            get { return this.GetProperty(IsAppComeHereProperty); }
            set { this.SetProperty(IsAppComeHereProperty, value); }
        }
        #endregion

        #region 供应商编码 Supplier
        /// <summary>
        /// 供应商编码Id
        /// </summary>
        [Label("供应商编码")]
        public static readonly IRefIdProperty SupplierIdProperty =
            P<OutDepot>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商编码Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)this.GetRefNullableId(SupplierIdProperty); }
            set { this.SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty =
            P<OutDepot>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public Supplier Supplier
        {
            get { return this.GetRefEntity(SupplierProperty); }
            set { this.SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 发货方式 DeliveryWay
        /// <summary>
        /// 发货方式
        /// </summary>
        [Label("发货方式")]
        public static readonly Property<DeliveryWay?> DeliveryWayProperty = P<OutDepot>.Register(e => e.DeliveryWay);

        /// <summary>
        /// 发货方式
        /// </summary>
        public DeliveryWay? DeliveryWay
        {
            get { return this.GetProperty(DeliveryWayProperty); }
            set { this.SetProperty(DeliveryWayProperty, value); }
        }
        #endregion

        #region 预计返还日期 DepotRetDate
        /// <summary>
        /// 预计返还日期
        /// </summary>
        [Label("预计返还日期")]
        public static readonly Property<DateTime?> DepotRetDateProperty = P<OutDepot>.Register(e => e.DepotRetDate);

        /// <summary>
        /// 预计返还日期
        /// </summary>
        public DateTime? DepotRetDate
        {
            get { return this.GetProperty(DepotRetDateProperty); }
            set { this.SetProperty(DepotRetDateProperty, value); }
        }
        #endregion

        #region 维修出库状态 OutState
        /// <summary>
        /// 出库状态
        /// </summary>
        [Label("出库状态")]
        public static readonly Property<OutDepotState?> OutStateProperty = P<OutDepot>.Register(e => e.OutState);

        /// <summary>
        /// 出库状态
        /// </summary>
        public OutDepotState? OutState
        {
            get { return this.GetProperty(OutStateProperty); }
            set { this.SetProperty(OutStateProperty, value); }
        }
        #endregion

        #region 关单原因 CloseReason
        /// <summary>
        /// 关单原因
        /// </summary>
        [Label("关单原因")]
        public static readonly Property<string> CloseReasonProperty = P<OutDepot>.Register(e => e.CloseReason);

        /// <summary>
        /// 关单原因
        /// </summary>
        public string CloseReason
        {
            get { return this.GetProperty(CloseReasonProperty); }
            set { this.SetProperty(CloseReasonProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 出库仓库 WarehouseName
        /// <summary>
        /// 出库仓库
        /// </summary>
        [Label("出库仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<OutDepot>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<OutDepot>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<OutDepot>.RegisterView(e => e.Contacts, p => p.Supplier.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return this.GetProperty(ContactsProperty); }
        }
        #endregion

        #region 联系电话 ContactNumber
        /// <summary>
        /// 联系电话
        /// </summary>
        [Label("联系电话")]
        public static readonly Property<string> ContactNumberProperty = P<OutDepot>.RegisterView(e => e.ContactNumber, p => p.Supplier.ContactNumber);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber
        {
            get { return this.GetProperty(ContactNumberProperty); }
        }
        #endregion

        #region 联系地址 ContactAddress
        /// <summary>
        /// 联系地址
        /// </summary>
        [Label("联系地址")]
        public static readonly Property<string> ContactAddressProperty = P<OutDepot>.RegisterView(e => e.ContactAddress, p => p.Supplier.ContactAddress);

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress
        {
            get { return this.GetProperty(ContactAddressProperty); }
        }
        #endregion

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<OutDepot>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

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
        public static readonly Property<string> EquipAccountNameProperty = P<OutDepot>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库的属性

        #region 备件 SparePart
        /// <summary>
        /// 备件编码Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<OutDepot>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件编码Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)this.GetRefNullableId(SparePartIdProperty); }
            set { this.SetRefNullableId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件编码
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<OutDepot>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件编码
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<OutDepot>.Register(e => e.SparePartCode);

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
        public static readonly Property<string> SparePartNameProperty = P<OutDepot>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<OutDepot>.Register(e => e.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { this.SetProperty(ControlMethodProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<OutDepot>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<OutDepot>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<OutDepot>.Register(e => e.StorageLocationName);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
            set { this.SetProperty(StorageLocationNameProperty, value); }
        }
        #endregion

        #region 库位数量 StorageLocationNum
        /// <summary>
        /// 库位数量
        /// </summary>
        [Label("库位数量")]
        public static readonly Property<int?> StorageLocationNumProperty = P<OutDepot>.Register(e => e.StorageLocationNum);

        /// <summary>
        /// 库位数量
        /// </summary>
        public int? StorageLocationNum
        {
            get { return this.GetProperty(StorageLocationNumProperty); }
            set { this.SetProperty(StorageLocationNumProperty, value); }
        }
        #endregion

        #region 扫描值 ScanValue
        /// <summary>
        /// 扫描值
        /// </summary>
        [Label("扫描值")]
        public static readonly Property<string> ScanValueProperty = P<OutDepot>.Register(e => e.ScanValue);

        /// <summary>
        /// 扫描值
        /// </summary>
        public string ScanValue
        {
            get { return this.GetProperty(ScanValueProperty); }
            set { this.SetProperty(ScanValueProperty, value); }
        }
        #endregion

        #region 提示消息 Message
        /// <summary>
        /// 提示消息
        /// </summary>
        [Label("提示消息")]
        public static readonly Property<string> MessageProperty = P<OutDepot>.Register(e => e.Message);

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message
        {
            get { return this.GetProperty(MessageProperty); }
            set { this.SetProperty(MessageProperty, value); }
        }
        #endregion

        #region 推荐库位 AdviceStorageLocation
        /// <summary>
        /// 推荐库位
        /// </summary>
        [Label("推荐库位")]
        public static readonly Property<string> AdviceStorageLocationProperty = P<OutDepot>.Register(e => e.AdviceStorageLocation);

        /// <summary>
        /// 推荐库位
        /// </summary>
        public string AdviceStorageLocation
        {
            get { return this.GetProperty(AdviceStorageLocationProperty); }
            set { this.SetProperty(AdviceStorageLocationProperty, value); }
        }
        #endregion

        #region 是否扫描条码 IsBarcode
        /// <summary>
        /// 是否扫描条码
        /// </summary>
        [Label("是否扫描条码")]
        public static readonly Property<bool> IsBarcodeProperty = P<OutDepot>.Register(e => e.IsBarcode);

        /// <summary>
        /// 是否扫描条码
        /// </summary>
        public bool IsBarcode
        {
            get { return this.GetProperty(IsBarcodeProperty); }
            set { this.SetProperty(IsBarcodeProperty, value); }
        }
        #endregion

        #region 是否存在明细 IsExistDetail
        /// <summary>
        /// 是否存在明细
        /// </summary>
        [Label("是否存在明细")]
        public static readonly Property<bool> IsExistDetailProperty = P<OutDepot>.Register(e => e.IsExistDetail);

        /// <summary>
        /// 是否存在明细
        /// </summary>
        public bool IsExistDetail
        {
            get { return this.GetProperty(IsExistDetailProperty); }
            set { this.SetProperty(IsExistDetailProperty, value); }
        }
        #endregion

        #region 出库明细查询关键字 OutDepotDetailKeyWord
        /// <summary>
        /// 出库明细查询关键字
        /// </summary>
        [Label("出库明细查询关键字")]
        public static readonly Property<string> OutDepotDetailKeyWordProperty = P<OutDepot>.Register(e => e.OutDepotDetailKeyWord);

        /// <summary>
        /// 出库明细查询关键字
        /// </summary>
        public string OutDepotDetailKeyWord
        {
            get { return this.GetProperty(OutDepotDetailKeyWordProperty); }
            set { this.SetProperty(OutDepotDetailKeyWordProperty, value); }
        }
        #endregion

        #region 接收明细查询关键字 HandoverDetailKeyWord
        /// <summary>
        /// 接收明细查询关键字
        /// </summary>
        [Label("接收明细查询关键字")]
        public static readonly Property<string> HandoverDetailKeyWordProperty = P<OutDepot>.Register(e => e.HandoverDetailKeyWord);

        /// <summary>
        /// 接收明细查询关键字
        /// </summary>
        public string HandoverDetailKeyWord
        {
            get { return this.GetProperty(HandoverDetailKeyWordProperty); }
            set { this.SetProperty(HandoverDetailKeyWordProperty, value); }
        }
        #endregion

        #region 维修出库类型 RepairOutDepotType
        /// <summary>
        /// 出库类型
        /// </summary>
        [Label("出库类型")]
        public static readonly Property<OutDepotType> RepairOutDepotTypeProperty = P<OutDepot>.Register(e => e.RepairOutDepotType);

        /// <summary>
        /// 出库类型
        /// </summary>
        public OutDepotType RepairOutDepotType
        {
            get { return this.GetProperty(RepairOutDepotTypeProperty); }
            set { this.SetProperty(RepairOutDepotTypeProperty, value); }
        }
        #endregion

        #region 是否需要拣货 IsNeedPick
        /// <summary>
        /// 是否需要拣货
        /// </summary>
        [Label("是否需要拣货")]
        public static readonly Property<bool> IsNeedPickProperty = P<OutDepot>.Register(e => e.IsNeedPick);

        /// <summary>
        /// 是否需要拣货
        /// </summary>
        public bool IsNeedPick
        {
            get { return this.GetProperty(IsNeedPickProperty); }
            set { this.SetProperty(IsNeedPickProperty, value); }
        }
        #endregion

        #endregion
    }
    /// <summary>
    /// 备件出库 实体配置
    /// </summary>
    internal class OutDepotConfig : EntityConfig<OutDepot>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SPARE_PART_OUT_DEPOT").MapAllProperties();
            Meta.Property(OutDepot.SparePartIdProperty).DontMapColumn();
            Meta.Property(OutDepot.SparePartProperty).DontMapColumn();
            Meta.Property(OutDepot.SparePartCodeProperty).DontMapColumn();
            Meta.Property(OutDepot.SparePartNameProperty).DontMapColumn();
            Meta.Property(OutDepot.ControlMethodProperty).DontMapColumn();
            Meta.Property(OutDepot.StorageLocationIdProperty).DontMapColumn();
            Meta.Property(OutDepot.StorageLocationProperty).DontMapColumn();
            Meta.Property(OutDepot.StorageLocationNameProperty).DontMapColumn();
            Meta.Property(OutDepot.StorageLocationNumProperty).DontMapColumn();
            Meta.Property(OutDepot.ScanValueProperty).DontMapColumn();
            Meta.Property(OutDepot.MessageProperty).DontMapColumn();
            Meta.Property(OutDepot.AdviceStorageLocationProperty).DontMapColumn();
            Meta.Property(OutDepot.IsBarcodeProperty).DontMapColumn();
            Meta.Property(OutDepot.IsExistDetailProperty).DontMapColumn();
            Meta.Property(OutDepot.OutDepotDetailKeyWordProperty).DontMapColumn();
            Meta.Property(OutDepot.HandoverDetailKeyWordProperty).DontMapColumn();
            Meta.Property(OutDepot.RepairOutDepotTypeProperty).DontMapColumn();
            Meta.Property(OutDepot.IsNeedPickProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }

        /// <summary>
        /// 校验规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var para = o.CastTo<OutDepot>();
                    StringBuilder sb = new StringBuilder();

                    if (para.WarehouseId == null || para.WarehouseId == 0)
                    {
                        sb.AppendLine("【仓库】不能为空！".L10N());
                    }
                    if (para.GetDepartmentId == 0)
                    {
                        sb.AppendLine("【领用部门】不能为空！".L10N());
                    }
                    if (para.QualityStatus == null)
                    {
                        sb.AppendLine("【质量状态】不能为空！".L10N());
                    }

                    if (para.OutDepotType == OutDepotType.DgMaintain) 
                    {
                        if (para.SupplierId == null || para.SupplierId == 0) 
                        {
                            sb.AppendLine("【供应商编码】不能为空！".L10N());
                        }

                        if (para.DeliveryWay == null)
                        {
                            sb.AppendLine("【发货方式】不能为空！".L10N());
                        }

                        if (para.DepotRetDate == null)
                        {
                            sb.AppendLine("【预计返还日期】不能为空！".L10N());
                        }
                    }
                    e.BrokenDescription = sb.ToString();
                }
            }, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }
    }
}
