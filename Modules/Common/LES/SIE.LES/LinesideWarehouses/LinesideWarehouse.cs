using NPOI.SS.Formula.Functions;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.LinesideWarehouses
{
    /// <summary>
    /// 产线线边仓维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(LinesideWarehouseCriteria))]
    [Label("产线线边仓维护")]
    public partial class LinesideWarehouse : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<LinesideWarehouse>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<LinesideWarehouse>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<LinesideWarehouse>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<LinesideWarehouse>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 WipResouce
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResouceIdProperty =
            P<LinesideWarehouse>.RegisterRefId(e => e.WipResouceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResouceId
        {
            get { return (double?)this.GetRefNullableId(WipResouceIdProperty); }
            set { this.SetRefNullableId(WipResouceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResouceProperty =
            P<LinesideWarehouse>.RegisterRef(e => e.WipResouce, WipResouceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResouce
        {
            get { return this.GetRefEntity(WipResouceProperty); }
            set { this.SetRefEntity(WipResouceProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<LinesideWarehouse>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<LinesideWarehouse>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<LinesideWarehouse>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double StorageLocationId
        {
            get { return (double)GetRefId(StorageLocationIdProperty); }
            set { SetRefId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<LinesideWarehouse>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 物料自动接收 AutoReceive
        /// <summary>
        /// 物料自动接收
        /// </summary>
        [Label("物料自动接收")]
        public static readonly Property<bool> AutoReceiveProperty = P<LinesideWarehouse>.Register(e => e.AutoReceive);

        /// <summary>
        /// 物料自动接收
        /// </summary>
        public bool AutoReceive
        {
            get { return this.GetProperty(AutoReceiveProperty); }
            set { this.SetProperty(AutoReceiveProperty, value); }
        }
        #endregion

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<LinesideWarehouse>.RegisterView(e => e.FactoryCode, p => p.Factory.Code);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
        }
        #endregion

        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<LinesideWarehouse>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<LinesideWarehouse>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
            set { this.SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<LinesideWarehouse>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 资源编码 WipResouceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> WipResouceCodeProperty = P<LinesideWarehouse>.RegisterView(e => e.WipResouceCode, p => p.WipResouce.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string WipResouceCode
        {
            get { return this.GetProperty(WipResouceCodeProperty); }
        }
        #endregion

        #region 资源名称 WipResouceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> WipResouceNameProperty = P<LinesideWarehouse>.RegisterView(e => e.WipResouceName, p => p.WipResouce.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string WipResouceName
        {
            get { return this.GetProperty(WipResouceNameProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<LinesideWarehouse>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<LinesideWarehouse>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库名
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 库位编码 LocaltionCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> LocaltionCodeProperty = P<LinesideWarehouse>.RegisterView(e => e.LocaltionCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocaltionCode
        {
            get { return this.GetProperty(LocaltionCodeProperty); }
        }
        #endregion

        #region 库位名称 LocaltionName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> LocaltionNameProperty = P<LinesideWarehouse>.RegisterView(e => e.LocaltionName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocaltionName
        {
            get { return this.GetProperty(LocaltionNameProperty); }
        }
        #endregion


        #region 产线线边仓名称 Name
        /// <summary>
        /// 产线线边仓名称
        /// </summary>
        [Label("产线线边仓名称")]
        public static readonly Property<string> NameProperty = P<LinesideWarehouse>.RegisterReadOnly(
            e => e.Name, e => e.GetLinesideWarehouseName(), WipResouceProperty, WarehouseProperty, StorageLocationProperty);

        /// <summary>
        /// 产线线边仓名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
        }

        /// <summary>
        /// 获取产线线边仓名称
        /// </summary>
        /// <returns>产线线边仓名称</returns>
        private string GetLinesideWarehouseName()
        {
            if (WipResouce == null)
                return string.Empty;
            if (Warehouse == null)
                return string.Empty;
            if (StorageLocation == null)
                return string.Empty;
            return WipResouce.Name + "-" + Warehouse.Name + "-" + StorageLocation.Name;
        }
        #endregion

    }

    /// <summary>
    /// 产线线边仓维护 实体配置
    /// </summary>
    internal class LinesideWarehouseConfig : EntityConfig<LinesideWarehouse>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LINE_SIDE_WAREHOUSE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}