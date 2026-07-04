using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 物料校准日志
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料校准日志")]
    public class CalibrationLog : DataEntity
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<CalibrationLog>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<CalibrationLog>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<CalibrationLog>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<CalibrationLog>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        [Required]
        public static readonly Property<string> LabelProperty = P<CalibrationLog>.Register(e => e.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
            set { this.SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 上料数量 Qty
        /// <summary>
        /// 上料数量
        /// </summary>
        [Label("上料数量")]
        [MinValue(0)]
        public static readonly Property<decimal> QtyProperty = P<CalibrationLog>.Register(e => e.Qty);

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        ////[MinValue(0)]
        public static readonly Property<decimal> RemainQtyProperty = P<CalibrationLog>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 校准数量 CalibrationQty
        /// <summary>
        /// 校准数量
        /// </summary>
        [Label("校准数量")]
        [MinValue(0)]
        public static readonly Property<decimal> CalibrationQtyProperty = P<CalibrationLog>.Register(e => e.CalibrationQty);

        /// <summary>
        /// 校准数量
        /// </summary>
        public decimal CalibrationQty
        {
            get { return this.GetProperty(CalibrationQtyProperty); }
            set { this.SetProperty(CalibrationQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(2400)]
        public static readonly Property<string> RemarkProperty = P<CalibrationLog>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 操作人 OperatorBy
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperatorByIdProperty =
            P<CalibrationLog>.RegisterRefId(e => e.OperatorById, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperatorById
        {
            get { return (double)this.GetRefId(OperatorByIdProperty); }
            set { this.SetRefId(OperatorByIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperatorByProperty =
            P<CalibrationLog>.RegisterRef(e => e.OperatorBy, OperatorByIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee OperatorBy
        {
            get { return this.GetRefEntity(OperatorByProperty); }
            set { this.SetRefEntity(OperatorByProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<CalibrationLog>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<CalibrationLog>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<CalibrationLog>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// 上料 实体配置
    /// </summary>
    internal class CalibrationLogConfig : EntityConfig<CalibrationLog>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_ITEM_CAL_LOG").MapAllProperties();
            Meta.Property(CalibrationLog.RemarkProperty).ColumnMeta.HasLength(2400);
            Meta.DisablePhantoms();
        }
    }
}