using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class ItemLabelCriteria : Criteria
    {
        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelProperty = P<ItemLabelCriteria>.Register(e => e.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
            set { this.SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemLabelCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemLabelCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工单编号 WorkOrderNo
        /// <summary>
        /// 工单编号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ItemLabelCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单号")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<ItemLabelCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<ItemLabelCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 物料类型 ItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<ItemType?> ItemTypeProperty = P<ItemLabelCriteria>.Register(e => e.ItemType);

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType? ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
            set { this.SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        #region 条码信息来源 SourceType
        /// <summary>
        /// 条码信息来源
        /// </summary>
        [Label("条码信息来源")]
        public static readonly Property<LabelSource?> SourceTypeProperty = P<ItemLabelCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 条码信息来源
        /// </summary>
        public LabelSource? SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 显示数量为0的标签 ShowZero
        /// <summary>
        /// 显示数量为0的标签
        /// </summary>
        [Label("显示数量为0的标签")]
        public static readonly Property<bool> ShowZeroProperty = P<ItemLabelCriteria>.Register(e => e.ShowZero);

        /// <summary>
        /// 显示数量为0的标签
        /// </summary>
        public bool ShowZero
        {
            get { return this.GetProperty(ShowZeroProperty); }
            set { this.SetProperty(ShowZeroProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<ItemLabelCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotProperty = P<ItemLabelCriteria>.Register(e => e.Lot);

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot
        {
            get { return GetProperty(LotProperty); }
            set { SetProperty(LotProperty, value); }
        }
        #endregion

        #region 外部处理单位标识 Exidv
        /// <summary>
        /// 外部处理单位标识
        /// </summary>
        [Label("外部处理单位标识")]
        public static readonly Property<string> ExidvProperty = P<ItemLabelCriteria>.Register(e => e.Exidv);

        /// <summary>
        /// 外部处理单位标识
        /// </summary>
        public string Exidv
        {
            get { return this.GetProperty(ExidvProperty); }
            set { this.SetProperty(ExidvProperty, value); }
        }
        #endregion

        #region 绿标标签 Exidv2
        /// <summary>
        /// 绿标标签
        /// </summary>
        [Label("绿标标签")]
        public static readonly Property<string> Exidv2Property = P<ItemLabelCriteria>.Register(e => e.Exidv2);

        /// <summary>
        /// 绿标标签
        /// </summary>
        public string Exidv2
        {
            get { return this.GetProperty(Exidv2Property); }
            set { this.SetProperty(Exidv2Property, value); }
        }
        #endregion

        #region 状态 ItemLabelState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ItemLabelState?> ItemLabelStateProperty = P<ItemLabelCriteria>.Register(e => e.ItemLabelState);

        /// <summary>
        /// 状态
        /// </summary>
        public ItemLabelState? ItemLabelState
        {
            get { return this.GetProperty(ItemLabelStateProperty); }
            set { this.SetProperty(ItemLabelStateProperty, value); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ItemLabelCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 供应商批次 Licha
        /// <summary>
        /// 供应商批次
        /// </summary>
        [Label("供应商批次")]
        public static readonly Property<string> LichaProperty = P<ItemLabelCriteria>.Register(e => e.Licha);

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string Licha
        {
            get { return this.GetProperty(LichaProperty); }
            set { this.SetProperty(LichaProperty, value); }
        }
        #endregion

        #region MRP控制者 MrpController
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<ItemLabelCriteria>.Register(e => e.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
            set { this.SetProperty(MrpControllerProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <returns>物料标签</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemLabelController>().GetItemLabels(this);
        }
    }
}