using SIE.Domain;
using SIE.Items;
using SIE.LES;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单BOM
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单BOM")]
    public partial class WorkOrderBom : SIE.Core.WorkOrders.WorkOrderBom
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static new readonly IRefIdProperty ItemIdProperty = P<WorkOrderBom>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public new double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static new readonly RefEntityProperty<Item> ItemProperty = P<WorkOrderBom>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public new Item Item
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
        public static readonly Property<string> ItemCodeProperty = P<WorkOrderBom>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<WorkOrderBom>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 主料编码 MainMaterial
        /// <summary>
        /// 主料编码Id
        /// </summary>
        [Label("主料编码")]
        public static readonly IRefIdProperty MainMaterialIdProperty =
            P<WorkOrderBom>.RegisterRefId(e => e.MainMaterialId, ReferenceType.Normal);

        /// <summary>
        /// 主料编码Id
        /// </summary>
        public double? MainMaterialId
        {
            get { return (double?)GetRefNullableId(MainMaterialIdProperty); }
            set { SetRefNullableId(MainMaterialIdProperty, value); }
        }

        /// <summary>
        /// 主料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> MainMaterialProperty =
            P<WorkOrderBom>.RegisterRef(e => e.MainMaterial, MainMaterialIdProperty);

        /// <summary>
        /// 主料编码
        /// </summary>
        public Item MainMaterial
        {
            get { return GetRefEntity(MainMaterialProperty); }
            set { SetRefEntity(MainMaterialProperty, value); }
        }
        #endregion

        #region 替代组合分组 AlterGroup
        /// <summary>
        /// 替代组合分组
        /// </summary>
        [Label("替代组合分组")]
        public static readonly Property<string> AlterGroupProperty = P<WorkOrderBom>.Register(e => e.AlterGroup);

        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup
        {
            get { return this.GetProperty(AlterGroupProperty); }
            set { this.SetProperty(AlterGroupProperty, value); }
        }
        #endregion

        #region 替代组 Alter
        /// <summary>
        /// 替代组
        /// </summary>
        [Label("替代组")]
        public static readonly Property<string> AlterProperty = P<WorkOrderBom>.Register(e => e.Alter);

        /// <summary>
        /// 替代组
        /// </summary>
        public string Alter
        {
            get { return this.GetProperty(AlterProperty); }
            set { this.SetProperty(AlterProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<int> PriorityProperty = P<WorkOrderBom>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get { return this.GetProperty(PriorityProperty); }
            set { this.SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 工单与工单BOM关系 WorkOrder
        /// <summary>
        /// 工单与工单BOM关系Id
        /// </summary>
        [Label("工单")]
        public static new readonly IRefIdProperty WorkOrderIdProperty = P<WorkOrderBom>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 工单与工单BOM关系Id
        /// </summary>
        public new double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单与工单BOM关系
        /// </summary>
        [Label("工单")]
        public static new readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WorkOrderBom>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单与工单BOM关系
        /// </summary>
        public new WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 视图属性(关联实体属性平铺显示，一般用于Web)

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ItemType> TypeProperty = P<WorkOrderBom>.RegisterView(e => e.Type, e => e.Item.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public ItemType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty = P<WorkOrderBom>.RegisterView(e => e.SpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return GetProperty(SpecificationModelProperty); }
            set { SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WorkOrderBom>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 工单产品编码 ProductCode
        /// <summary>
        /// 工单产品编码
        /// </summary>
        [Label("工单产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WorkOrderBom>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 工单产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 工单产品名称 ProductName
        /// <summary>
        /// 工单产品名称
        /// </summary>
        [Label("工单产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WorkOrderBom>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 工单产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<WorkOrderBom>.RegisterView(e => e.UnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 物料消耗类型 ItemConsumeMode
        /// <summary>
        /// 物料消耗类型
        /// </summary>
        [Label("物料消耗类型")]
        public static readonly Property<ConsumeMode> ItemConsumeModeProperty = P<WorkOrderBom>.RegisterView(e => e.ItemConsumeMode, p => p.Item.ConsumeMode);

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode ItemConsumeMode
        {
            get { return this.GetProperty(ItemConsumeModeProperty); }
        }
        #endregion

        #region 项目编码 ProjectMaintainCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectMaintainCodeProperty = P<WorkOrderBom>.RegisterView((System.Linq.Expressions.Expression<Func<WorkOrderBom, string>>)(e => (string)e.ProjectMaintainCode), p => p.WorkOrder.ProjectMaintain.Code);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectMaintainCode
        {
            get { return this.GetProperty(ProjectMaintainCodeProperty); }
        }

        #endregion

        #endregion

        #region 预留编号 Rsnum
        /// <summary>
        /// 预留编号
        /// </summary>
        [Label("预留编号")]
        public static readonly Property<string> RsnumProperty = P<WorkOrderBom>.Register(e => e.Rsnum);

        /// <summary>
        /// 预留编号
        /// </summary>
        public string Rsnum
        {
            get { return this.GetProperty(RsnumProperty); }
            set { this.SetProperty(RsnumProperty, value); }
        }
        #endregion

        #region 预留编号序号 Rspos
        /// <summary>
        /// 预留编号序号
        /// </summary>
        [Label("预留编号序号")]
        public static readonly Property<string> RsposProperty = P<WorkOrderBom>.Register(e => e.Rspos);

        /// <summary>
        /// 预留编号序号
        /// </summary>
        public string Rspos
        {
            get { return this.GetProperty(RsposProperty); }
            set { this.SetProperty(RsposProperty, value); }
        }
        #endregion

        #region 行项号 Posnr
        /// <summary>
        /// 行项号
        /// </summary>
        [Label("行项号")]
        public static readonly Property<string> PosnrProperty = P<WorkOrderBom>.Register(e => e.Posnr);

        /// <summary>
        /// 行项号
        /// </summary>
        public string Posnr
        {
            get { return this.GetProperty(PosnrProperty); }
            set { this.SetProperty(PosnrProperty, value); }
        }
        #endregion

        #region 移动类型 Bwart
        /// <summary>
        /// 移动类型
        /// </summary>
        [Label("移动类型")]
        public static readonly Property<string> BwartProperty = P<WorkOrderBom>.Register(e => e.Bwart);

        /// <summary>
        /// 移动类型
        /// </summary>
        public string Bwart
        {
            get { return this.GetProperty(BwartProperty); }
            set { this.SetProperty(BwartProperty, value); }
        }
        #endregion

        #region 提货数 Enmng
        /// <summary>
        /// 提货数
        /// </summary>
        [Label("提货数")]
        public static readonly Property<decimal?> EnmngProperty = P<WorkOrderBom>.Register(e => e.Enmng);

        /// <summary>
        /// 提货数
        /// </summary>
        public decimal? Enmng
        {
            get { return this.GetProperty(EnmngProperty); }
            set { this.SetProperty(EnmngProperty, value); }
        }
        #endregion

        #region 库存地点 Lgort
        /// <summary>
        /// 库存地点
        /// </summary>
        [Label("库存地点")]
        public static readonly Property<string> LgortProperty = P<WorkOrderBom>.Register(e => e.Lgort);

        /// <summary>
        /// 库存地点
        /// </summary>
        public string Lgort
        {
            get { return this.GetProperty(LgortProperty); }
            set { this.SetProperty(LgortProperty, value); }
        }
        #endregion

        #region 发料工厂 Werks
        /// <summary>
        /// 发料工厂
        /// </summary>
        [Label("发料工厂")]
        public static readonly Property<string> WerksProperty = P<WorkOrderBom>.Register(e => e.Werks);

        /// <summary>
        /// 发料工厂
        /// </summary>
        public string Werks
        {
            get { return this.GetProperty(WerksProperty); }
            set { this.SetProperty(WerksProperty, value); }
        }
        #endregion

        #region 单位 Meins
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> MeinsProperty = P<WorkOrderBom>.Register(e => e.Meins);

        /// <summary>
        /// 单位
        /// </summary>
        public string Meins
        {
            get { return this.GetProperty(MeinsProperty); }
            set { this.SetProperty(MeinsProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 工单BOM 实体配置
    /// </summary>
    internal class WorkOrderBomConfig : EntityConfig<WorkOrderBom>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_BOM").MapAllProperties();
            Meta.Property(WorkOrderBom.IsAllowEditProperty).DontMapColumn();
            Meta.Property(WorkOrderBom.WorkOrderIdProperty).ColumnMeta.IgnoreFK();
            Meta.Property(WorkOrderBom.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}