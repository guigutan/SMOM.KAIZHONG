using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Core.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using SIE.Inventory.Commom;

namespace SIE.Inventory.Task
{
    /// <summary>
	/// 任务组
	/// </summary>
	[RootEntity, Serializable]
    [CriteriaQuery]
    [EntityWithConfig(typeof(NoConfig))]
    [Label("任务组")]
    public partial class TaskGroup : DataEntity
    {
        #region 组号 No
        /// <summary>
        /// 组号
        /// </summary>
        [Label("组号")]
        public static readonly Property<string> NoProperty = P<TaskGroup>.Register(e => e.No);

        /// <summary>
        /// 组号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<TaskGroup>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 盘点单Id BillId
        /// <summary>
        /// 盘点单Id
        /// </summary>
        [Label("盘点单Id")]
        public static readonly Property<double> BillIdProperty = P<TaskGroup>.Register(e => e.BillId);

        /// <summary>
        /// 盘点单Id
        /// </summary>
        public double BillId
        {
            get { return GetProperty(BillIdProperty); }
            set { SetProperty(BillIdProperty, value); }
        }
        #endregion

        #region 释放日期 ReleaseDate
        /// <summary>
        /// 释放日期
        /// </summary>
        [Label("释放日期")]
        public static readonly Property<DateTime?> ReleaseDateProperty = P<TaskGroup>.Register(e => e.ReleaseDate);

        /// <summary>
        /// 释放日期
        /// </summary>
        public DateTime? ReleaseDate
        {
            get { return GetProperty(ReleaseDateProperty); }
            set { SetProperty(ReleaseDateProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<TaskGroup>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<TaskGroup>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<TaskGroup>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<OrderType> OrderTypeProperty = P<TaskGroup>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区Id
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<TaskGroup>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区Id
        /// </summary>
        public double? StorageAreaId
        {
            get { return (double?)GetRefNullableId(StorageAreaIdProperty); }
            set { SetRefNullableId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<TaskGroup>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 库区编码 StorageAreaCode
        /// <summary>
        /// 库区编码
        /// </summary>
        [Label("库区编码")]
        public static readonly Property<string> StorageAreaCodeProperty = P<TaskGroup>.RegisterView(e => e.StorageAreaCode, p => p.StorageArea.Code);

        /// <summary>
        /// 库区编码
        /// </summary>
        public string StorageAreaCode
        {
            get { return this.GetProperty(StorageAreaCodeProperty); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<TaskGroup>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)GetRefNullableId(StorageLocationIdProperty); }
            set { SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<TaskGroup>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<TaskGroup>.RegisterView(e => e.StorageLocationCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
        }
        #endregion

        #region 盘点细度 CountDimension
        /// <summary>
        /// 盘点细度
        /// </summary>
        [Label("盘点细度")]
        public static readonly Property<CountDimension> CountDimensionProperty = P<TaskGroup>.Register(e => e.CountDimension);

        /// <summary>
        /// 盘点细度
        /// </summary>
        public CountDimension CountDimension
        {
            get { return GetProperty(CountDimensionProperty); }
            set { SetProperty(CountDimensionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class TaskGroupConfig : EntityConfig<TaskGroup>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TASK_GROUP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}