using SIE.Domain;
using SIE.LES.MaterialReceptions.Services;
using SIE.LES.StockOrders;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.MaterialReceptions
{
    /// <summary>
    /// 物料接收查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("物料接收查询实体")]
    public class MaterialReceptionCriterial : Criteria
    {
        #region 备料单号 StockOrderNo
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> StockOrderNoProperty = P<MaterialReceptionCriterial>.Register(e => e.StockOrderNo);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string StockOrderNo
        {
            get { return this.GetProperty(StockOrderNoProperty); }
            set { this.SetProperty(StockOrderNoProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReceiveState?> StateProperty = P<MaterialReceptionCriterial>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ReceiveState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialReceptionCriterial>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MaterialReceptionCriterial>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 标签号 LabelNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelNoProperty = P<MaterialReceptionCriterial>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion

        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<MaterialReceptionCriterial>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return this.GetProperty(LotNoProperty); }
            set { this.SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 接收仓库 Warehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<MaterialReceptionCriterial>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 接收仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<MaterialReceptionCriterial>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<MaterialReceptionCriterial>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<MaterialReceptionCriterial>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<MaterialReceptionCriterial>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 发运单号 SoNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> SoNoProperty = P<MaterialReceptionCriterial>.Register(e => e.SoNo);

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo
        {
            get { return this.GetProperty(SoNoProperty); }
            set { this.SetProperty(SoNoProperty, value); }
        }
        #endregion

        #region 接收人 Receiver
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty ReceiverIdProperty =
            P<MaterialReceptionCriterial>.RegisterRefId(e => e.ReceiverId, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double? ReceiverId
        {
            get { return (double?)this.GetRefNullableId(ReceiverIdProperty); }
            set { this.SetRefNullableId(ReceiverIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiverProperty =
            P<MaterialReceptionCriterial>.RegisterRef(e => e.Receiver, ReceiverIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee Receiver
        {
            get { return this.GetRefEntity(ReceiverProperty); }
            set { this.SetRefEntity(ReceiverProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiverTime
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateRange> ReceiverTimeProperty = P<MaterialReceptionCriterial>.Register(e => e.ReceiverTime);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateRange ReceiverTime
        {
            get { return this.GetProperty(ReceiverTimeProperty); }
            set { this.SetProperty(ReceiverTimeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询实现
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MaterialReceptionServices>().GetMaterialReceptions(this);
        }
    }
}
