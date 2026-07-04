using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Repairs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using FixtureEncode = SIE.Fixtures.Models.FixtureEncode;

namespace SIE.Fixtures.Querys.ViewModels
{
    /// <summary>
    /// 工治具查询ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureQueryViewModelCriteria))]
    [Label("工治具查询")]
    public class FixtureQueryViewModel : Entity<double>
    {
        #region 工治具台帐 FixtureAccount
        /// <summary>
        /// 工治具台帐Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<FixtureQueryViewModel>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<FixtureQueryViewModel>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具治具台帐
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureQueryViewModel>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return (double)GetRefId(FixtureEncodeIdProperty); }
            set { SetRefId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureQueryViewModel>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<FixtureQueryViewModel>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<FixtureQueryViewModel>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        public static readonly IRefIdProperty LocationIdProperty = P<FixtureQueryViewModel>.RegisterRefId(e => e.LocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? LocationId
        {
            get { return (double?)GetRefNullableId(LocationIdProperty); }
            set { SetRefNullableId(LocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> LocationProperty = P<FixtureQueryViewModel>.RegisterRef(e => e.Location, LocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation Location
        {
            get { return GetRefEntity(LocationProperty); }
            set { SetRefEntity(LocationProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> QtyProperty = P<FixtureQueryViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 位置状态 RepairBeforeState
        /// <summary>
        /// 位置状态
        /// </summary>
        [Label("位置状态")]
        public static readonly Property<RepairBeforeState> RepairBeforeStateProperty = P<FixtureQueryViewModel>.Register(e => e.RepairBeforeState);

        /// <summary>
        /// 位置状态
        /// </summary>
        public RepairBeforeState RepairBeforeState
        {
            get { return GetProperty(RepairBeforeStateProperty); }
            set { SetProperty(RepairBeforeStateProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        public static readonly IRefIdProperty ResourceIdProperty = P<FixtureQueryViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<FixtureQueryViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<FixtureQueryViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<FixtureQueryViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工治具ID AccountCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> AccountCodeProperty = P<FixtureQueryViewModel>.RegisterView(e => e.AccountCode, p => p.FixtureAccount.Code);

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
        public static readonly Property<string> EncodeCodeProperty = P<FixtureQueryViewModel>.RegisterView(e => e.EncodeCode, p => p.FixtureAccount.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
            set { this.SetProperty(EncodeCodeProperty, value); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureQueryViewModel>.RegisterView(e => e.ModelCode, p => p.FixtureAccount.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureQueryViewModel>.RegisterView(e => e.ModelName, p => p.FixtureAccount.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty = P<FixtureQueryViewModel>.RegisterView(e => e.FixtureType, p => p.FixtureAccount.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
            set { this.SetProperty(FixtureTypeProperty, value); }
        }
        #endregion

        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Label("管理方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<FixtureQueryViewModel>.RegisterView(e => e.ManageMode, p => p.FixtureAccount.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
            set { this.SetProperty(ManageModeProperty, value); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<FixtureQueryViewModel>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 库位名称 LocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> LocationNameProperty = P<FixtureQueryViewModel>.RegisterView(e => e.LocationName, p => p.Location.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName
        {
            get { return this.GetProperty(LocationNameProperty); }
            set { this.SetProperty(LocationNameProperty, value); }
        }
        #endregion

        #region 产线 ResourceName
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> ResourceNameProperty = P<FixtureQueryViewModel>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<FixtureQueryViewModel>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class FixtureQueryViewModelEntityConfig : EntityConfig<FixtureQueryViewModel>
    {
        //// <summary>
        //// 配置元数据
        //// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<FixtureAccount>("acc")
                    .LeftJoin<FixtureEncode>("en", (acc, en) => acc.FixtureEncodeId == en.Id
                        && en.SQL<int>("en.IS_PHANTOM") == 0
                        && en.SQL<int>("en.INV_ORG_ID") == RT.InvOrg)
                    .Select<FixtureEncode>((acc, en) => new
                    {
                        acc.Id,
                        FIXTURE_ACCOUNT_ID = acc.Id,
                        FIXTURE_ENCODE_ID = acc.FixtureEncodeId,
                        WAREHOUSE_ID = acc.WarehouseId,
                        LOCATION_ID = acc.LocationId,
                        RESOURCE_ID = acc.Id,
                        WORK_ORDER_ID = acc.Id,
                        REPAIR_BEFORE_STATE = acc.AccountState,
                        QTY = acc.InStockQty,

                    })
                .Where(acc => acc.SQL<int>("acc.INV_ORG_ID") == RT.InvOrg)
                .Where(acc => acc.SQL<int>("acc.IS_PHANTOM") == 0).ToQuery();

            Meta.MapView(view).MapAllProperties();
            Meta.IsTreeEntity = false;
        }
    }
}
