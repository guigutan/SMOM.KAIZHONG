using SIE.Domain;
using SIE.Items;
using SIE.LES.MaterialPreparations.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单明细基础数据
    /// </summary>
    [ChildEntity, Serializable]
    [Label("备料需求单明细基础数据")]
    public class MaterialPreparationDetailBase : DataEntity
    {
        #region 备料需求单 MaterialPreparation
        /// <summary>
        /// 备料需求单Id
        /// </summary>
        [Label("备料需求单")]
        public static readonly IRefIdProperty MaterialPreparationIdProperty =
            P<MaterialPreparationDetailBase>.RegisterRefId(e => e.MaterialPreparationId, ReferenceType.Parent);

        /// <summary>
        /// 备料需求单Id
        /// </summary>
        public double MaterialPreparationId
        {
            get { return (double)this.GetRefId(MaterialPreparationIdProperty); }
            set { this.SetRefId(MaterialPreparationIdProperty, value); }
        }

        /// <summary>
        /// 备料需求单
        /// </summary>
        public static readonly RefEntityProperty<MaterialPreparation> MaterialPreparationProperty =
            P<MaterialPreparationDetailBase>.RegisterRef(e => e.MaterialPreparation, MaterialPreparationIdProperty);

        /// <summary>
        /// 备料需求单
        /// </summary>
        public MaterialPreparation MaterialPreparation
        {
            get { return this.GetRefEntity(MaterialPreparationProperty); }
            set { this.SetRefEntity(MaterialPreparationProperty, value); }
        }
        #endregion

        #region 备料状态 PreDetailStatus
        /// <summary>
        /// 备料状态
        /// </summary>
        [Label("备料状态")]
        public static readonly Property<PrepareDetailStatus> PreDetailStatusProperty = P<MaterialPreparationDetailBase>.Register(e => e.PreDetailStatus);

        /// <summary>
        /// 备料状态
        /// </summary>
        public PrepareDetailStatus PreDetailStatus
        {
            get { return this.GetProperty(PreDetailStatusProperty); }
            set { this.SetProperty(PreDetailStatusProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<MaterialPreparationDetailBase>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<MaterialPreparationDetailBase>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<MaterialPreparationDetailBase>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料消耗方式 ItemConsumeMode
        /// <summary>
        /// 物料消耗方式
        /// </summary>
        [Label("物料消耗方式")]
        public static readonly Property<ConsumeMode> ItemConsumeModeProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.ItemConsumeMode, p => p.Item.ConsumeMode);

        /// <summary>
        /// 物料消耗方式
        /// </summary>
        public ConsumeMode ItemConsumeMode
        {
            get { return this.GetProperty(ItemConsumeModeProperty); }
            set { this.SetProperty(ItemConsumeModeProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.UnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 是否启用扩展属性 EnableExtendProperty
        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        [Label("是否启用扩展属性")]
        public static readonly Property<bool> EnableExtendPropertyProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.EnableExtendProperty, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        public bool EnableExtendProperty
        {
            get { return this.GetProperty(EnableExtendPropertyProperty); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<MaterialPreparationDetailBase>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性值 ItemExtPropName
        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        [Label("物料扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MaterialPreparationDetailBase>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 本次备料量 Qty
        /// <summary>
        /// 本次备料量
        /// </summary>
        [Label("本次备料量")]
        public static readonly Property<decimal> QtyProperty = P<MaterialPreparationDetailBase>.Register(e => e.Qty);

        /// <summary>
        /// 本次备料量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 接收数 ReceiveQty
        /// <summary>
        /// 接收数
        /// </summary>
        [Label("接收数")]
        public static readonly Property<decimal> ReceiveQtyProperty = P<MaterialPreparationDetailBase>.Register(e => e.ReceiveQty);

        /// <summary>
        /// 接收数
        /// </summary>
        public decimal ReceiveQty
        {
            get { return this.GetProperty(ReceiveQtyProperty); }
            set { this.SetProperty(ReceiveQtyProperty, value); }
        }
        #endregion

        #region 拒收数 RefuseQty
        /// <summary>
        /// 拒收数
        /// </summary>
        [Label("拒收数")]
        public static readonly Property<decimal> RefuseQtyProperty = P<MaterialPreparationDetailBase>.Register(e => e.RefuseQty);

        /// <summary>
        /// 拒收数
        /// </summary>
        public decimal RefuseQty
        {
            get { return this.GetProperty(RefuseQtyProperty); }
            set { this.SetProperty(RefuseQtyProperty, value); }
        }
        #endregion

        #region 发料数 ShippingQty
        /// <summary>
        /// 发料数
        /// </summary>
        [Label("发料数")]
        public static readonly Property<decimal> ShippingQtyProperty = P<MaterialPreparationDetailBase>.Register(e => e.ShippingQty);

        /// <summary>
        /// 发料数
        /// </summary>
        public decimal ShippingQty
        {
            get { return this.GetProperty(ShippingQtyProperty); }
            set { this.SetProperty(ShippingQtyProperty, value); }
        }
        #endregion

        #region 待收数 ToReceiveQty
        /// <summary>
        /// 待收数
        /// </summary>
        [Label("待收数")]
        public static readonly Property<decimal> ToReceiveQtyProperty = P<MaterialPreparationDetailBase>.RegisterReadOnly(
            e => e.ToReceiveQty, e => e.GetToReceiveQty(), ShippingQtyProperty, ReceiveQtyProperty, RefuseQtyProperty);
        /// <summary>
        /// 待收数
        /// </summary>

        public decimal ToReceiveQty
        {
            get { return this.GetProperty(ToReceiveQtyProperty); }
        }
        private decimal GetToReceiveQty()
        {
            return (ShippingQty - ReceiveQty - RefuseQty);
        }
        #endregion

        #region 需求量 BomNeedQty
        /// <summary>
        /// 需求量
        /// </summary>
        [Label("需求量")]
        public static readonly Property<decimal> BomNeedQtyProperty = P<MaterialPreparationDetailBase>.Register(e => e.BomNeedQty);

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal BomNeedQty
        {
            get { return this.GetProperty(BomNeedQtyProperty); }
            set { this.SetProperty(BomNeedQtyProperty, value); }
        }
        #endregion

        #region 可备料数 CanPrepareQty
        /// <summary>
        /// 可备料数
        /// </summary>
        [Label("可备料数")]
        public static readonly Property<decimal> CanPrepareQtyProperty = P<MaterialPreparationDetailBase>.Register(e => e.CanPrepareQty);

        /// <summary>
        /// 可备料数
        /// </summary>
        public decimal CanPrepareQty
        {
            get { return this.GetProperty(CanPrepareQtyProperty); }
            set { this.SetProperty(CanPrepareQtyProperty, value); }
        }
        #endregion

        #region 取消数 CancelQty
        /// <summary>
        /// 取消数
        /// </summary>
        [Label("取消数")]
        public static readonly Property<decimal> CancelQtyProperty = P<MaterialPreparationDetailBase>.Register(e => e.CancelQty);

        /// <summary>
        /// 取消数
        /// </summary>
        public decimal CancelQty
        {
            get { return this.GetProperty(CancelQtyProperty); }
            set { this.SetProperty(CancelQtyProperty, value); }
        }
        #endregion

        #region 工单Id MpWoId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double?> MpWoIdProperty = P<MaterialPreparationDetailBase>.Register(e => e.MpWoId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? MpWoId
        {
            get { return this.GetProperty(MpWoIdProperty); }
            set { this.SetProperty(MpWoIdProperty, value); }
        }
        #endregion

        #region 工单号 MpWo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> MpWoProperty = P<MaterialPreparationDetailBase>.Register(e => e.MpWo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string MpWo
        {
            get { return this.GetProperty(MpWoProperty); }
            set { this.SetProperty(MpWoProperty, value); }
        }
        #endregion

        #region 发运单号 ShippingDetailNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> ShippingDetailNoProperty = P<MaterialPreparationDetailBase>.Register(e => e.ShippingDetailNo);

        /// <summary>
        /// 发运单号
        /// </summary>
        public string ShippingDetailNo
        {
            get { return this.GetProperty(ShippingDetailNoProperty); }
            set { this.SetProperty(ShippingDetailNoProperty, value); }
        }
        #endregion

        #region 主视图
        #region 车间 WorkShopCode
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopCodeProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.WorkShopCode, p => p.MaterialPreparation.WorkShop.Code);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
        }
        #endregion

        #region 生产资源 WipResourceCode
        /// <summary>
        /// 生产资源
        /// </summary>
        [Label("生产资源")]
        public static readonly Property<string> WipResourceCodeProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.WipResourceCode, p => p.MaterialPreparation.Resource.Code);

        /// <summary>
        /// 生产资源
        /// </summary>
        public string WipResourceCode
        {
            get { return this.GetProperty(WipResourceCodeProperty); }
        }
        #endregion

        #region 备料需求单状态 MpType
        /// <summary>
        /// 备料需求单状态
        /// </summary>
        [Label("备料需求单状态")]
        public static readonly Property<PrepareType> MpTypeProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.MpType, p => p.MaterialPreparation.PrepareType);

        /// <summary>
        /// 备料需求单状态
        /// </summary>
        public PrepareType MpType
        {
            get { return this.GetProperty(MpTypeProperty); }
        }
        #endregion

        #region 备料需求单号 MpNo
        /// <summary>
        /// 备料需求单号
        /// </summary>
        [Label("备料需求单号")]
        public static readonly Property<string> MpNoProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.MpNo, p => p.MaterialPreparation.No);

        /// <summary>
        /// 备料需求单号
        /// </summary>
        public string MpNo
        {
            get { return this.GetProperty(MpNoProperty); }
        }
        #endregion

        #region 创单时间 MpCreateDate
        /// <summary>
        /// 创单时间
        /// </summary>
        [Label("创单时间")]
        public static readonly Property<DateTime> MpCreateDateProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.MpCreateDate, p => p.MaterialPreparation.CreateDate);

        /// <summary>
        /// 创单时间
        /// </summary>
        public DateTime MpCreateDate
        {
            get { return this.GetProperty(MpCreateDateProperty); }
        }
        #endregion

        #region 发货仓库编码 WarehouseCode
        /// <summary>
        /// 发货仓库编码
        /// </summary>
        [Label("发货仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.WarehouseCode, p => p.MaterialPreparation.Warehouse.Code);

        /// <summary>
        /// 发货仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 发货仓库 WarehouseName
        /// <summary>
        /// 发货仓库
        /// </summary>
        [Label("发货仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.WarehouseName, p => p.MaterialPreparation.Warehouse.Name);

        /// <summary>
        /// 发货仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 项目号 ProjectMaintainCode
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectMaintainCodeProperty = P<MaterialPreparationDetailBase>.RegisterView(e => e.ProjectMaintainCode, p => p.MaterialPreparation.ProjectMaintain.Code);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectMaintainCode
        {
            get { return this.GetProperty(ProjectMaintainCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class MaterialPreparationDetailBaseConfig : EntityConfig<MaterialPreparationDetailBase>
    {
        /// <summary>
        /// 表配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MATERIAL_PREDTL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(MaterialPreparationDetailBase.ToReceiveQtyProperty).DontMapColumn();
            Meta.Property(MaterialPreparationDetailBase.CanPrepareQtyProperty).DontMapColumn();
        }
    }
}
