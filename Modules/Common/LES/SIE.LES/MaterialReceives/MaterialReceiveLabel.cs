using SIE.Domain;
using SIE.Core.Items;
using SIE.ObjectModel;
using System;
using SIE.MetaModel;

namespace SIE.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收标签明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料接收标签明细")]
    public class MaterialReceiveLabel : DataEntity
    {
        #region 物料接收 MaterialReceive
        /// <summary>
        /// 物料接收Id
        /// </summary>
        [Label("物料接收")]
        public static readonly IRefIdProperty MaterialReceiveIdProperty =
            P<MaterialReceiveLabel>.RegisterRefId(e => e.MaterialReceiveId, ReferenceType.Parent);

        /// <summary>
        /// 物料接收Id
        /// </summary>
        public double? MaterialReceiveId
        {
            get { return (double?)this.GetRefNullableId(MaterialReceiveIdProperty); }
            set { this.SetRefNullableId(MaterialReceiveIdProperty, value); }
        }

        /// <summary>
        /// 物料接收
        /// </summary>
        public static readonly RefEntityProperty<MaterialReceive> MaterialReceiveProperty =
            P<MaterialReceiveLabel>.RegisterRef(e => e.MaterialReceive, MaterialReceiveIdProperty);

        /// <summary>
        /// 物料接收
        /// </summary>
        public MaterialReceive MaterialReceive
        {
            get { return this.GetRefEntity(MaterialReceiveProperty); }
            set { this.SetRefEntity(MaterialReceiveProperty, value); }
        }
        #endregion

        #region 物料接收 MaterialReceiveDetail
        /// <summary>
        /// 物料接收Id
        /// </summary>
        [Label("物料接收")]
        public static readonly IRefIdProperty MaterialReceiveDetailIdProperty =
            P<MaterialReceiveLabel>.RegisterRefId(e => e.MaterialReceiveDetailId, ReferenceType.Normal);

        /// <summary>
        /// 物料接收Id
        /// </summary>
        public double? MaterialReceiveDetailId
        {
            get { return (double?)this.GetRefNullableId(MaterialReceiveDetailIdProperty); }
            set { this.SetRefNullableId(MaterialReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 物料接收
        /// </summary>
        public static readonly RefEntityProperty<MaterialReceiveDetail> MaterialReceiveDetailProperty =
            P<MaterialReceiveLabel>.RegisterRef(e => e.MaterialReceiveDetail, MaterialReceiveDetailIdProperty);

        /// <summary>
        /// 物料接收
        /// </summary>
        public MaterialReceiveDetail MaterialReceiveDetail
        {
            get { return this.GetRefEntity(MaterialReceiveDetailProperty); }
            set { this.SetRefEntity(MaterialReceiveDetailProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReceiveState?> StateProperty = P<MaterialReceiveLabel>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ReceiveState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 发运单行号 SoLineNo
        /// <summary>
        /// 发运单行号
        /// </summary>
        [Label("发运单行号")]
        public static readonly Property<string> SoLineNoProperty = P<MaterialReceiveLabel>.Register(e => e.SoLineNo);

        /// <summary>
        /// 发运单行号
        /// </summary>
        public string SoLineNo
        {
            get { return this.GetProperty(SoLineNoProperty); }
            set { this.SetProperty(SoLineNoProperty, value); }
        }
        #endregion

        #region 标签号 LabelNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelNoProperty = P<MaterialReceiveLabel>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<MaterialReceiveLabel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<MaterialReceiveLabel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialReceiveLabel>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<MaterialReceiveLabel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性名称 ItemExtPropName
        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        [Label("物料扩展属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MaterialReceiveLabel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<MaterialReceiveLabel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<MaterialReceiveLabel>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<MaterialReceiveLabel>.Register(e => e.ItemUnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
            set { this.SetProperty(ItemUnitNameProperty, value); }
        }
        #endregion

        #region 是否序列号 IsSerialNumber
        /// <summary>
        /// 是否序列号
        /// </summary>
        [Label("是否序列号")]
        public static readonly Property<bool> IsSerialNumberProperty = P<MaterialReceiveLabel>.Register(e => e.IsSerialNumber);

        /// <summary>
        /// 是否序列号
        /// </summary>
        public bool IsSerialNumber
        {
            get { return this.GetProperty(IsSerialNumberProperty); }
            set { this.SetProperty(IsSerialNumberProperty, value); }
        }
        #endregion

        #region 是否合并拣货 IsMerge
        /// <summary>
        /// 是否合并拣货
        /// </summary>
        [Label("是否合并拣货")]
        public static readonly Property<bool> IsMergeProperty = P<MaterialReceiveLabel>.Register(e => e.IsMerge);

        /// <summary>
        /// 是否合并拣货
        /// </summary>
        public bool IsMerge
        {
            get { return this.GetProperty(IsMergeProperty); }
            set { this.SetProperty(IsMergeProperty, value); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<MaterialReceiveLabel>.Register(e => e.LotCode);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 发料数 IssuedQty
        /// <summary>
        /// 发料数
        /// </summary>
        [Label("发料数")]
        public static readonly Property<decimal> IssuedQtyProperty = P<MaterialReceiveLabel>.Register(e => e.IssuedQty);

        /// <summary>
        /// 发料数
        /// </summary>
        public decimal IssuedQty
        {
            get { return this.GetProperty(IssuedQtyProperty); }
            set { this.SetProperty(IssuedQtyProperty, value); }
        }
        #endregion

        #region 接收数 ReceivedQty
        /// <summary>
        /// 接收数
        /// </summary>
        [Label("接收数")]
        public static readonly Property<decimal> ReceivedQtyProperty = P<MaterialReceiveLabel>.Register(e => e.ReceivedQty);

        /// <summary>
        /// 接收数
        /// </summary>
        public decimal ReceivedQty
        {
            get { return this.GetProperty(ReceivedQtyProperty); }
            set { this.SetProperty(ReceivedQtyProperty, value); }
        }
        #endregion

        #region 接收方式 ReceiveType
        /// <summary>
        /// 接收方式
        /// </summary>
        [Label("接收方式")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<MaterialReceiveLabel>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收方式
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return this.GetProperty(ReceiveTypeProperty); }
            set { this.SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 来源单号 SourceNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceNoProperty = P<MaterialReceiveLabel>.RegisterView(e => e.SourceNo, p => p.MaterialReceive.SourceNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
        }

        #endregion

        #region 备料单号 MaterialPreparationNo
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> MaterialPreparationNoProperty = P<MaterialReceiveLabel>.RegisterView(e => e.MaterialPreparationNo, p => p.MaterialReceive.MaterialPreparation.No);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string MaterialPreparationNo
        {
            get { return this.GetProperty(MaterialPreparationNoProperty); }
        }

        #endregion

        #region 发运单号 SoNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> SoNoProperty = P<MaterialReceiveLabel>.RegisterView(e => e.SoNo, p => p.MaterialReceive.SoNo);

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo
        {
            get { return this.GetProperty(SoNoProperty); }
        }

        #endregion

        #region 接收库仓库Id ReceiveWarehouseId
        /// <summary>
        /// 接收库仓库Id
        /// </summary>
        [Label("接收库仓库Id")]
        public static readonly Property<double?> ReceiveWarehouseIdProperty = P<MaterialReceiveLabel>.RegisterView(e => e.ReceiveWarehouseId, p => p.MaterialReceive.ReceiveWarehouseId);

        /// <summary>
        /// 接收库仓库Id
        /// </summary>
        public double? ReceiveWarehouseId
        {
            get { return this.GetProperty(ReceiveWarehouseIdProperty); }
        }

        #endregion

        #region 接收仓库编码 ReceiveWarehouseCode
        /// <summary>
        /// 接收仓库编码
        /// </summary>
        [Label("接收仓库编码")]
        public static readonly Property<string> ReceiveWarehouseCodeProperty = P<MaterialReceiveLabel>.RegisterView(e => e.ReceiveWarehouseCode, p => p.MaterialReceive.ReceiveWarehouse.Code);

        /// <summary>
        /// 接收仓库编码
        /// </summary>
        public string ReceiveWarehouseCode
        {
            get { return this.GetProperty(ReceiveWarehouseCodeProperty); }
        }

        #endregion

        #region 接收库位Id ReceiveLocationId
        /// <summary>
        /// 接收库位Id
        /// </summary>
        [Label("接收库位Id")]
        public static readonly Property<double?> ReceiveLocationIdProperty = P<MaterialReceiveLabel>.RegisterView(e => e.ReceiveLocationId, p => p.MaterialReceive.ReceiveLocationId);

        /// <summary>
        /// 接收库位Id
        /// </summary>
        public double? ReceiveLocationId
        {
            get { return this.GetProperty(ReceiveLocationIdProperty); }
        }

        #endregion

        #region 接收仓库编码 ReceiveLocationCode
        /// <summary>
        /// 接收仓库编码
        /// </summary>
        [Label("接收仓库编码")]
        public static readonly Property<string> ReceiveLocationCodeProperty = P<MaterialReceiveLabel>.RegisterView(e => e.ReceiveLocationCode, p => p.MaterialReceive.ReceiveLocation.Code);

        /// <summary>
        /// 接收仓库编码
        /// </summary>
        public string ReceiveLocationCode
        {
            get { return this.GetProperty(ReceiveLocationCodeProperty); }
        }

        #endregion

        #region 工厂Id FactoryId
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂Id")]
        public static readonly Property<double> FactoryIdProperty = P<MaterialReceiveLabel>.RegisterView(e => e.FactoryId, p => p.MaterialReceive.FactoryId ?? p.MaterialReceive.WorkShop.TreePId);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
        }

        #endregion

        #region 车间Id WorkShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double> WorkShopIdProperty = P<MaterialReceiveLabel>.RegisterView(e => e.WorkShopId, p => p.MaterialReceive.WorkShopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return this.GetProperty(WorkShopIdProperty); }
        }

        #endregion


        #endregion

    }


    /// <summary>
    ///  实体配置
    /// </summary>
    internal class MaterialReceiveLabelConfig : EntityConfig<MaterialReceiveLabel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MATERIAL_REC_LABEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
