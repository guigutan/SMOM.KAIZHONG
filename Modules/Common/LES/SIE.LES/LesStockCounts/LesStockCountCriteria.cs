using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 盘点单
    /// </summary>
    [QueryEntity, Serializable]
    [Label("线边仓盘点查询实体")]
    public partial class LesStockCountCriteria : Criteria
    {       
        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<LesStockCountCriteria>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<string> StateProperty = P<LesStockCountCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public string State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 盘点结果 StockCountResult
        /// <summary>
        /// 盘点结果
        /// </summary>
        [Label("盘点结果")]
        public static readonly Property<LesStockCountResult?> StockCountResultProperty = P<LesStockCountCriteria>.Register(e => e.LesStockCountResult);

        /// <summary>
        /// 盘点结果
        /// </summary>
        public LesStockCountResult? LesStockCountResult
        {
            get { return GetProperty(StockCountResultProperty); }
            set { SetProperty(StockCountResultProperty, value); }
        }
        #endregion
                
        #region 关联单据 SourceBillNo
        /// <summary>
        /// 关联单据
        /// </summary>
        [Label("关联单据")]
        public static readonly Property<string> SourceBillNoProperty = P<LesStockCountCriteria>.Register(e => e.SourceBillNo);

        /// <summary>
        /// 关联单据
        /// </summary>
        public string SourceBillNo
        {
            get { return this.GetProperty(SourceBillNoProperty); }
            set { this.SetProperty(SourceBillNoProperty, value); }
        }
        #endregion        

        #region 物料 Item
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<LesStockCountCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<LesStockCountCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<LesStockCountCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<LesStockCountCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<LesStockCountCriteria>.Register(e => e.CreateDate);

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
        public static readonly IRefIdProperty CreateByIdProperty = P<LesStockCountCriteria>.RegisterRefId(e => e.CreateById, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> CreateByProperty = P<LesStockCountCriteria>.RegisterRef(e => e.CreateBy, CreateByIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee CreateBy
        {
            get { return GetRefEntity(CreateByProperty); }
            set { SetRefEntity(CreateByProperty, value); }
        }
        #endregion

        #region 盘点细度 CountDimension
        /// <summary>
        /// 盘点细度
        /// </summary>
        [Label("盘点细度")]
        public static readonly Property<CountDimension?> CountDimensionProperty = P<LesStockCountCriteria>.Register(e => e.CountDimension);

        /// <summary>
        /// 盘点细度
        /// </summary>
        public CountDimension? CountDimension
        {
            get { return GetProperty(CountDimensionProperty); }
            set { SetProperty(CountDimensionProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<LesStockCountController>().GetLesStockCounts(this);
        }
    }
}
