using SIE.Domain;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.LES.MaterialReturnApplys.Enums;
using SIE.LES.Reports;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using System;

namespace SIE.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("退料申请明细")]
    public class MaterialReturnApplyDetailBase : DataEntity
    {
        #region 退料申请 MaterialReturnApply
        /// <summary>
        /// 退料申请Id
        /// </summary>
        [Label("属性名")]
        public static readonly IRefIdProperty MaterialReturnApplyIdProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRefId(e => e.MaterialReturnApplyId, ReferenceType.Parent);

        /// <summary>
        /// 退料申请Id
        /// </summary>
        public double MaterialReturnApplyId
        {
            get { return (double)this.GetRefId(MaterialReturnApplyIdProperty); }
            set { this.SetRefId(MaterialReturnApplyIdProperty, value); }
        }

        /// <summary>
        /// 退料申请
        /// </summary>
        public static readonly RefEntityProperty<MaterialReturnApply> MaterialReturnApplyProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRef(e => e.MaterialReturnApply, MaterialReturnApplyIdProperty);

        /// <summary>
        /// 退料申请
        /// </summary>
        public MaterialReturnApply MaterialReturnApply
        {
            get { return this.GetRefEntity(MaterialReturnApplyProperty); }
            set { this.SetRefEntity(MaterialReturnApplyProperty, value); }
        }
        #endregion

        #region 工单需求 WoDemandReport
        /// <summary>
        /// 工单需求Id
        /// </summary>
        [Label("工单需求")]
        public static readonly IRefIdProperty WoDemandReportIdProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRefId(e => e.WoDemandReportId, ReferenceType.Normal);

        /// <summary>
        /// 工单需求Id
        /// </summary>
        public double? WoDemandReportId
        {
            get { return (double?)this.GetRefNullableId(WoDemandReportIdProperty); }
            set { this.SetRefNullableId(WoDemandReportIdProperty, value); }
        }

        /// <summary>
        /// 工单需求
        /// </summary>
        public static readonly RefEntityProperty<WoDemandReport> WoDemandReportProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRef(e => e.WoDemandReport, WoDemandReportIdProperty);

        /// <summary>
        /// 工单需求
        /// </summary>
        public WoDemandReport WoDemandReport
        {
            get { return this.GetRefEntity(WoDemandReportProperty); }
            set { this.SetRefEntity(WoDemandReportProperty, value); }
        }
        #endregion

        #region 批次Lpn Lpn
        /// <summary>
        /// 批次LpnId
        /// </summary>
        [Label("批次Lpn")]
        public static readonly IRefIdProperty LpnIdProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRefId(e => e.LpnId, ReferenceType.Normal);

        /// <summary>
        /// 批次LpnId
        /// </summary>
        public double? LpnId
        {
            get { return (double?)this.GetRefNullableId(LpnIdProperty); }
            set { this.SetRefNullableId(LpnIdProperty, value); }
        }

        /// <summary>
        /// 批次Lpn
        /// </summary>
        public static readonly RefEntityProperty<LotLpnOnhand> LpnProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRef(e => e.Lpn, LpnIdProperty);

        /// <summary>
        /// 批次Lpn
        /// </summary>
        public LotLpnOnhand Lpn
        {
            get { return this.GetRefEntity(LpnProperty); }
            set { this.SetRefEntity(LpnProperty, value); }
        }
        #endregion

        #region 不良批次Lpn NgLpn
        /// <summary>
        /// 不良批次LpnId
        /// </summary>
        [Label("不良批次Lpn")]
        public static readonly IRefIdProperty NgLpnIdProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRefId(e => e.NgLpnId, ReferenceType.Normal);

        /// <summary>
        /// 不良批次LpnId
        /// </summary>
        public double? NgLpnId
        {
            get { return (double?)this.GetRefNullableId(NgLpnIdProperty); }
            set { this.SetRefNullableId(NgLpnIdProperty, value); }
        }

        /// <summary>
        /// 不良批次Lpn
        /// </summary>
        public static readonly RefEntityProperty<LotLpnOnhand> NgLpnProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRef(e => e.NgLpn, NgLpnIdProperty);

        /// <summary>
        /// 不良批次Lpn
        /// </summary>
        public LotLpnOnhand NgLpn
        {
            get { return this.GetRefEntity(NgLpnProperty); }
            set { this.SetRefEntity(NgLpnProperty, value); }
        }
        #endregion

        #region 退料状态 ReDetailStatus
        /// <summary>
        /// 退料状态
        /// </summary>
        [Label("退料状态")]
        public static readonly Property<ReDetailStatus> ReDetailStatusProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.ReDetailStatus);

        /// <summary>
        /// 退料状态
        /// </summary>
        public ReDetailStatus ReDetailStatus
        {
            get { return this.GetProperty(ReDetailStatusProperty); }
            set { this.SetProperty(ReDetailStatusProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.LineNo);

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
            P<MaterialReturnApplyDetailBase>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<MaterialReturnApplyDetailBase>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<string> ItemCodeProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 单位Id UniId
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位Id")]
        public static readonly Property<double> UniIdProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.UniId, p => p.Item.UnitId);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UniId
        {
            get { return this.GetProperty(UniIdProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.UnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 良品状态 ReDetailQuality
        /// <summary>
        /// 良品状态
        /// </summary>
        [Label("良品状态")]
        public static readonly Property<ReDetailQuality> ReDetailQualityProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.ReDetailQuality);

        /// <summary>
        /// 良品状态
        /// </summary>
        public ReDetailQuality ReDetailQuality
        {
            get { return this.GetProperty(ReDetailQualityProperty); }
            set { this.SetProperty(ReDetailQualityProperty, value); }
        }
        #endregion

        #region 是否启用扩展属性 EnableExtendProperty
        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        [Label("属性名")]
        public static readonly Property<bool> EnableExtendPropertyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.EnableExtendProperty, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        public bool EnableExtendProperty
        {
            get { return this.GetProperty(EnableExtendPropertyProperty); }
            set { this.SetProperty(EnableExtendPropertyProperty, value); }
        }
        #endregion

        #region 批次管控 IsBatch
        /// <summary>
        /// 批次管控
        /// </summary>
        [Label("批次管控")]
        public static readonly Property<bool> IsBatchProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.IsBatch);

        /// <summary>
        /// 批次管控
        /// </summary>
        public bool IsBatch
        {
            get { return this.GetProperty(IsBatchProperty); }
            set { this.SetProperty(IsBatchProperty, value); }
        }
        #endregion

        #region 序列号管控 IsSeri
        /// <summary>
        /// 序列号管控
        /// </summary>
        [Label("序列号管控")]
        public static readonly Property<bool> IsSeriProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.IsSeri);

        /// <summary>
        /// 序列号管控
        /// </summary>
        public bool IsSeri
        {
            get { return this.GetProperty(IsSeriProperty); }
            set { this.SetProperty(IsSeriProperty, value); }
        }
        #endregion

        #region 物料扩展属性Id ItemExtProp
        /// <summary>
        /// 物料扩展属性Id
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性Id
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料标签 ItemLabel
        /// <summary>
        /// 物料标签Id
        /// </summary>
        [Label("物料标签")]
        public static readonly IRefIdProperty ItemLabelIdProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRefId(e => e.ItemLabelId, ReferenceType.Normal);

        /// <summary>
        /// 物料标签Id
        /// </summary>
        public double? ItemLabelId
        {
            get { return (double?)this.GetRefNullableId(ItemLabelIdProperty); }
            set { this.SetRefNullableId(ItemLabelIdProperty, value); }
        }

        /// <summary>
        /// 物料标签
        /// </summary>
        public static readonly RefEntityProperty<ItemLabel> ItemLabelProperty =
            P<MaterialReturnApplyDetailBase>.RegisterRef(e => e.ItemLabel, ItemLabelIdProperty);

        /// <summary>
        /// 物料标签
        /// </summary>
        public ItemLabel ItemLabel
        {
            get { return this.GetRefEntity(ItemLabelProperty); }
            set { this.SetRefEntity(ItemLabelProperty, value); }
        }
        #endregion

        #region 退料数 ReturnQty
        /// <summary>
        /// 退料数
        /// </summary>
        [Label("退料数")]
        public static readonly Property<decimal> ReturnQtyProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.ReturnQty);

        /// <summary>
        /// 退料数
        /// </summary>
        public decimal ReturnQty
        {
            get { return this.GetProperty(ReturnQtyProperty); }
            set { this.SetProperty(ReturnQtyProperty, value); }
        }
        #endregion

        #region 在途数 OnWayQty
        /// <summary>
        /// 在途数
        /// </summary>
        [Label("在途数")]
        public static readonly Property<decimal> OnWayQtyProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.OnWayQty);

        /// <summary>
        /// 在途数
        /// </summary>
        public decimal OnWayQty
        {
            get { return this.GetProperty(OnWayQtyProperty); }
            set { this.SetProperty(OnWayQtyProperty, value); }
        }
        #endregion

        #region 收货数 CollectQty
        /// <summary>
        /// 收货数
        /// </summary>
        [Label("收货数")]
        public static readonly Property<decimal> CollectQtyProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.CollectQty);

        /// <summary>
        /// 收货数
        /// </summary>
        public decimal CollectQty
        {
            get { return this.GetProperty(CollectQtyProperty); }
            set { this.SetProperty(CollectQtyProperty, value); }
        }
        #endregion

        #region 取消数量 CancelQty
        /// <summary>
        /// 取消数量
        /// </summary>
        [Label("取消数量")]
        public static readonly Property<decimal> CancelQtyProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.CancelQty);

        /// <summary>
        /// 取消数量
        /// </summary>
        public decimal CancelQty
        {
            get { return GetProperty(CancelQtyProperty); }
            set { SetProperty(CancelQtyProperty, value); }
        }
        #endregion             

        #region 可编辑扩展属性 CanExtProp
        /// <summary>
        /// 可编辑扩展属性
        /// </summary>
        [Label("可编辑扩展属性")]
        public static readonly Property<bool> CanExtPropProperty = P<MaterialReturnApplyDetailBase>.Register(e => e.CanExtProp);

        /// <summary>
        /// 可编辑扩展属性
        /// </summary>
        public bool CanExtProp
        {
            get { return this.GetProperty(CanExtPropProperty); }
            set { this.SetProperty(CanExtPropProperty, value); }
        }
        #endregion

        #region 不映射字段
        #region 管控方式 CtrlMode
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<string> CtrlModeProperty = P<MaterialReturnApplyDetailBase>.RegisterReadOnly(
            e => e.CtrlMode, e => e.GetMode(), MaterialReturnApplyDetailBase.IsBatchProperty, MaterialReturnApplyDetailBase.IsSeriProperty);
        /// <summary>
        /// 管控方式
        /// </summary>

        public string CtrlMode
        {
            get { return this.GetProperty(CtrlModeProperty); }
        }
        private string GetMode()
        {
            var isBatch = IsBatch ? "批次".L10N() : "";
            var isSeri = IsSeri ? "序列号".L10N() : "";
            return isBatch + isSeri;
        }
        #endregion

        #region 可用数
        #region 接收数 ReceivedQty
        /// <summary>
        /// 接收数
        /// </summary>
        [Label("接收数")]
        public static readonly Property<decimal> ReceivedQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.ReceivedQty, p => p.WoDemandReport.ReceivedQty);

        /// <summary>
        /// 接收数
        /// </summary>
        public decimal ReceivedQty
        {
            get { return this.GetProperty(ReceivedQtyProperty); }
            set { this.SetProperty(ReceivedQtyProperty, value); }
        }
        #endregion

        #region 挪入数 MovedInQty
        /// <summary>
        /// 挪入数
        /// </summary>
        [Label("挪入数")]
        public static readonly Property<decimal> MovedInQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.MovedInQty, p => p.WoDemandReport.MovedInQty);

        /// <summary>
        /// 挪入数
        /// </summary>
        public decimal MovedInQty
        {
            get { return this.GetProperty(MovedInQtyProperty); }
            set { this.SetProperty(MovedInQtyProperty, value); }
        }
        #endregion

        #region 上料数 FeedQty
        /// <summary>
        /// 上料数
        /// </summary>
        [Label("上料数")]
        public static readonly Property<decimal> FeedQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.FeedQty, p => p.WoDemandReport.FeedQty);

        /// <summary>
        /// 上料数
        /// </summary>
        public decimal FeedQty
        {
            get { return this.GetProperty(FeedQtyProperty); }
            set { this.SetProperty(FeedQtyProperty, value); }
        }
        #endregion

        #region 挪出数 MovedOutQty
        /// <summary>
        /// 挪出数
        /// </summary>
        [Label("挪出数")]
        public static readonly Property<decimal> MovedOutQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.MovedOutQty, p => p.WoDemandReport.MovedOutQty);

        /// <summary>
        /// 挪出数
        /// </summary>
        public decimal MovedOutQty
        {
            get { return this.GetProperty(MovedOutQtyProperty); }
            set { this.SetProperty(MovedOutQtyProperty, value); }
        }
        #endregion

        #region 正常退料在途数 ReturnQtyInTransit
        /// <summary>
        /// 正常退料在途数
        /// </summary>
        [Label("正常退料在途数")]
        public static readonly Property<decimal> ReturnQtyInTransitProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.ReturnQtyInTransit, p => p.WoDemandReport.ReturnQtyInTransit);

        /// <summary>
        /// 正常退料在途数
        /// </summary>
        public decimal ReturnQtyInTransit
        {
            get { return this.GetProperty(ReturnQtyInTransitProperty); }
            set { this.SetProperty(ReturnQtyInTransitProperty, value); }
        }
        #endregion

        #region 不良退料在途数 NgReturnQtyInTransit
        /// <summary>
        /// 不良退料在途数
        /// </summary>
        [Label("不良退料在途数")]
        public static readonly Property<decimal> NgReturnQtyInTransitProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.NgReturnQtyInTransit, p => p.WoDemandReport.NgReturnQtyInTransit);

        /// <summary>
        /// 不良退料在途数
        /// </summary>
        public decimal NgReturnQtyInTransit
        {
            get { return this.GetProperty(NgReturnQtyInTransitProperty); }
            set { this.SetProperty(NgReturnQtyInTransitProperty, value); }
        }
        #endregion

        #region 正常退料数 WoReturnQty
        /// <summary>
        /// 正常退料数
        /// </summary>
        [Label("正常退料数")]
        public static readonly Property<decimal> WoReturnQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.WoReturnQty, p => p.WoDemandReport.ReturnQty);

        /// <summary>
        /// 正常退料数
        /// </summary>
        public decimal WoReturnQty
        {
            get { return this.GetProperty(WoReturnQtyProperty); }
            set { this.SetProperty(WoReturnQtyProperty, value); }
        }
        #endregion

        #region 不良退料数 WoNgReturnQty
        /// <summary>
        /// 不良退料数
        /// </summary>
        [Label("不良退料数")]
        public static readonly Property<decimal> WoNgReturnQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.WoNgReturnQty, p => p.WoDemandReport.NgReturnQty);

        /// <summary>
        /// 不良退料数
        /// </summary>
        public decimal WoNgReturnQty
        {
            get { return this.GetProperty(WoNgReturnQtyProperty); }
            set { this.SetProperty(WoNgReturnQtyProperty, value); }
        }
        #endregion

        #region 可用数 AvailableQty
        /// <summary>
        /// 可用数
        /// </summary>
        [Label("可用数")]
        public static readonly Property<decimal> AvailableQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterReadOnly(
            e => e.AvailableQty, e => e.GetAvailableQty());
        /// <summary>
        /// 可用数
        /// </summary>

        public decimal AvailableQty
        {
            get { return this.GetProperty(AvailableQtyProperty); }
        }
        private decimal GetAvailableQty()
        {
            return ReceivedQty + MovedInQty - FeedQty - NgQty - MovedOutQty - ReturnQtyInTransit - NgReturnQtyInTransit - WoReturnQty - WoNgReturnQty;
        }
        #endregion

        #endregion

        #region 不良数 NgQty
        /// <summary>
        /// 不良数
        /// </summary>
        [Label("不良数")]
        public static readonly Property<decimal> NgQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.NgQty, p => p.WoDemandReport.NgQty);

        /// <summary>
        /// 不良数
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 标签 Label
        /// <summary>
        /// 标签
        /// </summary>
        [Label("标签")]
        public static readonly Property<string> LabelProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.Label, p => p.ItemLabel.Label);

        /// <summary>
        /// 标签
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
        }
        #endregion

        #region 批次号 Lot
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.Lot, p => p.ItemLabel.Lot);

        /// <summary>
        /// 批次号
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
        }
        #endregion

        #region 标签可用数 LabelQty
        /// <summary>
        /// 标签可用数
        /// </summary>
        [Label("标签可用数")]
        public static readonly Property<decimal> LabelQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.LabelQty, p => p.ItemLabel.Qty);

        /// <summary>
        /// 标签可用数
        /// </summary>
        public decimal LabelQty
        {
            get { return this.GetProperty(LabelQtyProperty); }
            set { this.SetProperty(LabelQtyProperty, value); }
        }
        #endregion

        #region 标签不良数 LabelNgQty
        /// <summary>
        /// 标签不良数
        /// </summary>
        [Label("标签不良数")]
        public static readonly Property<decimal> LabelNgQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.LabelNgQty, p => p.ItemLabel.NgQty);

        /// <summary>
        /// 标签不良数
        /// </summary>
        public decimal LabelNgQty
        {
            get { return this.GetProperty(LabelNgQtyProperty); }
            set { this.SetProperty(LabelNgQtyProperty, value); }
        }
        #endregion

        #region 批次库存数 LpnQty
        /// <summary>
        /// 批次库存数
        /// </summary>
        [Label("批次库存数")]
        public static readonly Property<decimal> LpnQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.LpnQty, p => p.Lpn.AvailableQty);

        /// <summary>
        /// 批次库存数
        /// </summary>
        public decimal LpnQty
        {
            get { return this.GetProperty(LpnQtyProperty); }
            set { this.SetProperty(LpnQtyProperty, value); }
        }
        #endregion

        #region 不良批次库存数 NgLpnQty
        /// <summary>
        /// 不良批次库存数
        /// </summary>
        [Label("不良批次库存数")]
        public static readonly Property<decimal> NgLpnQtyProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.NgLpnQty, p => p.NgLpn.AvailableQty);

        /// <summary>
        /// 不良批次库存数
        /// </summary>
        public decimal NgLpnQty
        {
            get { return this.GetProperty(NgLpnQtyProperty); }
            set { this.SetProperty(NgLpnQtyProperty, value); }
        }
        #endregion

        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<MaterialReturnApplyDetailBase>.RegisterView(e => e.WorkOrderNo, p => p.MaterialReturnApply.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }

        #endregion

    }

    /// <summary>
    /// 退料申请明细实体配置
    /// </summary>
    public class MaterialReturnApplyDetailBaseConfig : EntityConfig<MaterialReturnApplyDetailBase>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MRAPPLY_DETAIL").MapAllProperties();
            Meta.Property(MaterialReturnApplyDetailBase.CtrlModeProperty).DontMapColumn();
            Meta.Property(MaterialReturnApplyDetailBase.AvailableQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
