using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.Querys.ViewModels
{
    /// <summary>
    /// 出库ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("出库")]
    public class UnloadViewModel : Entity<double>
    {
        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        public static readonly IRefIdProperty ResourceIdProperty = P<UnloadViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<UnloadViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

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
        public static readonly IRefIdProperty WorkOrderIdProperty = P<UnloadViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<UnloadViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工治具台帐 FixtureAccount
        /// <summary>
        /// 工治具台帐Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<UnloadViewModel>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<UnloadViewModel>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具治具台帐
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<UnloadViewModel>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<UnloadViewModel>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        public static readonly IRefIdProperty LocationIdProperty = P<UnloadViewModel>.RegisterRefId(e => e.LocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> LocationProperty = P<UnloadViewModel>.RegisterRef(e => e.Location, LocationIdProperty);

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
        public static readonly Property<int> QtyProperty = P<UnloadViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 出库数量 UnloadQty
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        public static readonly Property<int> UnloadQtyProperty = P<UnloadViewModel>.Register(e => e.UnloadQty);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int UnloadQty
        {
            get { return GetProperty(UnloadQtyProperty); }
            set { SetProperty(UnloadQtyProperty, value); }
        }
        #endregion

        #region 关联载具 TurnoverToolCode
        /// <summary>
        /// 关联载具
        /// </summary>
        [Label("关联载具")]
        public static readonly Property<string> TurnoverToolCodeProperty = P<UnloadViewModel>.Register(e => e.TurnoverToolCode);

        /// <summary>
        /// 关联载具
        /// </summary>
        public string TurnoverToolCode
        {
            get { return GetProperty(TurnoverToolCodeProperty); }
            set { SetProperty(TurnoverToolCodeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工治具ID AccountCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> AccountCodeProperty = P<UnloadViewModel>.RegisterView(e => e.AccountCode, p => p.FixtureAccount.Code);

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
        public static readonly Property<string> EncodeCodeProperty = P<UnloadViewModel>.RegisterView(e => e.EncodeCode, p => p.FixtureAccount.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
            set { this.SetProperty(EncodeCodeProperty, value); }
        }
        #endregion

        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Label("管理方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<UnloadViewModel>.RegisterView(e => e.ManageMode, p => p.FixtureAccount.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
            set { this.SetProperty(ManageModeProperty, value); }
        }
        #endregion

        #endregion
    }
}
