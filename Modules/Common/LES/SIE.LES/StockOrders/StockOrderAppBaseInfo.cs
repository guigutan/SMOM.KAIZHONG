using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料单APP用户基本信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("备料单APP用户基本信息")]
    public class StockOrderAppBaseInfo : DataEntity
    {
        #region 用户Id EmployeeId
        /// <summary>
        /// 用户Id
        /// </summary>
        [Label("用户Id")]
        public static readonly Property<double> EmployeeIdProperty = P<StockOrderAppBaseInfo>.Register(e => e.EmployeeId);

        /// <summary>
        /// 用户Id
        /// </summary>
        public double EmployeeId
        {
            get { return this.GetProperty(EmployeeIdProperty); }
            set { this.SetProperty(EmployeeIdProperty, value); }
        }
        #endregion

        #region 工厂Id FactoryId
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂Id")]
        public static readonly Property<double> FactoryIdProperty = P<StockOrderAppBaseInfo>.Register(e => e.FactoryId);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
            set { this.SetProperty(FactoryIdProperty, value); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<StockOrderAppBaseInfo>.Register(e => e.FactoryName);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
            set { this.SetProperty(FactoryNameProperty, value); }
        }
        #endregion

        #region 车间Id WorkShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double?> WorkShopIdProperty = P<StockOrderAppBaseInfo>.Register(e => e.WorkShopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return this.GetProperty(WorkShopIdProperty); }
            set { this.SetProperty(WorkShopIdProperty, value); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<StockOrderAppBaseInfo>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { this.SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 生产资源Id ResourceId
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源Id")]
        public static readonly Property<double?> ResourceIdProperty = P<StockOrderAppBaseInfo>.Register(e => e.ResourceId);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
            set { this.SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 生产资源名称 ResourceName
        /// <summary>
        /// 生产资源名称
        /// </summary>
        [Label("生产资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<StockOrderAppBaseInfo>.Register(e => e.ResourceName);

        /// <summary>
        /// 生产资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 接收仓库Id WarehouseId
        /// <summary>
        /// 接收仓库Id
        /// </summary>
        [Label("接收仓库Id")]
        public static readonly Property<double?> WarehouseIdProperty = P<StockOrderAppBaseInfo>.Register(e => e.WarehouseId);

        /// <summary>
        /// 接收仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
            set { this.SetProperty(WarehouseIdProperty, value); }
        }
        #endregion

        #region 接收仓库名称 WarehouseName
        /// <summary>
        /// 接收仓库名称
        /// </summary>
        [Label("接收仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<StockOrderAppBaseInfo>.Register(e => e.WarehouseName);

        /// <summary>
        /// 接收仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 接收库位Id StorageId
        /// <summary>
        /// 接收库位Id
        /// </summary>
        [Label("接收库位Id")]
        public static readonly Property<double?> StorageIdProperty = P<StockOrderAppBaseInfo>.Register(e => e.StorageId);

        /// <summary>
        /// 接收库位Id
        /// </summary>
        public double? StorageId
        {
            get { return this.GetProperty(StorageIdProperty); }
            set { this.SetProperty(StorageIdProperty, value); }
        }
        #endregion

        #region 接收库位名称 StorageName
        /// <summary>
        /// 接收库位名称
        /// </summary>
        [Label("接收库位名称")]
        public static readonly Property<string> StorageNameProperty = P<StockOrderAppBaseInfo>.Register(e => e.StorageName);

        /// <summary>
        /// 接收库位名称
        /// </summary>
        public string StorageName
        {
            get { return this.GetProperty(StorageNameProperty); }
            set { this.SetProperty(StorageNameProperty, value); }
        }
        #endregion

        #region 备料模式 StockType
        /// <summary>
        /// 备料模式
        /// </summary>
        [Label("备料模式")]
        public static readonly Property<PrepareItemType> StockTypeProperty = P<StockOrderAppBaseInfo>.Register(e => e.StockType);

        /// <summary>
        /// 备料模式
        /// </summary>
        public PrepareItemType StockType
        {
            get { return this.GetProperty(StockTypeProperty); }
            set { this.SetProperty(StockTypeProperty, value); }
        }
        #endregion

        #region 接收方式 ReceiveType
        /// <summary>
        /// 接收方式
        /// </summary>
        [Label("接收方式")]
        public static readonly Property<StockReceiveType> ReceiveTypeProperty = P<StockOrderAppBaseInfo>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收方式
        /// </summary>
        public StockReceiveType ReceiveType
        {
            get { return this.GetProperty(ReceiveTypeProperty); }
            set { this.SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    public class StockOrderAppBaseInfoConfig: EntityConfig<StockOrderAppBaseInfo>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("STOCK_APP_INFO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
