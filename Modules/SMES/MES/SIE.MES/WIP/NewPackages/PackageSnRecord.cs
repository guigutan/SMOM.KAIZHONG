using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WIP.NewPackages
{
    /// <summary>
    /// 待包装SN扫描记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("待包装SN扫描记录")]
    public class PackageSnRecord : DataEntity
    {
        #region 条码号 Sn
        /// <summary>
        /// 条码号
        /// </summary>
        [Label("条码号")]
        public static readonly Property<string> SnProperty = P<PackageSnRecord>.Register(e => e.Sn);

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<PackageSnRecord>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<PackageSnRecord>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<PackageSnRecord>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)this.GetRefId(ProductIdProperty); }
            set { this.SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<PackageSnRecord>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 包装单位 PackageUnit
        /// <summary>
        /// 包装单位ID
        /// </summary>
        [Label("包装单位")]
        public static readonly IRefIdProperty PackageUnitIdProperty =
                 P<PackageSnRecord>.RegisterRefId(e => e.PackageUnitId, ReferenceType.Normal);

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId
        {
            get { return (double)this.GetRefId(PackageUnitIdProperty); }
            set { this.SetRefId(PackageUnitIdProperty, value); }
        }

        /// <summary>
        /// 包装单位
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> PackageUnitProperty =
            P<PackageSnRecord>.RegisterRef(e => e.PackageUnit, PackageUnitIdProperty);

        /// <summary>
        /// 包装单位
        /// </summary>
        public PackingUnit PackageUnit
        {
            get { return this.GetRefEntity(PackageUnitProperty); }
            set { this.SetRefEntity(PackageUnitProperty, value); }
        }
        #endregion

        #region 工单条码号 ParentNo
        /// <summary>
        /// 工单条码号
        /// </summary>
        [MaxLength(2000)]
        [Label("工单条码号")]
        public static readonly Property<string> WoSnProperty = P<PackageSnRecord>.Register(e => e.WoSn);

        /// <summary>
        /// 工单条码号
        /// </summary>
        public string WoSn
        {
            get { return this.GetProperty(WoSnProperty); }
            set { this.SetProperty(WoSnProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<PackageSnRecord>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<PackageSnRecord>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<PackageSnRecord>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<PackageSnRecord>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<PackageSnRecord>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)this.GetRefId(StationIdProperty); }
            set { this.SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<PackageSnRecord>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 已加入包装数 PackedQty
        /// <summary>
        /// 已加入包装数
        /// </summary>
        [Label("已加入包装数")]
        public static readonly Property<decimal> PackedQtyProperty = P<PackageSnRecord>.Register(e => e.PackedQty);

        /// <summary>
        /// 已加入包装数
        /// </summary>
        public decimal PackedQty
        {
            get { return GetProperty(PackedQtyProperty); }
            set { SetProperty(PackedQtyProperty, value); }
        }
        #endregion

        #region 物料数量
        /// <summary>
        /// 物料数量
        /// </summary>
        [Label("物料数量")]
        public static readonly Property<decimal> ItemQtyProperty = P<PackageSnRecord>.Register(e => e.ItemQty);

        /// <summary>
        /// 物料数量
        /// </summary>
        public decimal ItemQty
        {
            get { return this.GetProperty(ItemQtyProperty); }
            set { this.SetProperty(ItemQtyProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 包装单位名称 PackageUnitName
        /// <summary>
        /// 包装单位名称
        /// </summary>
        [Label("包装单位名称")]
        public static readonly Property<string> PackageUnitNameProperty = P<PackageSnRecord>.RegisterView(e => e.PackageUnitName, p => p.PackageUnit.Name);

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackageUnitName
        {
            get { return this.GetProperty(PackageUnitNameProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<PackageSnRecord>.RegisterView(e => e.ProductCode, p => p.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<PackageSnRecord>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<PackageSnRecord>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 待包装SN扫描记录 实体配置
    /// </summary>
    internal class PackageSnRecordConfig : EntityConfig<PackageSnRecord>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PACKAGE_SN_RECORD").MapAllProperties();
            Meta.Property(PackageSnRecord.WoSnProperty).MapColumn().HasLength(4000);
            Meta.DisablePhantoms();
        }
    }
}
