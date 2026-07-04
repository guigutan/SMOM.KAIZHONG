using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯管理叫料子表
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安灯管理叫料子表")]
    public class AndonManageCallMaterial : DataEntity
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public AndonManageCallMaterial()
        {
            Qty = 0;
            TimeNeed = DateTime.Now;
        }

        #region 安灯管理 AndonManage
        /// <summary>
        /// 安灯管理Id
        /// </summary>
        [Label("安灯管理")]
        public static readonly IRefIdProperty AndonManageIdProperty =
            P<AndonManageCallMaterial>.RegisterRefId(e => e.AndonManageId, ReferenceType.Parent);

        /// <summary>
        /// 安灯管理Id
        /// </summary>
        public double AndonManageId
        {
            get { return (double)this.GetRefId(AndonManageIdProperty); }
            set { this.SetRefId(AndonManageIdProperty, value); }
        }

        /// <summary>
        /// 安灯管理
        /// </summary>
        public static readonly RefEntityProperty<AndonManage> AndonManageProperty =
            P<AndonManageCallMaterial>.RegisterRef(e => e.AndonManage, AndonManageIdProperty);

        /// <summary>
        /// 安灯管理
        /// </summary>
        public AndonManage AndonManage
        {
            get { return this.GetRefEntity(AndonManageProperty); }
            set { this.SetRefEntity(AndonManageProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<AndonManageCallMaterial>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<AndonManageCallMaterial>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料消耗类型 ConsumeType
        /// <summary>
        /// 物料消耗类型
        /// </summary>
        [Label("物料消耗类型")]
        public static readonly Property<ConsumeMode?> ConsumeTypeProperty = P<AndonManageCallMaterial>.Register(e => e.ConsumeType);

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode? ConsumeType
        {
            get { return this.GetProperty(ConsumeTypeProperty); }
            set { this.SetProperty(ConsumeTypeProperty, value); }
        }
        #endregion

        #region 本次备料数量 Qty
        /// <summary>
        /// 本次备料数量
        /// </summary>
        [Label("本次备料数量")]
        public static readonly Property<decimal> QtyProperty = P<AndonManageCallMaterial>.Register(e => e.Qty);

        /// <summary>
        /// 本次备料数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 需求时间 TimeNeed
        /// <summary>
        /// 需求时间
        /// </summary>
        [Label("需求时间")]
        public static readonly Property<DateTime> TimeNeedProperty = P<AndonManageCallMaterial>.Register(e => e.TimeNeed);

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime TimeNeed
        {
            get { return this.GetProperty(TimeNeedProperty); }
            set { this.SetProperty(TimeNeedProperty, value); }
        }
        #endregion

        #region 备料接收仓库 WareHouse
        /// <summary>
        /// 备料接收仓库Id
        /// </summary>
        [Label("备料接收仓库")]
        public static readonly IRefIdProperty WareHouseIdProperty =
            P<AndonManageCallMaterial>.RegisterRefId(e => e.WareHouseId, ReferenceType.Normal);

        /// <summary>
        /// 备料接收仓库Id
        /// </summary>
        public double? WareHouseId
        {
            get { return (double?)this.GetRefNullableId(WareHouseIdProperty); }
            set { this.SetRefNullableId(WareHouseIdProperty, value); }
        }

        /// <summary>
        /// 备料接收仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WareHouseProperty =
            P<AndonManageCallMaterial>.RegisterRef(e => e.WareHouse, WareHouseIdProperty);

        /// <summary>
        /// 备料接收仓库
        /// </summary>
        public Warehouse WareHouse
        {
            get { return this.GetRefEntity(WareHouseProperty); }
            set { this.SetRefEntity(WareHouseProperty, value); }
        }
        #endregion

        #region 备料接收库位 StorageLocation
        /// <summary>
        /// 备料接收库位Id
        /// </summary>
        [Label("备料接收库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<AndonManageCallMaterial>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 备料接收库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 备料接收库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<AndonManageCallMaterial>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 备料接收库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 备料单号 No
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> NoProperty = P<AndonManageCallMaterial>.Register(e => e.No);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 备料单行号 LineNo
        /// <summary>
        /// 备料单行号
        /// </summary>
        [Label("备料单行号")]
        public static readonly Property<string> LineNoProperty = P<AndonManageCallMaterial>.Register(e => e.LineNo);

        /// <summary>
        /// 备料单行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<AndonManageCallMaterial>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<AndonManageCallMaterial>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料消耗类型(视图属性) ConsumeModeView
        /// <summary>
        /// 物料消耗类型(视图属性)
        /// </summary>
        [Label("物料消耗类型(视图属性)")]
        public static readonly Property<ConsumeMode?> ConsumeModeViewProperty = P<AndonManageCallMaterial>.RegisterView(e => e.ConsumeModeView, p => p.Item.ConsumeMode);

        /// <summary>
        /// 物料消耗类型(视图属性)
        /// </summary>
        public ConsumeMode? ConsumeModeView
        {
            get { return this.GetProperty(ConsumeModeViewProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库属性
        #region 工厂Id FactoryId
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂Id")]
        public static readonly Property<double> FactoryIdProperty = P<AndonManageCallMaterial>.Register(e => e.FactoryId);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
            set { this.SetProperty(FactoryIdProperty, value); }
        }
        #endregion

        #region 资源Id WipId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly Property<double> WipIdProperty = P<AndonManageCallMaterial>.Register(e => e.WipId);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double WipId
        {
            get { return this.GetProperty(WipIdProperty); }
            set { this.SetProperty(WipIdProperty, value); }
        }
        #endregion

        #region 车间Id WorkShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double> WorkShopIdProperty = P<AndonManageCallMaterial>.Register(e => e.WorkShopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return this.GetProperty(WorkShopIdProperty); }
            set { this.SetProperty(WorkShopIdProperty, value); }
        }
        #endregion

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double?> WorkOrderIdProperty = P<AndonManageCallMaterial>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 工序Id ProcessId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double?> ProcessIdProperty = P<AndonManageCallMaterial>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 仓库名称 WareHouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WareHouseNameProperty = P<AndonManageCallMaterial>.Register(e => e.WareHouseName);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseName
        {
            get { return this.GetProperty(WareHouseNameProperty); }
            set { this.SetProperty(WareHouseNameProperty, value); }
        }
        #endregion

        #region 库位名称 LocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> LocationNameProperty = P<AndonManageCallMaterial>.Register(e => e.LocationName);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName
        {
            get { return this.GetProperty(LocationNameProperty); }
            set { this.SetProperty(LocationNameProperty, value); }
        }
        #endregion

        #region 手动填写 Hand
        /// <summary>
        /// 手动填写
        /// </summary>
        [Label("手动填写")]
        public static readonly Property<bool> HandProperty = P<AndonManageCallMaterial>.Register(e => e.Hand);

        /// <summary>
        /// 手动填写
        /// </summary>
        public bool Hand
        {
            get { return this.GetProperty(HandProperty); }
            set { this.SetProperty(HandProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 安灯管理叫料子表实体配置
    /// </summary>
    public class AndonManageCallMaterualConfig : EntityConfig<AndonManageCallMaterial>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDON_CALLMAT").MapAllProperties();
            Meta.Property(AndonManageCallMaterial.FactoryIdProperty).DontMapColumn();
            Meta.Property(AndonManageCallMaterial.WipIdProperty).DontMapColumn();
            Meta.Property(AndonManageCallMaterial.HandProperty).DontMapColumn();
            Meta.Property(AndonManageCallMaterial.WareHouseNameProperty).DontMapColumn();
            Meta.Property(AndonManageCallMaterial.LocationNameProperty).DontMapColumn();
            Meta.Property(AndonManageCallMaterial.WorkOrderIdProperty).DontMapColumn();
            Meta.Property(AndonManageCallMaterial.ProcessIdProperty).DontMapColumn();
            Meta.Property(AndonManageCallMaterial.WorkShopIdProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
