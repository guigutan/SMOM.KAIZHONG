using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("物料接收记录查询实体")]
    public class MaterialReceiveRecordCriteria : Criteria
    {
        #region 发运单号 SoNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> SoNoProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.SoNo);

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo
        {
            get { return this.GetProperty(SoNoProperty); }
            set { this.SetProperty(SoNoProperty, value); }
        }
        #endregion

        #region 备料单号 StockOrderNo
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> StockOrderNoProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.StockOrderNo);

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
        public static readonly Property<ReceiveState?> StateProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.State);

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
        public static readonly Property<string> ItemCodeProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.ItemName);

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
        public static readonly Property<string> LabelNoProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.LotCode);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 接收仓库 Warehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<MaterialReceiveRecordCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
            P<MaterialReceiveRecordCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        public static readonly Property<string> WorkOrderNoProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<MaterialReceiveRecordCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<MaterialReceiveRecordCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<MaterialReceiveRecordCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<MaterialReceiveRecordCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 接收方式 ReceiveType
        /// <summary>
        /// 接收方式
        /// </summary>
        [Label("接收方式")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收方式
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return this.GetProperty(ReceiveTypeProperty); }
            set { this.SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

        #region 接收人 ReceiveBy
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty ReceiveByIdProperty =
            P<MaterialReceiveRecordCriteria>.RegisterRefId(e => e.ReceiveById, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double? ReceiveById
        {
            get { return (double?)this.GetRefId(ReceiveByIdProperty); }
            set { this.SetRefId(ReceiveByIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiveByProperty =
            P<MaterialReceiveRecordCriteria>.RegisterRef(e => e.ReceiveBy, ReceiveByIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee ReceiveBy
        {
            get { return this.GetRefEntity(ReceiveByProperty); }
            set { this.SetRefEntity(ReceiveByProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiveTime
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateRange> ReceiveTimeProperty = P<MaterialReceiveRecordCriteria>.Register(e => e.ReceiveTime);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateRange ReceiveTime
        {
            get { return this.GetProperty(ReceiveTimeProperty); }
            set { this.SetProperty(ReceiveTimeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询实现
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MaterialReceiveController>().GetMaterialReceiveRecords(this);
        }
    }
}
