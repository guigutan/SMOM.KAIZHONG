using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料单合并下发规则查询条件
    /// </summary>
    [QueryEntity, Serializable]
    public class StockOrderMergeIssuedCriteria : Criteria
    {
        #region 生产资源 WipResource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<StockOrderMergeIssuedCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<StockOrderMergeIssuedCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 备料模式 StockModel
        /// <summary>
        /// 备料模式
        /// </summary>
        [Label("备料模式")]
        public static readonly Property<PrepareItemType?> StockModelProperty = P<StockOrderMergeIssuedCriteria>.Register(e => e.StockModel);

        /// <summary>
        /// 备料模式
        /// </summary>
        public PrepareItemType? StockModel
        {
            get => GetProperty(StockModelProperty);
            set => SetProperty(StockModelProperty, value);
        }
        #endregion

        #region 接收仓库 Warehouse
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<StockOrderMergeIssuedCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 接收仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<StockOrderMergeIssuedCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 状态 State  
        /// <summary>  
        /// 状态  
        /// </summary>  
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<StockOrderMergeIssuedCriteria>.Register(e => e.State);

        /// <summary>  
        /// 状态  
        /// </summary>  
        public State? State
        {
            get => GetProperty(StateProperty);
            set => SetProperty(StateProperty, value);
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<StockOrderMergeIssuedCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 创建人 CreateBy
        /// <summary>
        /// 创建人Id
        /// </summary>
        [Label("创建人")]
        public static readonly IRefIdProperty CreateByIdProperty = P<StockOrderMergeIssuedCriteria>.RegisterRefId(e => e.CreateById, ReferenceType.Normal);

        /// <summary>
        /// 创建人Id
        /// </summary>
        public double? CreateById
        {
            get { return (double?)GetRefNullableId(CreateByIdProperty); }
            set { SetRefNullableId(CreateByIdProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CreateByProperty = P<StockOrderMergeIssuedCriteria>.RegisterRef(e => e.CreateBy, CreateByIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee CreateBy
        {
            get { return GetRefEntity(CreateByProperty); }
            set { SetRefEntity(CreateByProperty, value); }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<StockOrderMergeIssuedController>().GetMergeIssued(this);
        }
    }
}
