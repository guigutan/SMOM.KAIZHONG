using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 基礎分配信息
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("基础分配信息")]
    [DisplayMember(nameof(BaseAssign.Id))]
    public partial class BaseAssign : DataEntity
    {
        #region LPN Lpn
        /// <summary>
        /// LPN
        /// </summary>
        [Label("LPN")]
        public static readonly Property<string> LpnProperty = P<BaseAssign>.Register(e => e.Lpn);

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn
        {
            get { return GetProperty(LpnProperty); }
            set { SetProperty(LpnProperty, value); }
        }
        #endregion

        #region 分配数 AssignQty
        /// <summary>
        /// 分配数
        /// </summary>
        [Label("分配数")]
        public static readonly Property<decimal> AssignQtyProperty = P<BaseAssign>.Register(e => e.AssignQty);

        /// <summary>
        /// 分配数
        /// </summary>
        public decimal AssignQty
        {
            get { return GetProperty(AssignQtyProperty); }
            set { SetProperty(AssignQtyProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<BaseAssign>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> StorerCodeProperty = P<BaseAssign>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return this.GetProperty(StorerCodeProperty); }
            set { this.SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 任务号 TaskNo
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> TaskNoProperty = P<BaseAssign>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
            set { this.SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 库存状态 OnhandState
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState> OnhandStateProperty = P<BaseAssign>.Register(e => e.OnhandState);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState OnhandState
        {
            get { return this.GetProperty(OnhandStateProperty); }
            set { this.SetProperty(OnhandStateProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<BaseAssign>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<BaseAssign>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 源库位 SourceStorageLocation
        /// <summary>
        /// 源库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty SourceStorageLocationIdProperty = P<BaseAssign>.RegisterRefId(e => e.SourceStorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 源库位Id
        /// </summary>
        public double SourceStorageLocationId
        {
            get { return (double)GetRefId(SourceStorageLocationIdProperty); }
            set { SetRefId(SourceStorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 源库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> SourceStorageLocationProperty = P<BaseAssign>.RegisterRef(e => e.SourceStorageLocation, SourceStorageLocationIdProperty);

        /// <summary>
        /// 源库位
        /// </summary>
        public StorageLocation SourceStorageLocation
        {
            get { return GetRefEntity(SourceStorageLocationProperty); }
            set { SetRefEntity(SourceStorageLocationProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次Id
        /// </summary>
        [Label("批次")]
        public static readonly IRefIdProperty LotIdProperty = P<BaseAssign>.RegisterRefId(e => e.LotId, ReferenceType.Normal);

        /// <summary>
        /// 批次Id
        /// </summary>
        public double LotId
        {
            get { return (double)GetRefId(LotIdProperty); }
            set { SetRefId(LotIdProperty, value); }
        }

        /// <summary>
        /// 批次
        /// </summary>
        public static readonly RefEntityProperty<Lot> LotProperty = P<BaseAssign>.RegisterRef(e => e.Lot, LotIdProperty);

        /// <summary>
        /// 批次
        /// </summary>
        public Lot Lot
        {
            get { return GetRefEntity(LotProperty); }
            set { SetRefEntity(LotProperty, value); }
        }
        #endregion

        #region 批次和LPN库存 LotLpnOnhand
        /// <summary>
        /// 批次和LPN库存
        /// </summary>
        [Label("分配库存")]
        public static readonly IRefIdProperty LotLpnOnhandIdProperty = P<BaseAssign>.RegisterRefId(e => e.LotLpnOnhandId, ReferenceType.Normal);

        /// <summary>
        /// 批次和LPN库存
        /// </summary>
        public double LotLpnOnhandId
        {
            get { return (double)GetRefId(LotLpnOnhandIdProperty); }
            set { SetRefId(LotLpnOnhandIdProperty, value); }
        }

        /// <summary>
        /// 批次和LPN库存
        /// </summary>
        public static readonly RefEntityProperty<LotLpnOnhand> LotLpnOnhandProperty = P<BaseAssign>.RegisterRef(e => e.LotLpnOnhand, LotLpnOnhandIdProperty);

        /// <summary>
        /// 批次和LPN库存
        /// </summary>
        public LotLpnOnhand LotLpnOnhand
        {
            get { return GetRefEntity(LotLpnOnhandProperty); }
            set { SetRefEntity(LotLpnOnhandProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(500)]
        public static readonly Property<string> ItemExtPropNameProperty = P<BaseAssign>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        [MaxLength(120)]
        public static readonly Property<string> ItemExtPropProperty = P<BaseAssign>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料体积 ItemVolume
        /// <summary>
        /// 物料体积
        /// </summary>
        [Label("物料体积")]
        public static readonly Property<decimal?> ItemVolumeProperty = P<BaseAssign>.RegisterView(e => e.ItemVolume, p => p.Item.Volume);

        /// <summary>
        /// 物料体积
        /// </summary>
        public decimal? ItemVolume
        {
            get { return this.GetProperty(ItemVolumeProperty); }
        }
        #endregion

        #region 物料净重 ItemWeight
        /// <summary>
        /// 物料净重
        /// </summary>
        [Label("物料净重")]
        public static readonly Property<decimal?> ItemWeightProperty = P<BaseAssign>.RegisterView(e => e.ItemWeight, p => p.Item.Weight);

        /// <summary>
        /// 物料净重
        /// </summary>
        public decimal? ItemWeight
        {
            get { return this.GetProperty(ItemWeightProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BaseAssign>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<BaseAssign>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料规格 ItemSpecificationModel
        /// <summary>
        /// 物料规格
        /// </summary>
        [Label("物料规格")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<BaseAssign>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 物料规格
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<BaseAssign>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }
        #endregion

        #region 库位编码 SourceStorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> SourceStorageLocationCodeProperty = P<BaseAssign>.RegisterView(e => e.SourceStorageLocationCode, p => p.SourceStorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string SourceStorageLocationCode
        {
            get { return this.GetProperty(SourceStorageLocationCodeProperty); }
        }
        #endregion

        #region 来源库位 SourceStorageLocationName
        /// <summary>
        /// 来源库位
        /// </summary>
        [Label("来源库位")]
        public static readonly Property<string> SourceStorageLocationNameProperty = P<BaseAssign>.RegisterView(e => e.SourceStorageLocationName, p => p.SourceStorageLocation.Name);

        /// <summary>
        /// 来源库位
        /// </summary>
        public string SourceStorageLocationName
        {
            get { return this.GetProperty(SourceStorageLocationNameProperty); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        public static readonly Property<string> LotCodeProperty = P<BaseAssign>.RegisterView(e => e.LotCode, p => p.Lot.Code);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
        }
        #endregion

        #region 库存物料扩展属性 LotLpnOnhandItemExtProp
        /// <summary>
        /// 库存物料扩展属性
        /// </summary>
        [Label("库存物料扩展属性")]
        public static readonly Property<string> LotLpnOnhandItemExtPropProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandItemExtProp, p => p.LotLpnOnhand.ItemExtProp);

        /// <summary>
        /// 库存物料扩展属性
        /// </summary>
        public string LotLpnOnhandItemExtProp
        {
            get { return this.GetProperty(LotLpnOnhandItemExtPropProperty); }
        }
        #endregion

        #region 库存物料扩展属性名称 LotLpnOnhandItemExtPropName
        /// <summary>
        /// 库存物料扩展属性
        /// </summary>
        [Label("分配物料扩展属性")]
        public static readonly Property<string> LotLpnOnhandItemExtPropNameProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandItemExtPropName, p => p.LotLpnOnhand.ItemExtPropName);

        /// <summary>
        /// 库存物料扩展属性
        /// </summary>
        public string LotLpnOnhandItemExtPropName
        {
            get { return this.GetProperty(LotLpnOnhandItemExtPropNameProperty); }
        }
        #endregion

        #region 现有量 LotLpnOnhandQty
        /// <summary>
        /// 现有量
        /// </summary>
        [Label("现有量")]
        public static readonly Property<decimal> LotLpnOnhandQtyProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandQty, p => p.LotLpnOnhand.Qty);

        /// <summary>
        /// 现有量
        /// </summary>
        public decimal LotLpnOnhandQty
        {
            get { return this.GetProperty(LotLpnOnhandQtyProperty); }
        }
        #endregion

        #region 可用数量 LotLpnOnhandAvailableQty
        /// <summary>
        /// 可用数量
        /// </summary>
        [Label("可用数量")]
        public static readonly Property<decimal> LotLpnOnhandAvailableQtyProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandAvailableQty, p => p.LotLpnOnhand.AvailableQty);

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal LotLpnOnhandAvailableQty
        {
            get { return this.GetProperty(LotLpnOnhandAvailableQtyProperty); }
        }
        #endregion

        #region 货主 LotLpnOnhandStorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> LotLpnOnhandStorerCodeProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandStorerCode, p => p.LotLpnOnhand.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string LotLpnOnhandStorerCode
        {
            get { return this.GetProperty(LotLpnOnhandStorerCodeProperty); }
            set { this.SetProperty(LotLpnOnhandStorerCodeProperty, value); }
        }
        #endregion

        #region 库存项目号 LotLpnOnhandProjectNo
        /// <summary>
        /// 库存项目号
        /// </summary>
        [Label("库存项目号")]
        public static readonly Property<string> LotLpnOnhandProjectNoProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandProjectNo, p => p.LotLpnOnhand.ProjectNo);

        /// <summary>
        /// 库存项目号
        /// </summary>
        public string LotLpnOnhandProjectNo
        {
            get { return this.GetProperty(LotLpnOnhandProjectNoProperty); }
            set { this.SetProperty(LotLpnOnhandProjectNoProperty, value); }
        }
        #endregion

        #region 任务号 LotLpnOnhandTaskNo
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> LotLpnOnhandTaskNoProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandTaskNo, p => p.LotLpnOnhand.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string LotLpnOnhandTaskNo
        {
            get { return this.GetProperty(LotLpnOnhandTaskNoProperty); }
            set { this.SetProperty(LotLpnOnhandTaskNoProperty, value); }
        }
        #endregion

        #region 库存状态 LotLpnOnhandState
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState> LotLpnOnhandStateProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandState, p => p.LotLpnOnhand.State);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState LotLpnOnhandState
        {
            get { return this.GetProperty(LotLpnOnhandStateProperty); }
            set { this.SetProperty(LotLpnOnhandStateProperty, value); }
        }
        #endregion

        #region 分配库位名称 LotLpnOnhandStorageLocation
        /// <summary>
        /// 分配库位
        /// </summary>
        [Label("分配库位")]
        public static readonly Property<string> LotLpnOnhandStorageLocationProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandStorageLocation, p => p.LotLpnOnhand.StorageLocation.Name);

        /// <summary>
        /// 分配库位
        /// </summary>
        public string LotLpnOnhandStorageLocation
        {
            get { return this.GetProperty(LotLpnOnhandStorageLocationProperty); }
        }
        #endregion

        #region 分配库位Id LotLpnOnhandStorageLocationId
        /// <summary>
        /// 分配库位Id
        /// </summary>
        [Label("分配库位Id")]
        public static readonly Property<double> LotLpnOnhandStorageLocationIdProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandStorageLocationId, p => p.LotLpnOnhand.StorageLocationId);

        /// <summary>
        /// 分配库位Id
        /// </summary>
        public double LotLpnOnhandStorageLocationId
        {
            get { return this.GetProperty(LotLpnOnhandStorageLocationIdProperty); }
        }
        #endregion

        #region 分配库位编码 LotLpnOnhandStorageLocationCode
        /// <summary>
        /// 分配库位
        /// </summary>
        [Label("分配库位编码")]
        public static readonly Property<string> LotLpnOnhandStorageLocationCodeProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandStorageLocationCode, p => p.LotLpnOnhand.StorageLocation.Code);

        /// <summary>
        /// 分配库位
        /// </summary>
        public string LotLpnOnhandStorageLocationCode
        {
            get { return this.GetProperty(LotLpnOnhandStorageLocationCodeProperty); }
        }
        #endregion

        #region 分配LPN LotLpnOnhandLPN
        /// <summary>
        /// 分配LPN
        /// </summary>
        [Label("分配LPN")]
        public static readonly Property<string> LotLpnOnhandLPNProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandLPN, p => p.LotLpnOnhand.Lpn);

        /// <summary>
        /// 分配LPN
        /// </summary>
        public string LotLpnOnhandLPN
        {
            get { return this.GetProperty(LotLpnOnhandLPNProperty); }
        }
        #endregion

        #region 分配批次 LotLpnOnhandLotCode
        /// <summary>
        /// 分配批次
        /// </summary>
        [Label("分配批次")]
        public static readonly Property<string> LotLpnOnhandLotCodeProperty = P<BaseAssign>.RegisterView(e => e.LotLpnOnhandLotCode, p => p.LotLpnOnhand.LotCode);

        /// <summary>
        /// 分配批次
        /// </summary>
        public string LotLpnOnhandLotCode
        {
            get { return this.GetProperty(LotLpnOnhandLotCodeProperty); }
        }
        #endregion

        #region 物料启用扩展属性 ItemEnableExtendProperty
        /// <summary>
        /// 物料启用扩展属性
        /// </summary>
        public static readonly Property<bool> ItemEnableExtendPropertyProperty = P<BaseAssign>.RegisterView(e => e.ItemEnableExtendProperty, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 物料名称
        /// </summary>
        public bool ItemEnableExtendProperty
        {
            get { return this.GetProperty(ItemEnableExtendPropertyProperty); }
        }
        #endregion

        #region 库区ID StorageAreaId
        /// <summary>
        /// 库区ID
        /// </summary>
        [Label("库区ID")]
        public static readonly Property<double> StorageAreaIdProperty = P<BaseAssign>.RegisterView(e => e.StorageAreaId, p => p.SourceStorageLocation.AreaId);

        /// <summary>
        /// 库区ID
        /// </summary>
        public double StorageAreaId
        {
            get { return this.GetProperty(StorageAreaIdProperty); }
        }
        #endregion

        #endregion

        #region 是否正在取消分配 IsUnconfirmAssigning
        /// <summary>
        /// 是否拣货
        /// </summary>
        [Label("正在关闭")]
        public static readonly Property<bool> IsUnconfirmAssigningProperty = P<BaseAssign>.Register(e => e.IsUnconfirmAssigning);

        /// <summary>
        /// 正在关闭
        /// </summary>
        public bool IsUnconfirmAssigning
        {
            get { return GetProperty(IsUnconfirmAssigningProperty); }
            set { SetProperty(IsUnconfirmAssigningProperty, value); }
        }
        #endregion

    }
}